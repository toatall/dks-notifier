using DKSNotifier.Model;
using System;
using System.Collections.Generic;
using System.Data;
using DKSNotifier.Logs;
using DKSNotifier.Storage;
using System.Linq;
using DKSNotifier.Sql;

namespace DKSNotifier.Runners
{
    /// <summary>
    /// Перемещение сотрудников
    /// </summary>
    internal class RunnerMoving : Runner
    {
        /// <summary>
        /// Создание объектов
        /// </summary>
        /// <param name="connectionStringMssql">строка подключения</param>
        /// <param name="storage">объект хранилища</param>
        /// <param name="log">объект лога</param>
        /// <param name="sqlQueryFile">sql-файл</param>
        public RunnerMoving(string connectionStringMssql, IStorage storage, Log log, IQuery query)
            : base(connectionStringMssql, storage, log, query) { }

        /// <inheritdoc/>
        protected override IEntity FillEntity(IDataRecord record)
        {
            return new EntityMoving(
                id: record["LINK"].ToString().Trim(),
                fio: record["FIO"].ToString().Trim(),
                tabNumber: record["TAB_NUM"].ToString().Trim(),
                login: record["LOGIN"].ToString().Trim(),
                orgName: record["DEP_NAME"].ToString().Trim(),
                depNameOld: record["SUBDIV"].ToString(),
                depNameNew: record["NEW_SUBDIV"].ToString(),
                postOld: record["POST"].ToString(),
                postNew: record["NEW_POST"].ToString(),
                date: DateTime.Parse(record["DATE_BEGIN"].ToString()),
                ordNumber: record["ORD_NUMBER"].ToString().Trim(),
                ordDate: DateTime.Parse(record["ORD_DATE"].ToString())
            );            
        }

        /// <inheritdoc/>
        protected override string[][] GetData(IEnumerable<IEntity> entities)
        {
            return entities.Cast<EntityMoving>().Select(t => new string[] {
                t.TabNumber.Trim(),
                t.Login.Trim(),
                t.Fio.Trim(),                
                string.Format("№{0} от {1:dd.MM.yyyy}", t.OrdNumber, t.OrdDate),
                t.Date.ToShortDateString(),
                string.Format("{0}, {1}", t.DepNameOld, t.PostOld),
                string.Format("{0}, {1}", t.DepNameNew, t.PostNew),
            }).ToArray();
        }

        /// <inheritdoc/>
        protected override string[] GetLabels()
        {
            return new string[] {
                "Табельный номер", // TabNum
                "Учетная запись", // Login
                "ФИО", // Fio
                "Номер и дата приказа", // OrdNumber + OrdDate          
                "Дата", // Date
                "Из отдела, должность", // DepNameOld, PostOld
                "В отдел, должность", // DepNameNew, PostNew                     
            };
        }

        /// <inheritdoc/>
        protected override void SaveToStorage(IEnumerable<IEntity> entities)
        {
            foreach (EntityMoving entity in entities)
            {
                string description = string.Format("табельный номер: {0}, логин: {1}, ФИО: {2}",
                    entity.TabNumber.Trim(), entity.Login.Trim(), entity.Fio.Trim());
                storage.Add(entity.TypeEntity(), entity.GetUnique(), description);
            }
        }

    }
}
