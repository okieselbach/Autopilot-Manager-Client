using System;
using System.Configuration;
using System.Reflection;

namespace AutopilotManager.Utils
{
    public static class ConfigUtil
    {
        public static string GetBackendUrl()
        {
            //return "https://autopilotmanager-XXXX.azurewebsites.net";
            return GetConfigValue("BackendUrl");
        }
        public static string GetConfigValue(string name)
        {
            string response = ConfigurationManager.AppSettings.Get(name);
            if (string.IsNullOrEmpty(response))
            {
                return string.Empty;
                //throw new ApplicationException($"Config value not found in setting for {name}");
            }
            return response;
        }
    }
}
