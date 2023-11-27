using DKSNotifier.Formatter;
using DKSNotifier.Logs;
using DKSNotifier.Notifiers;
using DKSNotifier.Runners;
using DKSNotifier.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DKSNotifier
{
    /// <summary>
    /// Запуск всех 
    /// </summary>
    internal class AppStarter
    {
        #region Поля

        /// <summary>
        /// эксземпляр объекта лога
        /// </summary>
        private Log log;

        /// <summary>
        /// экземпляр хранилища
        /// </summary>
        private IStorage storage;

        /// <summary>
        /// экземпляр объекта форматирования
        /// </summary>
        private IFormatter formatter;

        /// <summary>
        /// строка подключения к MS SQL Server
        /// </summary>
        private string mssqlConnectionString;

        /// <summary>
        /// адрес email-сервера
        /// </summary>
        private string emailServerName;

        /// <summary>
        /// порт email-сервера
        /// </summary>
        private int emailServerPort;

        /// <summary>
        /// адрес отправителя
        /// </summary>
        private string emailFrom;

        /// <summary>
        /// адрес получателя
        /// </summary>
        private string emailTo;

        /// <summary>
        /// направлять уведомление по email
        /// </summary>
        private bool isEmailSend;

        /// <summary>
        /// каталог выгрузки файлов
        /// </summary>
        private string dirOut;

        /// <summary>
        /// Расположение файла хранилища
        /// </summary>
        private string xmlBaseFile;

        /// <summary>
        /// Проверка уволенных сотрудников
        /// </summary>
        private bool checkDismissal;

        /// <summary>
        /// Проверка переводов сотрудников
        /// </summary>
        private bool checkMoving;

        /// <summary>
        /// Проверка отпусков сотрудников
        /// </summary>
        private bool checkVacation;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mssqlConnectionString"></param>
        /// <param name="emailServerName"></param>
        /// <param name="emailServerPort"></param>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="isEmailSend"></param>
        /// <param name="dirOut"></param>
        /// <param name="xmlBaseFile"></param>
        /// <param name="checkDismissal"></param>
        /// <param name="checkMoving"></param>
        /// <param name="checkVacation"></param>
        public AppStarter(            
            string mssqlConnectionString,
            string emailServerName,
            int emailServerPort,
            string emailFrom,
            string emailTo,
            bool isEmailSend,
            string dirOut,
            string xmlBaseFile,
            bool checkDismissal,
            bool checkMoving,
            bool checkVacation
            )
        {            
            this.mssqlConnectionString = mssqlConnectionString;
            this.emailServerName = emailServerName;
            this.emailServerPort = emailServerPort;
            this.emailFrom = emailFrom;
            this.emailTo = emailTo;
            this.isEmailSend = isEmailSend;
            this.dirOut = dirOut;
            this.xmlBaseFile = xmlBaseFile;
            this.checkDismissal = checkDismissal;
            this.checkMoving = checkMoving;
            this.checkVacation = checkVacation;
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Init()
        {
            this.log = new Log();
            this.storage = new XmlStorage(this.xmlBaseFile, log);
            this.formatter = new HtmlFormatter();
        }

        /// <summary>
        /// Запуск
        /// </summary>
        public void Run()
        {
            this.Init();
            this.log.Info("Запуск приложения");

            try
            {
                StringBuilder textResult = new StringBuilder();
                Directory.CreateDirectory(this.dirOut);
                string htmlFilename = Path.Combine(this.dirOut, GenerateHtmlFilename());

                List<INotifier> notifiers = new List<INotifier>
            {
                new HtmlNotifier(htmlFilename, log)
            };
                if (this.isEmailSend)
                {
                    notifiers.Add(new EmailNotifier(this.emailServerName, this.emailServerPort, "Уведомление о движении сотрудников",
                        this.emailFrom, this.emailTo, log));
                }

                // уволенные
                if (this.checkDismissal)
                {
                    RunnerDismissal runnerDismissal = new RunnerDismissal(this.mssqlConnectionString, this.storage, this.log,
                        AppDomain.CurrentDomain.BaseDirectory + "SqlFiles/Dismissial.sql");
                    textResult.AppendLine(runnerDismissal.Start("УВОЛЬНЕНИЯ", this.formatter));
                }

                // переводы
                if (this.checkMoving)
                {
                    RunnerMoving runnerMoving = new RunnerMoving(this.mssqlConnectionString, this.storage, this.log,
                       AppDomain.CurrentDomain.BaseDirectory + "SqlFiles/Moving.sql");
                    textResult.AppendLine(runnerMoving.Start("ПЕРЕВОДЫ", this.formatter));
                }

                // отпуск
                if (this.checkVacation)
                {
                    RunnerVacation runnerVacation = new RunnerVacation(this.mssqlConnectionString, this.storage, this.log,
                       AppDomain.CurrentDomain.BaseDirectory + "SqlFiles/Vacation.sql");
                    textResult.AppendLine(runnerVacation.Start("ОТПУСКА", this.formatter));
                }

                // запуск уведомлений
                foreach (INotifier notifier in notifiers)
                {
                    notifier.Exec(textResult.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка в AppStarter. " + ex.Message);
            }

            this.log.Info("Завершение приложения");
            this.log.EmptyLines(3);

        }

        /// <summary>
        /// Генерирование имени html-файла по текущей дате
        /// </summary>
        /// <returns></returns>
        private string GenerateHtmlFilename()
        {
            return DateTime.Now.ToString("yyyy_MM_dd") + ".html";
        }
        

    }
}
