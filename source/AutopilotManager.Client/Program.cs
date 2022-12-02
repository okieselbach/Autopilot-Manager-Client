using System;
using System.Linq;
using System.Threading.Tasks;
using AutopilotManager.Models;
using AutopilotManager.Services;
using AutopilotManager.Utils;
using AutopilotManager.Clients;
using static QRCoder.PayloadGenerator;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Net;
using System.Runtime;
using System.Windows;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Net.Cache;

namespace AutopilotManager.Client
{
    // Initially configured AutopilotManager.Client to .NET Framework 4.6 as this is the lowest common Framework supported by every Windows 10 version released.
    // This ensures best compatibility to run the ap.exe without any issues. .NET Standard and .NET Freamework 4.7.2 is not available with every Windows 10 version
    // e.g. System.Net.Http in version 4.2.0 has a breaking change and when referenced you loose support in older Windows 10 versions. So we make sure to use version 4.0.0

    // due to incompatibilities with newer Windows OS versions (21H2) I switched .NET Framework to 4.8 and native 64-bit binary!
    // according to the docs Windows 10 May 2019 Update (version 1903) is the oldest with preinstalled .NET 4.8. This is the lowest Windows OS version we are support from now on.
    // https://docs.microsoft.com/en-us/dotnet/framework/get-started/system-requirements

    // M. Niehaus created a script which works similar to the AutopilotManager/client solution, but targeted (imho) more or less for a IT personal to onboard a device on the fly
    // https://oofhours.com/2020/07/13/automating-the-windows-autopilot-device-hash-import-and-profile-assignment-process/
    // AutopilotManager in combination with this client (this project) is more targeted to the user for self onboarding without exposing credentials
    class Program
    {
        private static LogUtil _logger = null;
        private static string _backendUrl = string.Empty;
        private static Models.SystemInformation _systemInformation = null;
        private static BackendClient _backendClient = null;
        private static WindowsAutopilotHashService _windowsAutopilotHashService = null;
        private static string _preCheckErrorMessage = string.Empty;
        private static bool _skipEndpointsValidation = false;
        private static bool _endpointsValidationResult = true;
        private static bool _endpointsValidationOnly = false;
        private static bool _ignoreAutopilotAssignment = false;
        private static bool _fetchHardwareDataOnly = false;
        private static bool _deleteManagedDeviceOnly = false;
        private static bool _bypassExtendedValidationAndFallbackToManualApproval = false;
        private static bool _skipClientVersionCheck = false;
        private const string _updateXmlUri = "https://github.com/okieselbach/Autopilot-Manager-Client/raw/master/dist/update.xml";
        private static string _version = string.Empty;
        private static string _newVersion = string.Empty;
        private static string _updateTempFileName = string.Empty;
        
        // ap.exe should be called from OOBE [Shift] + [F10] command prompt via:

        // powershell -c iwr ap.domain.com -o ap.exe & qr
        // or
        // curl -o ap.exe ap.domain.com & ap

