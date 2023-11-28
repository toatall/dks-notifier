using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier
{
    internal class ConfigurationStorage
    {
        public string CodeNO { get; private set; }
        
        public bool DismissalCheck { get; private set; }
        public int DismissalCountDays { get; private set; }
        public string[] DismissalOrdType { get; private set; }

        public bool MovingCheck { get; private set; }
        public int MovingCountDays { get; private set; }
        public string[] MovingOrdType { get; private set; }

        public bool VacationCheck { get; private set; }
        public int VacationCountDays { get; private set; }
        public string[] VacationOrdType { get; private set; }
        public string[] VacationTypeCode { get; private set; }

        public string EmailServerName { get; private set; }
        public int EmailServerPort { get; private set; }
        public string EmailFrom { get; private set; }
        public string EmailTo { get; private set; }
        public bool EmailSend { get; private set; }

        public bool UseOutFile { get; private set; }
        public string DirOut { get; private set; }


        public string SqlConnectionString { get; private set; }

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
