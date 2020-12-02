using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Options;
using System;
using System.Linq;
using AutopilotManager.Models;

namespace AutopilotManager.Services
{
    // Implementation based on .NET Standard 2.0 and Microsoft.Management.Infrastructure Reference
    // due to compatibility for old Windows 10 versions this is not used...

    public class WindowsAutopilotHashService
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public SystemInformation FetchData()
        {
            var information = new SystemInformation { Id = Guid.NewGuid() };
            var sessionOptions = new DComSessionOptions
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
            MessageReceived(this, new MessageReceivedEventArgs { Message = "Starting CIM session" });s
            var cimSession = CimSession.Create("localhost", sessionOptions);
            MessageReceived(this, new MessageReceivedEventArgs { Message = "CIM session started" });
            MessageReceived(this, new MessageReceivedEventArgs { Message = "Getting information for current system" });


            var serialNumberQuery = "SELECT * FROM Win32_BIOS";
            var serialNumberInstances = cimSession.QueryInstances(@"root\cimv2", "WQL", serialNumberQuery);
            var serialNumberInstancesResponse = serialNumberInstances.ToList();
            if (serialNumberInstances.Any())
            {
                var serialNumber = serialNumberInstancesResponse[0].CimInstanceProperties.Single(x => x.Name.Equals("SerialNumber")).Value.ToString();
                information.SerialNumber = serialNumber;
                MessageReceived(this, new MessageReceivedEventArgs { Message = "Serial number fetched" });
            }

            var systemInformationQuery = "SELECT * FROM Win32_ComputerSystem";
            var systemInformationInstances = cimSession.QueryInstances(@"root\cimv2", "WQL", systemInformationQuery);
            var systemInformationInstancesResponse = systemInformationInstances.ToList();
            if (systemInformationInstancesResponse.Any())
            {
                var manufacturer = systemInformationInstancesResponse[0].CimInstanceProperties.Single(x => x.Name.Equals("Manufacturer")).Value.ToString();
                var model = systemInformationInstancesResponse[0].CimInstanceProperties.Single(x => x.Name.Equals("Model")).Value.ToString();
                information.Manufacturer = manufacturer;
                information.Model = model;
                MessageReceived(this, new MessageReceivedEventArgs { Message = "Device make and model fetched" });

            }

            string hardwareHashQuery = "SELECT * FROM MDM_DevDetail_Ext01 WHERE InstanceID='Ext' AND ParentID='./DevDetail'";
            var hardwareHashInstances = cimSession.QueryInstances(@"root\cimv2\mdm\dmmap", "WQL", hardwareHashQuery);
            var hardwareHashInstancesResponse = hardwareHashInstances.ToList();
            if (hardwareHashInstancesResponse.Any())
            {
                var hash = hardwareHashInstancesResponse[0].CimInstanceProperties.Single(x => x.Name.Equals("DeviceHardwareData")).Value.ToString();
                information.HardwareHash = hash;
                MessageReceived(this, new MessageReceivedEventArgs { Message = "Device hash fetched" });
            }
            else
            {
                MessageReceived(this, new MessageReceivedEventArgs { Message = "Device hash not found!" });
            }

            return information;
        }
    }
}
