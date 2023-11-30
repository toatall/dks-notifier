using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier
{
    /// <summary>
    /// Чтение настроек
    /// </summary>
    internal class ConfigurationManagerReader
    {      
        /// <summary>
        /// Текстовый параметр
        /// </summary>
        /// <param name="paramName">имя параметра</param>
        /// <param name="defaultValue">значение по умолчанию</param>
        /// <returns></returns>
        public static string AppSettingRead(string paramName, string defaultValue)
        {
            return ConfigurationManager.AppSettings[paramName]?.ToString() ?? defaultValue;
        }

        /// <summary>
        /// Числовой параметр
        /// </summary>
        /// <param name="paramName">имя параметра</param>
        /// <param name="defaultValue">значение по умолчанию</param>
        /// <returns></returns>
        public static int AppSettingRead(string paramName, int defaultValue)
        {
            int res;
            if (int.TryParse(ConfigurationManager.AppSettings[paramName]?.ToString(), out res))
            {
                return res;
            }
            return defaultValue;
        }

        /// <summary>
        /// Логический параметр
        /// </summary>
        /// <param name="paramName">имя параметра</param>
        /// <param name="defaultValue">значение по умолчанию</param>
        /// <returns></returns>
        public static bool AppSettingRead(string paramName, bool defaultValue)
        {
            bool res;
            if (bool.TryParse(ConfigurationManager.AppSettings[paramName], out res))
            {
                return res;
            }
            return defaultValue;
        }

        /// <summary>
        /// Список значений
        /// </summary>
        /// <param name="paramName">имя параметра</param>
        /// <param name="defaultValue">список по умолчанию</param>
        /// <param name="separator">разделитель для создания списка</param>
        /// <returns></returns>
        public static string[] AppSettingRead(string paramName, string[] defaultValue, char separator = ',')
        {            
            string vals = ConfigurationManager.AppSettings[paramName]?.ToString() ?? null;
            if (!string.IsNullOrEmpty(vals))
            {
                return vals.Split(separator);
            }
            return defaultValue;
        }


        /// <summary>
        /// Настройки подключения
        /// </summary>
        /// <param name="paramName">имя параметра</param>
        /// <returns></returns>
        public static string ConnectionStringRead(string paramName)
        {            
            return ConfigurationManager.ConnectionStrings[paramName].ConnectionString;
        }
        
    }
}
