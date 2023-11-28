using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier
{
    /// <summary>
    /// Настройки
    /// </summary>
    internal class ConfigurationStorage
    {
        #region Свойства

        /// <summary>
        /// Код НО
        /// </summary>
        public string CodeNO { get; private set; }
        
        /// <summary>
        /// Включение/отключение поиска информации по уволенным
        /// </summary>
        public bool DismissalCheck { get; private set; }

        /// <summary>
        /// Кол-во дней поиска по уволенным
        /// </summary>
        public int DismissalCountDays { get; private set; }

        /// <summary>
        /// Виды приказов по уволенным
        /// </summary>
        public string[] DismissalOrdType { get; private set; }

        /// <summary>
        /// Включение/отключение поиска информации по перемещению 
        /// </summary>
        public bool MovingCheck { get; private set; }

        /// <summary>
        /// Кол-во дней поиска по перемещению
        /// </summary>
        public int MovingCountDays { get; private set; }

        /// <summary>
        /// Виды приказов по перемещению
        /// </summary>
        public string[] MovingOrdType { get; private set; }

        /// <summary>
        /// Включение/отключение поиска информации по отпускам 
        /// </summary>
        public bool VacationCheck { get; private set; }

        /// <summary>
        /// Кол-во дней поиска по отпускам
        /// </summary>
        public int VacationCountDays { get; private set; }

        /// <summary>
        /// Виды приказов по отпускам
        /// </summary>
        public string[] VacationOrdType { get; private set; }

        /// <summary>
        /// Коды отпусков
        /// </summary>
        public string[] VacationTypeCode { get; private set; }

        /// <summary>
        /// Имя сервера SMTP
        /// </summary>
        public string EmailServerName { get; private set; }

        /// <summary>
        /// Порт сервера SMTP
        /// </summary>
        public int EmailServerPort { get; private set; }

        /// <summary>
        /// Адрес отправителя
        /// </summary>
        public string EmailFrom { get; private set; }

        /// <summary>
        /// Адрес получателя
        /// </summary>
        public string EmailTo { get; private set; }

        /// <summary>
        /// Включение/отключение направления почты
        /// </summary>
        public bool EmailSend { get; private set; }

        /// <summary>
        /// Включение/отключение сохранения файла html
        /// </summary>
        public bool UseOutFile { get; private set; }

        /// <summary>
        /// Каталог сохранения html-файла
        /// </summary>
        public string DirOut { get; private set; }

        /// <summary>
        /// Строка подключения
        /// </summary>
        public string SqlConnectionString { get; private set; }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="codeNO"></param>
        /// <param name="dismissalCheck"></param>
        /// <param name="dismissalCountDays"></param>
        /// <param name="dismissalOrdType"></param>
        /// <param name="movingCheck"></param>
        /// <param name="movingCountDays"></param>
        /// <param name="movingOrdType"></param>
        /// <param name="vacationCheck"></param>
        /// <param name="vacationCountDays"></param>
        /// <param name="vacationOrdType"></param>
        /// <param name="vacationTypeCode"></param>
        /// <param name="emailServerName"></param>
        /// <param name="emailServerPort"></param>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="emailSend"></param>
        /// <param name="useOutFile"></param>
        /// <param name="dirOut"></param>
        /// <param name="sqlConnectionString"></param>
        public ConfigurationStorage(
            
            string codeNO,

            bool dismissalCheck,
            int dismissalCountDays,
            string[] dismissalOrdType,

            bool movingCheck,
            int movingCountDays,
            string[] movingOrdType,

            bool vacationCheck,
            int vacationCountDays,
            string[] vacationOrdType,
            string[] vacationTypeCode,

            string emailServerName,
            int emailServerPort,
            string emailFrom,
            string emailTo,
            bool emailSend,

            bool useOutFile,
            string dirOut,

            string sqlConnectionString
        )
        {
            this.CodeNO = codeNO;

            this.DismissalCheck = dismissalCheck;
            this.DismissalCountDays = dismissalCountDays;
            this.DismissalOrdType = dismissalOrdType;

            this.MovingCheck = movingCheck;
            this.MovingCountDays = movingCountDays;
            this.MovingOrdType = movingOrdType;

            this.VacationCheck = vacationCheck;
            this.VacationCountDays = vacationCountDays;
            this.VacationOrdType = vacationOrdType;
            this.VacationTypeCode = vacationTypeCode;

            this.EmailServerName = emailServerName;
            this.EmailServerPort = emailServerPort;
            this.EmailFrom = emailFrom;
            this.EmailTo = emailTo;
            this.EmailSend = emailSend;

            this.UseOutFile = useOutFile;
            this.DirOut = dirOut;

            this.SqlConnectionString = sqlConnectionString;
        }


    }
}
