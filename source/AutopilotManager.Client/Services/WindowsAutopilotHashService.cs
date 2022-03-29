using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using AutopilotManager.Models;

namespace AutopilotManager.Services
{
    // Implementation based on .NET Framework with Sytem.Management Reference.... for max backward compatibility, 
    // supporting older Windows 10 versions with older .NET Frameworks!

    public class WindowsAutopilotHashService
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public SystemInformation FetchData()
        {
            MessageReceived(this, new MessageReceivedEventArgs { Message = "Starting CIM session" });

            var options = new ConnectionOptions
            {
                Timeout = TimeSpan.FromSeconds(30),
            };

            var scope = new ManagementScope(@"\\localhost\root\cimv2", options);
            scope.Connect();

            MessageReceived(this, new MessageReceivedEventArgs { Message = "CIM session started" });
            MessageReceived(this, new MessageReceivedEventArgs { Message = "Getting information for current system" });

            try
            {
                var osQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                var osSearcher = new ManagementObjectSearcher(scope, osQuery);
                var osQueryCollection = osSearcher.Get();
                foreach (var entry in osQueryCollection)
                {
                    var version = entry["Version"].ToString();
                    if (version.StartsWith("10."))
                    {
                        MessageReceived(this, new MessageReceivedEventArgs { Message = $"OS version [{version}] fetched" });
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                MessageReceived(this, new MessageReceivedEventArgs { Message = $"WARNING: OS version could not be fetched!" });
            }

            var information = new SystemInformation { Id = Guid.NewGuid() };


            try
            {
                var serialNumberQuery = new ObjectQuery("SELECT * FROM Win32_BIOS");
                var serialNumberSearcher = new ManagementObjectSearcher(scope, serialNumberQuery);
                var serialNumberQueryCollection = serialNumberSearcher.Get();
                foreach (var entry in serialNumberQueryCollection)
                {
                    var serialNumber = entry["SerialNumber"].ToString();
                    information.SerialNumber = serialNumber;
                    MessageReceived(this, new MessageReceivedEventArgs { Message = $"Serial number [{serialNumber}] fetched" });
                }
            }
            catch (Exception)
            {
                MessageReceived(this, new MessageReceivedEventArgs { Message = $"WARNING: Serial number could not be fetched!" });
            }

            try
            {
                string model = string.Empty;
                var systemInformationQuery = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
                var systemInformationSearcher = new ManagementObjectSearcher(scope, systemInformationQuery);
                var systemInformationQueryCollection = systemInformationSearcher.Get();
                foreach (var entry in systemInformationQueryCollection)
                {
                    var manufacturer = entry["Manufacturer"].ToString();
                    if (manufacturer.ToLower().Contains("lenovo"))
                    {
                        var lenovoSystemInformationQuery = new ObjectQuery("SELECT * FROM Win32_ComputerSystemProduct");
                        var lenovoSystemInformationSearcher = new ManagementObjectSearcher(scope, lenovoSystemInformationQuery);
                        var lenovoSystemInformationQueryCollection = lenovoSystemInformationSearcher.Get();
                        foreach (var systemEntry in lenovoSystemInformationQueryCollection)
                        {
                            model = systemEntry["Version"].ToString();
                            break;
                        }
                    }
                    else
                    {
                        model = entry["Model"].ToString(); 
                    }
                    information.Manufacturer = manufacturer;
                    information.Model = model;
                    MessageReceived(this, new MessageReceivedEventArgs { Message = $"Device manufacturer [{manufacturer}] and model [{model}] fetched" });
                }
            }
            catch (Exception)
            {
                MessageReceived(this, new MessageReceivedEventArgs { Message = $"WARNING: Manufacturer and/or model could not be fetched!" });
            }

            try
            {
                scope.Path = new ManagementPath(@"\\localhost\root\cimv2\mdm\dmmap");
                scope.Connect();
                var hardwareHashQuery = new ObjectQuery("SELECT * FROM MDM_DevDetail_Ext01 WHERE InstanceID='Ext' AND ParentID='./DevDetail'");
                var hardwareHashSearcher = new ManagementObjectSearcher(scope, hardwareHashQuery);
                var hardwareHashQueryCollection = hardwareHashSearcher.Get();
                if (hardwareHashQueryCollection.Count < 1)
                {
                    MessageReceived(this, new MessageReceivedEventArgs { Message = "Device hash not found!" });
                }
                foreach (var entry in hardwareHashQueryCollection)
                {
                    var hardwareHash = entry["DeviceHardwareData"].ToString();
                    information.HardwareHash = hardwareHash;
                    MessageReceived(this, new MessageReceivedEventArgs { Message = $"Device hash [{hardwareHash.Substring(0, 5)}...] fetched" });
                }
            }
            catch (ManagementException ex)
            {
                MessageReceived(this, new MessageReceivedEventArgs { Message = $"ERROR: DeviceHardwareData {ex.Message}(probably not running as system)" });
            }

            return information;           
        }
    }
}
