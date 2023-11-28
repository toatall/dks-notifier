using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier
{
    internal class ConfigurationManagerReader
    {      
        public static string AppSettingRead(string paramName, string defaultValue)
        {
            return ConfigurationManager.AppSettings[paramName]?.ToString() ?? defaultValue;
        }

        public static int AppSettingRead(string paramName, int defaultValue)
        {
            int res;
            if (int.TryParse(ConfigurationManager.AppSettings[paramName]?.ToString(), out res))
            {
                return res;
            }
            return defaultValue;
        }

        public static bool AppSettingRead(string paramName, bool defaultValue)
        {
            bool res;
            if (bool.TryParse(ConfigurationManager.AppSettings[paramName]?.ToString(), out res))
            {
                return res;
            }
            return defaultValue;
        }

        public static string[] AppSettingRead(string paramName, char separator = ',')
        {            
            string vals = ConfigurationManager.AppSettings[paramName]?.ToString() ?? null;
            if (!string.IsNullOrEmpty(vals))
            {
                return vals.Split(separator);
            }
            return new string[] { };
        }



        public static string ConnectionStringRead(string paramName)
        {            
            return ConfigurationManager.ConnectionStrings[paramName].ConnectionString;
        }
        
    }
}
