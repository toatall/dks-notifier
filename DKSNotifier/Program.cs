using System;
using System.Configuration;

namespace DKSNotifier
{
    internal class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {   
            // строка подключения к MS SQL Server
            string connectionString = ConfigurationManager.ConnectionStrings["Mssql"].ConnectionString;

            // параметры запуска 
            bool checkDismissal = bool.Parse(ConfigurationManager.AppSettings["CheckDismissial"]?.ToString() ?? "True");
            bool checkMoving = bool.Parse(ConfigurationManager.AppSettings["CheckMoving"]?.ToString() ?? "True");
            bool checkVacation = bool.Parse(ConfigurationManager.AppSettings["CheckVacation"]?.ToString() ?? "True");

            // настройки email
            string emailServerName = ConfigurationManager.AppSettings["EmailServerName"];
            int emailServerPort = int.Parse(ConfigurationManager.AppSettings["EmailServerPort"]);
            string emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            string emailTo = ConfigurationManager.AppSettings["EmailTo"];
            bool isEmailSend = bool.Parse(ConfigurationManager.AppSettings["EmailSend"]?.ToString() ?? "True");

            // каталог сохранения html-файлов 
            string dirOut = ConfigurationManager.AppSettings["DirOut"] ?? AppDomain.CurrentDomain.BaseDirectory + "HtmlOut";

            // путь к xml-файлу для сохранения информации о полученных сведениях (для исключения повторного направления/сохранения информации)            
            string xmlBaseFile = AppDomain.CurrentDomain.BaseDirectory + "base.xml";
            
            AppStarter appStarter = new AppStarter(connectionString, 
                emailServerName, emailServerPort, emailFrom, emailTo, 
                isEmailSend, dirOut, xmlBaseFile, checkDismissal, checkMoving, checkVacation);
            appStarter.Run();
        }       

    }
}