        // curl remarks:
        // -L is used to follow redirects e.g. redirect to https
        // curl -o ap.exe -L ap.domain.com & ap
        static async Task Main(string[] args)
        {
            _logger = new LogUtil();
            _version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

            _logger.WriteInfo($"AutopilotManager.Client v{_version}");
            _backendUrl = ParseCommandlineArgs(args);
            
            if (!_skipClientVersionCheck)
            {
                _logger.WriteInfo("Checking for new client version...");
                if (await CheckForNewClientVersion())
                {
                    _logger.WriteInfo($"Found new client version {_newVersion}, updating now and restarting process...");
                    UpdateClient();
                }
            }

            _backendClient = new BackendClient();
            _backendClient.MessageReceived += BackendClient_MessageReceived;
            _backendClient.ResultReceived += BackendClient_ResultReceived;
            _windowsAutopilotHashService = new WindowsAutopilotHashService();
            _windowsAutopilotHashService.MessageReceived += WindowsAutopilotHashService_MessageReceived;

            try
            {
                // default Action is IMPORT
                var action = "IMPORT";

                if (_bypassExtendedValidationAndFallbackToManualApproval)
                {
                    Debug.WriteLine("Identified as device [BYPASS] request");
                    action = "BYPASS";
                }

                // --erase device deletion request found
                if (_deleteManagedDeviceOnly)
                {
                    _logger.WriteInfo("Identified as device [DELETE] request");
                    action = "DELETE";
                }
                else
                {
                    _logger.WriteInfo("Identified as device [IMPORT] request");

                    // --skip endpoint validation found
                    if (!_skipEndpointsValidation)
                    {
                        var endpointVerificationError = false;
                        if (_endpointsValidationResult = await _backendClient.VerifyEnrollmentEndpoints())
                        {
                            _logger.WriteInfo("All enrollment URLs reachable");
                        }
                        else
                        {
                            if (!_logger.DebugMode)
                            {
                                _logger.WriteInfo("Not all enrollment URLs reachable or system time is wrong!");
                                _logger.WriteInfo($"Use '{Assembly.GetExecutingAssembly().GetName().Name} --connect --verbose' for connectivity test details.");
                                endpointVerificationError = true;
                            }
                            else
                            {
                                _logger.WriteInfo("=> Not all enrollment URLs reachable or system time is wrong!");
                                endpointVerificationError = true;
                            }
                        }
                        // --connect endpoint validation only found
                        if (_endpointsValidationOnly)
                        {
                            if (endpointVerificationError)
                            {
                                Environment.Exit(1);
                            }
                            return;
                        }
                    }

                    // --ignore Autopilot assignment found
                    if (!_ignoreAutopilotAssignment)
                    {
                        try
                        {
                            // is this already a Autopilot device?
                            var rkbase = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                            using (RegistryKey key = rkbase.OpenSubKey(@"SOFTWARE\Microsoft\Provisioning\Diagnostics\AutoPilot"))
                            {
                                if (key != null)
                                {
                                    var tenantId = key.GetValue("CloudAssignedTenantId").ToString();
                                    if (!string.IsNullOrEmpty(tenantId))
                                    {
                                        _logger.WriteInfo("Device is already Autopilot provisioned");
                                        _logger.WriteDebug($"CloudAssignedTenantId: {tenantId}");

                                        var tenantDomain = key.GetValue("CloudAssignedTenantDomain").ToString();
                                        if (!string.IsNullOrEmpty(tenantDomain))
                                        {
                                            if (_logger.DebugMode)
                                            {
                                                _logger.WriteDebug($"CloudAssignedTenantDomain: {tenantDomain}");
                                            }
                                            else
                                            {
                                                _logger.WriteInfo($"Assigned tenant domain: {tenantDomain}");
                                            }
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            _logger.WriteInfo("Couldn't check existing Autopilot assignment, proceeding.");
                        }
                    }
                }

                // main work start now
                _logger.WriteInfo("Fetching system information");
                _systemInformation = _windowsAutopilotHashService.FetchData();
                _systemInformation.Action = action;

                if (_systemInformation == null)
                {
                    _logger.WriteInfo("No Windows 10/11 operating system, can't fetch data");
                    return;
                }

                if (string.IsNullOrEmpty(_systemInformation.HardwareHash))
                {
                    _logger.WriteInfo("Information couldn't be fetched");
                }
                else
                {
                    _logger.WriteInfo("Information successfully fetched");
                    if (_fetchHardwareDataOnly)
                    {
                        return;
                    }

                    // check if our backend server is reachable
                    if (!_backendClient.IsValidUrl(_backendUrl, out _backendUrl, "HEAD"))
                    {
                        _logger.WriteInfo("Backend URL not reachable");
                        return;
                    }
                    else
                    {
                        _logger.WriteDebug($"Resolved backend URL is {_backendUrl}");
                    }

                    // Safe the info to app service AutopilotManager... result here may be 'not allowed', catched in ResultReceived handler
                    await _backendClient.SaveDataAsync(_systemInformation, _backendUrl);

                    var url = new Url($"{_backendUrl}/autopilot/{_systemInformation.Id}/save-information");
                    var qrCodeImage = QrCodeUtil.GenerateQrCode(url.ToString());
                    //var qrCodeImage = QrCodeUtil.GenerateQrCode(url.ToString(), 8);

                    var urlNoSelfService = new Url($"{_backendUrl}/Home/NoSelfService");
                    var qrCodeNoSelfServiceImage = QrCodeUtil.GenerateQrCode(urlNoSelfService.ToString());

                    var form = new QrCodeForm(qrCodeImage, qrCodeNoSelfServiceImage, _systemInformation, _backendClient, _backendUrl, _preCheckErrorMessage, _endpointsValidationResult);
                    form.DisplayData();

                    if (form.DialogResult == DialogResult.Retry)
                    {
                        _logger.WriteDebug("Retry received");

                        string[] arguments = Environment.GetCommandLineArgs();

                        var path = Assembly.GetExecutingAssembly().Location;
                        var fullPathcommand = $"{path} {string.Join(" ", arguments.Skip(1).ToArray())}";

                        _logger.WriteDebug($"Using command: {fullPathcommand}");
                        _logger.WriteDebug("Starting as external process, debug output will not be received in this command prompt.");
                        _logger.WriteDebug("Start manually (not using Retry button) from this command prompt to receive the debug output again.");

                        using (var p = new Process())
                        {
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.FileName = path;
                            p.StartInfo.Arguments = string.Join(" ", arguments.Skip(1).ToArray());
                            p.StartInfo.CreateNoWindow = true;
                            p.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteInfo(ex.ToString());
                _logger.WriteInfo("**************** Closing application, fix and porting try again ****************");
                return;
            }
        }

        private static string ParseCommandlineArgs(string[] args)
        {
            var backendUrl = string.Empty;

            if (args.Any())
            {
                foreach (var arg in args)
                {
                    // this will handle most arg option like -v --V -verbose -trace -debug /v /debug ...
                    if (arg.StartsWith("-") || arg.StartsWith("/"))
                    {
                        switch (arg.TrimStart(new[] { '-', '/' })[0].ToString().ToLower())
                        {
                            case "v":
                            case "d":
                                _logger.DebugMode = true;
                                _logger.WriteDebug($"Enabled verbose output");
                                break;
                            case "t":
                                _logger.TraceMode = true;
                                _logger.WriteTrace($"Enabled trace output");
                                break;
                            case "s":
                                _skipEndpointsValidation = true;
                                _logger.WriteDebug($"Enrollment URL validation skipped");
                                break;
                            case "c":
                                _endpointsValidationOnly = true;
                                _logger.WriteDebug($"Enrollment URL validation only mode");
                                break;
                            case "i":
                                _ignoreAutopilotAssignment = true;
                                _logger.WriteDebug($"Ignoring current Autopilot assignment and proceeding");
                                break;
                            case "f":
                                _fetchHardwareDataOnly = true;
                                _logger.WriteDebug($"Fetch hardware data only");
                                break;
                            case "e":
                                _deleteManagedDeviceOnly = true;
                                _logger.WriteDebug($"Delete managed device only");
                                break;
                            case "g":
                                _skipClientVersionCheck = true;
                                _logger.WriteDebug($"Client Verison check skipped");
                                break;
                            case "b":
                                _bypassExtendedValidationAndFallbackToManualApproval = true;
                                Debug.WriteLine($"Parameter for [BYPASS] request identified");
                                break;
                            case "?":
                            case "h":
                                _logger.WriteInfo($"2022 by Oliver Kieselbach (oliverkieselbach.com)");
                                _logger.WriteInfo("");
                                _logger.WriteInfo($"USAGE: {Assembly.GetExecutingAssembly().GetName().Name} <URL> [options...]");
                                _logger.WriteInfo("");
                                _logger.WriteInfo("URL must point to the AutopilotManager.Server");
                                _logger.WriteInfo("");
                                _logger.WriteInfo($"-v, --verbose, /v, /verbose   enable verbose output");
                                _logger.WriteInfo($"-d, --debug, /d, / debug      enable verbose output (same output as -v)");
                                _logger.WriteInfo($"-t, --trace, /t, /trace       enables result-check trace output");
                                _logger.WriteInfo($"-s, --skip, /s, /skip         skip endpoint enrollment verification");
                                _logger.WriteInfo($"-c, --connect, /c, /connect   endpoint enrollment verification only");
                                _logger.WriteInfo($"-i, --ignore, /i, /ignore     ignore current Autopilot assignment and proceed");
                                _logger.WriteInfo($"-f, --fetch, /f, /fetch       fetch only hardware data, no result is send");
                                _logger.WriteInfo($"-e, --erase, /e, /erase       send delete device request, enabled server support is needed and");
                                _logger.WriteInfo($"                              deletion requests are only processed with enabled approval mode");
                                _logger.WriteInfo($"-g                            skip client version check on startup");
                                Environment.Exit(0);
                                break;
                            default:
                                break;
                        }
                    }
                    else if (arg.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        backendUrl = arg;
                    }
                    else
                    {
                        // by default we use secure endpoints
                        backendUrl = $"https://{args[0]}";
                    }

                }
                // we got the backend URL as parameter
                if (!string.IsNullOrEmpty(backendUrl))
                {
                    _logger.WriteDebug($"Provided backend URL is {backendUrl}");
                    return backendUrl;
                }
                else
                {
                    _logger.WriteDebug($"No backend URL provided");
                }
            }

            backendUrl = ConfigUtil.GetBackendUrl();
            if (!string.IsNullOrEmpty(backendUrl))
            {
                _logger.WriteDebug($"Provided backend URL is {backendUrl}");
            }
            else
            {
                _logger.WriteDebug($"No app config backend URL defined");
            }
            
            return backendUrl;
        }

        private static void WindowsAutopilotHashService_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            _logger.WriteDebug(e.Message);
        }

        private static void BackendClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            _logger.WriteDebug(e.Message);
        }

        private static void BackendClient_ResultReceived(object sender, ResultEventArgs e)
        {
            _preCheckErrorMessage = e.Message;
            _logger.WriteDebug($"Received result: {e.Message}");
            _logger.WriteTrace($"Received result: {e.Message}");
        }

        private static async Task<bool> CheckForNewClientVersion()
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    var systemWebProxy = WebRequest.GetSystemWebProxy();
                    systemWebProxy.Credentials = CredentialCache.DefaultCredentials;
                    webClient.Proxy = systemWebProxy;
                    webClient.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

                    // Download update.xml file
                    var data = await webClient.DownloadDataTaskAsync(new Uri(_updateXmlUri));
                    var xDocument = XDocument.Load(new MemoryStream(data));

                    var url = xDocument.XPathSelectElement("./LatestVersion/DownloadURL")?.Value;
                    var version = xDocument.XPathSelectElement("./LatestVersion/VersionNumber")?.Value;
                    _newVersion = version;

                    if (url == null || !url.StartsWith("https")) return false;
                    if (version == null) return false;
                    if (string.CompareOrdinal(version, 0, _version, 0, version.Length) <= 0) return false;

                    _updateTempFileName = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}");
                    if (_updateTempFileName == null) return false;

                    // Download new binary
                    await webClient.DownloadFileTaskAsync(new Uri(url), _updateTempFileName);

                    // simple sanity check, bigger than 10KB? we assume it is not a dummy or broken binary (e.g. 0 KB file)
                    if (!File.Exists(_updateTempFileName) || new FileInfo(_updateTempFileName).Length < 1024 * 10)
                    {
                        _logger.WriteDebug($"Download failed.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteDebug($"Download failed: {ex.Message}");
                return false;
            }

            return true;
        }

        private static void UpdateClient()
        {
            var path = Assembly.GetExecutingAssembly().Location;

            // normalize command line arguments for correct PS execution
            string[] arguments = Environment.GetCommandLineArgs();
            string argumentsString = String.Empty;

            if (arguments.Length > 1)
            {
                argumentsString = "-ArgumentList '" + string.Join("','", arguments.Skip(1).ToArray()).Trim() + "'";
            }

            try
            {
                // call a separate process (PowerShell) to overwrite the app binaries after app shutdown...
                // to give the app enough time for shutdown we wait 1 seconds before replacing - prevent currently in use scenarios
                using (var p = new Process())
                {
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.FileName = "PowerShell.exe";
                    p.StartInfo.Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"& { " +
                                            "Start-Sleep 1; " +
                                            $"Copy-Item -Path '{_updateTempFileName}' -Destination '{path}' -Force; " +
                                            $"Remove-Item -Path '{_updateTempFileName}' -Force; " +
                                            $"Start-Process -FilePath '{path}' {argumentsString}" + 
                                            "}\"";
                    p.StartInfo.CreateNoWindow = true;

                    Debug.WriteLine($"{p.StartInfo.FileName} {p.StartInfo.Arguments}");

                    p.Start();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteDebug($"UpdateClient failed: {ex.Message}");
                try
                {
                    if (_updateTempFileName != null)
                    {
                        if (!File.Exists(_updateTempFileName))
                            File.Delete(_updateTempFileName);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            Environment.Exit(0);
        }
    }
}