using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DKSNotifier.XML;
using DKSNotifier.Logs;
using DKSNotifier.Email;
using DKSNotifier.Runners;

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
            string connectionString = ConfigurationManager.ConnectionStrings["Mssql"].ConnectionString;
            Log log = new Log();
            Sender sender = new Sender(
                ConfigurationManager.AppSettings["EmailServerName"],
                int.Parse(ConfigurationManager.AppSettings["EmailServerPort"]),
                "Уведомление о движении сотрудников",
                ConfigurationManager.AppSettings["EmailFrom"],
                ConfigurationManager.AppSettings["EmailTo"],
                log
            );
            XmlStorage xmlStorage = new XmlStorage(AppDomain.CurrentDomain.BaseDirectory + "base.xml", log);

            log.Info("Запуск приложения");

            string emailMessage = "";

            emailMessage += Run(
                new RunnerDismissal(connectionString, xmlStorage, log, AppDomain.CurrentDomain.BaseDirectory + "SqlFiles/Dismissial.sql"), 
                "УВОЛЬНЕНИЕ:");
            emailMessage += Run(
                new RunnerMoving(connectionString, xmlStorage, log, AppDomain.CurrentDomain.BaseDirectory + "SqlFiles/Moving.sql"), 
                "ПЕРЕВОДЫ:");
            emailMessage += Run(
                new RunnerVacation(connectionString, xmlStorage, log, AppDomain.CurrentDomain.BaseDirectory + "SqlFiles/Vacation.sql"), 
                "ОТПУСК ПО БЕРЕМЕННОСТИ И РОДАМ:");

            if (!string.IsNullOrEmpty(emailMessage))
            {
                string head = @"
                        <style>
                            body { font-family: system-ui, -apple-system, ""Segoe UI"", Roboto, ""Helvetica Neue"", ""Noto Sans"", ""Liberation Sans"", Arial } 
                            table { caption-side: bottom; border-collapse: collapse; margin-bottom: 50px; width: 100%; }
                            table td, table th { border: 1px solid #aaa; padding: 5px; }
                        </style>";
                sender.Send(string.Format("<html><head>{0}</head><body>{1}</body></html>", head, emailMessage));
            }
            log.Info("Завершение приложения");
            log.EmptyLines(3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private static string Run(Runner runner, string title)
        {            
            string textRunner = runner.Start(title);
            if (!string.IsNullOrEmpty(textRunner))
            {
                return textRunner;
            }
            return null;
        }

    }
}
