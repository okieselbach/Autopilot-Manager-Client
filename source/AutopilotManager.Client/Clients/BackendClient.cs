using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutopilotManager.Models;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Diagnostics;

namespace AutopilotManager.Clients
{
    public class BackendClient
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler<ResultEventArgs> ResultReceived;
        private readonly Dictionary<string, int> _requiredEnrollmentUrlsTcpConnect;
        private readonly string _ntpAddress;
        private readonly Stopwatch _stopWatch;

        public BackendClient()
        {
            _stopWatch = new Stopwatch();

            // Enrollment Endpoints
            // https://docs.microsoft.com/en-us/mem/autopilot/networking-requirements
            // https://docs.microsoft.com/en-us/mem/intune/fundamentals/intune-endpoints
            // https://oofhours.com/2019/09/03/windows-autopilot-device-provisioning-network-traffic-annotated/
            // https://www.anoopcnair.com/windows-autopilot-in-depth-processes-part-3/

            _ntpAddress = "time.windows.com";

            _requiredEnrollmentUrlsTcpConnect = new Dictionary<string, int>
            {
                { "www.msftconnecttest.com", 80 },
                { "ztd.dds.microsoft.com", 443 },
                { "cs.dds.microsoft.com", 443 },
                { "login.live.com", 443 },
                { "login.microsoftonline.com", 443 },
                { "aadcdn.msauth.net", 443 },
                { "licensing.mp.microsoft.com", 443 },
                { "EnterpriseEnrollment.manage.microsoft.com", 443 },
                { "EnterpriseEnrollment-s.manage.microsoft.com", 443 },
                { "EnterpriseRegistration.windows.net", 443 },
                { "portal.manage.microsoft.com", 443 },
                { "enrollment.manage.microsoft.com", 443 },
                { "fe2cr.update.microsoft.com", 443 }, //OOBE-ZDP
                { "euprodimedatapri.azureedge.net", 443 }, // Europe
                { "euprodimedatasec.azureedge.net", 443 }, // Europe
                { "euprodimedatahotfix.azureedge.net", 443 }, // Europe
                //{ "naprodimedatapri.azureedge.net", 443 }, // North America
                //{ "naprodimedatasec.azureedge.net", 443 }, // North America
                //{ "naprodimedatahotfix.azureedge.net", 443 }. // North America
                //{ "approdimedatapri.azureedge.net", 443 }, // Asia Pacific
                //{ "approdimedatasec.azureedge.net", 443 }  // Asia Pacific
                //{ "approdimedatahotfix.azureedge.net", 443 }. // Asia Pacific

                // tokenprovider.termsofuse.identitygovernance.azure.com
                // wdcp.microsoft.com
            };
        }

