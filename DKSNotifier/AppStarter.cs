using DKSNotifier.Formatter;
using DKSNotifier.Logs;
using DKSNotifier.Notifiers;
using DKSNotifier.Runners;
using DKSNotifier.Sql;
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

        private readonly ConfigurationStorage configurationStorage;

        /// <summary>
        /// Расположение файла хранилища
        /// </summary>
        private readonly string xmlBaseFile;

        /*
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
        */
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationStorage"></param>
        /// <param name="xmlBaseFile"></param>
        public AppStarter(            
            ConfigurationStorage configurationStorage,
            string xmlBaseFile
            )
        {
            this.configurationStorage = configurationStorage;
            this.xmlBaseFile = xmlBaseFile;
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
            var cs = this.configurationStorage;

            try
            {
                StringBuilder textResult = new StringBuilder();
                Directory.CreateDirectory(cs.DirOut);
                string htmlFilename = Path.Combine(cs.DirOut, GenerateHtmlFilename());

                List<INotifier> notifiers = new List<INotifier>
                {
                    new HtmlNotifier(htmlFilename, log)
                };
                
                if (cs.EmailSend)
                {
                    notifiers.Add(new EmailNotifier(cs.EmailServerName, cs.EmailServerPort, "Уведомление о движении сотрудников",
                        cs.EmailFrom, cs.EmailTo, log));
                }

                string resultRunner = "";
               
                // уволенные
                if (cs.DismissalCheck)
                {                    
                    DismissalQuery dismissalQuery = new DismissalQuery(cs.CodeNO, cs.DismissalCountDays, cs.DismissalOrdType);
                    RunnerDismissal runnerDismissal = new RunnerDismissal(cs.SqlConnectionString, this.storage, this.log, dismissalQuery);
                    resultRunner = runnerDismissal.Start("УВОЛЬНЕНИЯ", this.formatter);
                    if (!string.IsNullOrEmpty(resultRunner))
                    {
                        textResult.AppendLine(resultRunner);
                    }
                }
                
                // переводы
                if (cs.MovingCheck)
                {
                    MovingQuery movingQuery = new MovingQuery(cs.CodeNO, cs.MovingCountDays, cs.MovingOrdType);
                    RunnerMoving runnerMoving = new RunnerMoving(cs.SqlConnectionString, this.storage, this.log, movingQuery);
                    resultRunner = runnerMoving.Start("ПЕРЕВОДЫ", this.formatter);
                    if (!string.IsNullOrEmpty(resultRunner))
                    {
                        textResult.AppendLine(resultRunner);
                    }
                }

                
                // отпуск
                if (cs.VacationCheck)
                {
                    VacationQuery vacationQuery = new VacationQuery(cs.CodeNO, cs.VacationCountDays, cs.VacationOrdType, cs.VacationTypeCode);
                    RunnerVacation runnerVacation = new RunnerVacation(cs.SqlConnectionString, this.storage, this.log, vacationQuery);
                    resultRunner = runnerVacation.Start("ОТПУСКА", this.formatter);
                    if (!string.IsNullOrEmpty(resultRunner))
                    {
                        textResult.AppendLine(resultRunner);
                    }                    
                }

                if (!string.IsNullOrEmpty(resultRunner))
                {
                    // запуск уведомлений
                    foreach (INotifier notifier in notifiers)
                    {
                        notifier.Exec(textResult.ToString());
                    }                    
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
