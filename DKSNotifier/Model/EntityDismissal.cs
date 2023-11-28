using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Model
{

    /// <summary>
    /// Запись об увольнении сотрудника
    /// </summary>
    internal class EntityDismissal : IEntity
    {
        #region Свойства

        /// <summary>
        /// Идентифиактор
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Табельный номер
        /// </summary>
        public string TabNumber { get; private set; }
        
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; private set; }
        
        /// <summary>
        /// ФИО
        /// </summary>
        public string Fio { get; private set; }
        
        /// <summary>
        /// Должность
        /// </summary>
        public string Post { get; private set; } 
        
        /// <summary>
        /// Наименование организации
        /// </summary>
        public string OrgName { get; private set; }
        
        /// <summary>
        /// Индекс отдела
        /// </summary>
        public string DepIndex { get; private set; }
        
        /// <summary>
        /// Наименование отдела
        /// </summary>
        public string DepName { get; private set; }
        
        /// <summary>
        /// Дата увольнения
        /// </summary>
        public DateTime DismissalDate { get; private set; }
        
        /// <summary>
        /// Причина увольнения
        /// </summary>
        public string DismissalDescription { get; private set; }
        
        /// <summary>
        /// Номер приказа
        /// </summary>
        public string OrdNumber { get; private set; }
                
        /// <summary>
        /// Дата приказа
        /// </summary>
        public DateTime OrdDate { get; private set; }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tabNumber"></param>
        /// <param name="login"></param>
        /// <param name="fio"></param>
        /// <param name="post"></param>
        /// <param name="orgName"></param>
        /// <param name="depIndex"></param>
        /// <param name="depName"></param>
        /// <param name="dismissalDate"></param>
        /// <param name="dismissalDescription"></param>
        /// <param name="ordNumber"></param>
        /// <param name="ordDate"></param>
        public EntityDismissal(string id, string tabNumber, string login, string fio, string post, string orgName,
            string depIndex, string depName, DateTime dismissalDate, string dismissalDescription, string ordNumber, DateTime ordDate)
        {
            this.Id = id;
            this.TabNumber = tabNumber;
            this.Login = login;
            this.Fio = fio;
            this.Post = post;
            this.OrgName = orgName;
            this.DepIndex = depIndex;
            this.DepName = depName;
            this.DismissalDate = dismissalDate;
            this.DismissalDescription = dismissalDescription;
            this.OrdNumber = ordNumber;
            this.OrdDate = ordDate;
        }

        /// <inheritdoc/>
        public string GetUnique()
        {
            return string.Format("{0}_{1}_{2}", Id, TabNumber.Trim(), DismissalDate);
        }

        /// <inheritdoc/>
        public string TypeEntity()
        {
            return "DISMISSAL";
        }

    }
}
