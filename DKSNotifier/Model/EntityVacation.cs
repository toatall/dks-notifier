using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Model
{
    /// <summary>
    /// Запись об отпуске сотрудника
    /// </summary>
    internal class EntityVacation: IEntity 
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
        /// Должность
        /// </summary>
        public string Post { get; private set; }

        /// <summary>
        /// Отдел
        /// </summary>
        public string Department { get; private set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public DateTime DateBegin { get; private set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime DateEnd { get; private set; }

        /// <summary>
        /// Количество дней
        /// </summary>
        public int Days { get; private set; }

        /// <summary>
        /// Вид отпуска
        /// </summary>
        public string TypeName { get; private set; }

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
        /// <param name="fio"></param>
        /// <param name="tabNumber"></param>
        /// <param name="login"></param>
        /// <param name="post"></param>
        /// <param name="department"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <param name="days"></param>
        /// <param name="typeName"></param>
        /// <param name="ordNumber"></param>
        /// <param name="ordDate"></param>
        public EntityVacation(string id, string fio, string tabNumber, string login, string post, string department, DateTime dateBegin,
            DateTime dateEnd, int days, string typeName, string ordNumber, DateTime ordDate)
        {
            this.Id = id;
            this.Fio = fio;
            this.TabNumber = tabNumber;
            this.Login = login;
            this.Post = post;
            this.Department = department;
            this.DateBegin = dateBegin;
            this.DateEnd = dateEnd;
            this.Days = days;
            this.TypeName = typeName;
            this.OrdNumber = ordNumber;
            this.OrdDate = ordDate;
        }

        /// <inheritdoc/>
        public string GetUnique()
        {
            return string.Format("{0}_{1}_{2}_{3}", Id, TabNumber, DateBegin, DateEnd);
        }

        /// <inheritdoc/>
        public string TypeEntity()
        {
            return "VACATION";
        }
    }
}