        public bool IsValidUrl(string url, out string resolvedUrl, string method = "GET")
        {
            resolvedUrl = url;
            try
            {
                // First we check for redirects to support URL shorteners 
                var httpWebrequest = WebRequest.Create(url) as HttpWebRequest;
                httpWebrequest.AllowAutoRedirect = false;
                using (var response = httpWebrequest.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.Redirect ||
                        response.StatusCode == HttpStatusCode.MovedPermanently)
                    {
                        resolvedUrl = response.Headers["Location"];
                    }
                }

                var webRequest = WebRequest.Create(resolvedUrl);
                webRequest.Timeout = 5000;
                webRequest.Method = method;

                using (var response = webRequest.GetResponse() as HttpWebResponse)
                {
                    response.Close();
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static DateTime? VerifyNtpEndpoint(string address)
        {
            try
            {
                // implementation based on this suggestion: https://stackoverflow.com/a/20157068

                var ntpData = new byte[48];
                ntpData[0] = 0x1B;

                //0x1B or 00 011 011

                //   0                   1                   2                   3
                // 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
                //+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
                //|LI | VN  |Mode |    Stratum     |      Poll     |  Precision   |
                //+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

                // Leap Indicator = 0 (no warning)
                // Version Number = 3 (IPv4 only)
                // Mode           = 3 (Client Mode)

                var addresses = Dns.GetHostEntry(address).AddressList;
                var ipEndPoint = new IPEndPoint(addresses[0], 123);
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
                {
                    ReceiveTimeout = 3000
                };
                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();

                ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
                ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                var networkDateTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(milliseconds);

                return networkDateTime; // UTC
                // return networkDateTime.ToLocalTime();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public async Task<bool> VerifyEnrollmentEndpoints()
        {
            bool endpointValidationResult = true;

            var time = VerifyNtpEndpoint(_ntpAddress);
            if (time != null)
            {
                MessageReceived(this, new MessageReceivedEventArgs { Message = $"NTP: {_ntpAddress,-50} -> reachable" });
                MessageReceived(this, new MessageReceivedEventArgs { Message = $"Received time: {time}" });
                MessageReceived(this, new MessageReceivedEventArgs { Message = $"System time:   {DateTime.UtcNow}" });
                TimeSpan span = DateTime.UtcNow.Subtract(time.Value);
                var minutesDiff = Math.Ceiling(Math.Abs(span.TotalMinutes));
                if (minutesDiff >= 5)
                {
                    MessageReceived(this, new MessageReceivedEventArgs { Message = $"=> Time difference is about {minutesDiff} minutes!" });

                    // treat the check as failed if more than 15 minutes off, as this might result in certificate validation problems.
                    if (minutesDiff > 15)
                    {
                        MessageReceived(this, new MessageReceivedEventArgs { Message = $"=> Time difference is more than {minutesDiff} minutes -> fail, please correct system time!" });
                        endpointValidationResult = false;
                    }
                }
            }
            else
            {
                MessageReceived(this, new MessageReceivedEventArgs { Message = $"NTP: {_ntpAddress,-50} -> not reachable" });
                endpointValidationResult = false;
            }

            TcpClient tcpClient;
            foreach (var url in _requiredEnrollmentUrlsTcpConnect)
            {
                tcpClient = new TcpClient();
                try
                {
                    await tcpClient.ConnectAsync(url.Key, url.Value);
                    if (tcpClient.Connected)
                    {
                        MessageReceived(this, new MessageReceivedEventArgs { Message = $"URL: {url,-50} -> reachable" });
                    }
                    else
                    {
                        MessageReceived(this, new MessageReceivedEventArgs { Message = $"URL: {url,-50} -> not reachable" });
                        endpointValidationResult = false;
                    }
                }
                catch (Exception)
                {
                    MessageReceived(this, new MessageReceivedEventArgs { Message = $"URL: {url,-50} -> not reachable" });
                    endpointValidationResult = false;
                }
                finally
                {
                    tcpClient.Close();
                    tcpClient.Dispose();
                }
            }

            return endpointValidationResult;
        }

        public async Task SaveDataAsync(SystemInformation systemInformation, string backendUrl)
        {
            _stopWatch.Start();

            string apiEndpoint = $"/api-autopilot/{systemInformation.Id}/save-information";
            var payload = JsonConvert.SerializeObject(systemInformation);
            using (var client = new HttpClient { BaseAddress = new Uri(backendUrl) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                MessageReceived(this, new MessageReceivedEventArgs { Message = $"[{_stopWatch.Elapsed}] Posting data to {backendUrl.TrimEnd('/')}{apiEndpoint}" });
                MessageReceived(this, new MessageReceivedEventArgs { Message = $"[{_stopWatch.Elapsed}] Payload is {payload}" });

                var httpResponseMessage = await client.PostAsync(apiEndpoint, new StringContent(payload, Encoding.UTF8, "application/json"));
                var responseToLog = new
                {
                    statusCode = httpResponseMessage.StatusCode,
                    content = await httpResponseMessage.Content.ReadAsStringAsync(),
                    headers = httpResponseMessage.Headers,
                    reasonPhrase = httpResponseMessage.ReasonPhrase,
                    errorMessage = httpResponseMessage.RequestMessage
                };
                MessageReceived(this, new MessageReceivedEventArgs { Message = $"[{_stopWatch.Elapsed}] Response is {JsonConvert.SerializeObject(responseToLog)}" });

                switch (httpResponseMessage.StatusCode)
                {
                    case HttpStatusCode.OK:
                        // 200
                        MessageReceived(this, new MessageReceivedEventArgs { Message = $"[{_stopWatch.Elapsed}] Data posted successfully" });
                        break;
                    case HttpStatusCode.BadRequest:
                        // 400
                        // e.g. Model or Manufacturer not allowed.
                        ResultReceived(this, new ResultEventArgs { Message = responseToLog.content.Trim('"') });
                        break;
                }

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    MessageReceived(this, new MessageReceivedEventArgs { Message = $"[{_stopWatch.Elapsed}] Status code is {httpResponseMessage.StatusCode}" });
                    MessageReceived(this, new MessageReceivedEventArgs { Message = $"[{_stopWatch.Elapsed}] Reason phrase is {httpResponseMessage.ReasonPhrase}" });
                }

                //httpResponseMessage.EnsureSuccessStatusCode();
                //MessageReceived(this, new MessageReceivedEventArgs { Message = "Data posted successfully" });
            }
        }

        public async Task GetResultAsync(SystemInformation systemInformation, string backendUrl)
        {
            string apiEndpoint = $"/api-autopilot/{systemInformation.Id}/result";

            using (var client = new HttpClient { BaseAddress = new Uri(backendUrl) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                MessageReceived(this, new MessageReceivedEventArgs { Message = $"[{_stopWatch.Elapsed}] Getting data from {backendUrl.TrimEnd('/')}{apiEndpoint}" });

                var httpResponseMessage = await client.GetAsync(apiEndpoint);
                var responseToLog = new
                {
                    statusCode = httpResponseMessage.StatusCode,
                    content = await httpResponseMessage.Content.ReadAsStringAsync(),
                    headers = httpResponseMessage.Headers,
                    reasonPhrase = httpResponseMessage.ReasonPhrase,
                    errorMessage = httpResponseMessage.RequestMessage
                };
                MessageReceived(this, new MessageReceivedEventArgs { Message = $"[{_stopWatch.Elapsed}] Response is {JsonConvert.SerializeObject(responseToLog)}" });

                switch (httpResponseMessage.StatusCode)
                {
                    case HttpStatusCode.Created:
                        // 201
                        if (responseToLog.content.ToLower().Contains("approvalmode"))
                        {
                            ResultReceived(this, new ResultEventArgs { Message = "ApprovalMode" });
                        }
                        ResultReceived(this, new ResultEventArgs { Message = "Queued" });
                        break;
                    case HttpStatusCode.Accepted:
                        // 202
                        ResultReceived(this, new ResultEventArgs { Message = "Processing" });
                        break;
                    case HttpStatusCode.NoContent:
                        // 204
                        ResultReceived(this, new ResultEventArgs { Message = "Success" });
                        _stopWatch.Reset();
                        break;
                    case HttpStatusCode.Conflict:
                        // 409
                        if (responseToLog.content.ToLower().Contains("806"))
                        {
                            ResultReceived(this, new ResultEventArgs { Message = "Already registered" });
                        }
                        else if (responseToLog.content.ToLower().Contains("808"))
                        {
                            ResultReceived(this, new ResultEventArgs { Message = "Registered elswere" });
                        }
                        _stopWatch.Reset();
                        break;
                    case HttpStatusCode.Gone:
                        // 410
                        ResultReceived(this, new ResultEventArgs { Message = "Timed out" });
                        _stopWatch.Reset();
                        break;
                    default:
                        // 500 and others
                        ResultReceived(this, new ResultEventArgs { Message = "Error occured" });
                        _stopWatch.Reset();
                        break;
                }
            }
        }
    }
}
