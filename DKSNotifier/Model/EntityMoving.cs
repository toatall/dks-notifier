using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Model
{

    /// <summary>
    /// Запись об переводе сотрудника
    /// </summary>
    internal class EntityMoving: IEntity
    {
        #region Свойства

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string Fio { get; private set; }

        /// <summary>
        /// Табельный номер
        /// </summary>
        public string TabNumber { get; private set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; private set; }

        /// <summary>
        /// Наименование организации
        /// </summary>
        public string OrgName { get; private set; }

        /// <summary>
        /// Старое наименование отдела
        /// </summary>
        public string DepNameOld { get; private set; }

        /// <summary>
        /// Новое наименование отдела
        /// </summary>
        public string DepNameNew { get; private set; }

        /// <summary>
        /// Старая должность
        /// </summary>
        public string PostOld { get; private set; }

        /// <summary>
        /// Новая должность
        /// </summary>
        public string PostNew { get; private set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; private set; }        

        /// <summary>
        /// Номер приказа
        /// </summary>
        public string OrdNumber { get; private set; }

        /// <summary>
        /// Дата приказа
        /// </summary>
        public DateTime OrdDate { get; private set; }

        #endregion

        public EntityMoving(string id, string fio, string tabNumber, string login, string orgName, string depNameOld, string depNameNew,
            string postOld, string postNew, DateTime date, string ordNumber, DateTime ordDate) 
        {
            this.Id = id;
            this.Fio = fio;
            this.TabNumber = tabNumber;
            this.Login = login;
            this.OrgName = orgName;
            this.DepNameOld = depNameOld;
            this.DepNameNew = depNameNew;
            this.PostOld = postOld;
            this.PostNew = postNew;
            this.Date = date;
            this.OrdNumber = ordNumber;
            this.OrdDate = ordDate;
        }

        /// <inheritdoc/>
        /// <returns></returns>
        public string GetUnique()
        {
            return string.Format("{0}_{1}_{2}_{3}", Id, TabNumber, PostNew, Date);
        }

        /// <inheritdoc/>
        /// <returns></returns>
        public string TypeEntity()
        {
            return "MOVING";
        }
    }
}
