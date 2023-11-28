using DKSNotifier.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DKSNotifier.Logs;
using DKSNotifier.Storage;
using DKSNotifier.Sql;

namespace DKSNotifier.Runners
{
    /// <summary>
    /// Декретные отпуска
    /// </summary>
    internal class RunnerVacation : Runner
    {
        /// <summary>
        /// Создание объектов
        /// </summary>
        /// <param name="connectionStringMssql">строка подключения</param>
        /// <param name="storage">объект хранилища</param>
        /// <param name="log">объект лога</param>
        /// <param name="sqlQueryFile">sql-файл</param>
        public RunnerVacation(string connectionStringMssql, IStorage storage, Log log, IQuery query)
            : base(connectionStringMssql, storage, log, query) { }

        /// <inheritdoc/> 
        protected override IEntity FillEntity(IDataRecord record)
        {
            return new EntityVacation(
                id: record["LINK"].ToString().Trim(),
                fio: record["FIO"].ToString().Trim(),
                tabNumber: record["TAB_NUM"].ToString().Trim(),
                login: record["LOGIN"].ToString().Trim(),
                post: record["POST"].ToString().Trim(),
                department: record["SUBDIV"].ToString().Trim(),
                dateBegin: DateTime.Parse(record["DATE_BEGIN"].ToString()),
                dateEnd: DateTime.Parse(record["DATE_END_REAL"].ToString()),
                days: int.Parse(record["COUNT_DAYS"].ToString().Trim()),
                typeName: record["TYPE_NAME"].ToString().Trim(),
                ordNumber: record["ORD_NUMBER"].ToString().Trim(),
                ordDate: DateTime.Parse(record["ORD_DATE"].ToString())
            );           
        }

        /// <inheritdoc/> 
        protected override string[][] GetData(IEnumerable<IEntity> entities)
        {
            return entities.Cast<EntityVacation>().Select(t => new string[] {
                t.TabNumber.Trim(),
                t.Login.Trim(),
                t.Fio.Trim(),
                t.Department.Trim(),
                t.Post.Trim(),
                string.Format("№{0} от {1:dd.MM.yyyy}", t.OrdNumber, t.OrdDate),
                string.Format("{0:dd.MM.yyyy} - {1:dd.MM.yyyy}", t.DateBegin, t.DateEnd),                
                t.Days.ToString(),
                t.TypeName.Trim(),
            }).ToArray();
        }

        /// <inheritdoc/> 
        protected override string[] GetLabels()
        {
            return new string[] {
                "Табельный номер", // TabNum
                "Учетная запись", // Login
                "ФИО", // Fio
                "Отдел", // Department
                "Должность", // Post
                "Номер и дата приказа", // OrdNumber + OrdDate
                "Период", // DateBegin - DateEnd
                "Количество дней", // Days
                "Вид отпуска", // TypeName
            };
        }

        /// <inheritdoc/>
        protected override void SaveToStorage(IEnumerable<IEntity> entities)
        {
            foreach (EntityVacation entity in entities)
            {                
                string description = string.Format("табельный номер: {0}, логин: {1}, ФИО: {2}",
                    entity.TabNumber.Trim(), entity.Login.Trim(), entity.Fio.Trim());
                storage.Add(entity.TypeEntity(), entity.GetUnique(), description);
            }
        }

    }
}
