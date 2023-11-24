using System;
using System.Collections.Generic;
using System.Linq;
using DKSNotifier.Logs;
using DKSNotifier.Model;
using DKSNotifier.Storage;
using System.Data;

namespace DKSNotifier.Runners
{
    /// <summary>
    /// Увольнение сотрудников
    /// </summary>
    internal class RunnerDismissal: Runner
    {

        /// <summary>
        /// Создание объектов
        /// </summary>
        /// <param name="connectionStringMssql">строка подключения</param>
        /// <param name="storage">объект хранилища</param>
        /// <param name="log">объект лога</param>
        /// <param name="sqlQueryFile">sql-файл</param>
        public RunnerDismissal(string connectionStringMssql, IStorage storage, Log log, string sqlQueryFile) 
            : base(connectionStringMssql, storage, log, sqlQueryFile) { }

        /// <inheritdoc/>        
        protected override IEntity FillEntity(IDataRecord record)
        {
            return new EntityDismissal(
                id: record["LINK"].ToString().Trim(),
                tabNumber: record["TAB_NUM"].ToString().Trim(),
                login: record["LOGIN"].ToString().Trim(),
                fio: record["FIO"].ToString().Trim(),
                post: record["POST_NAME"].ToString().Trim(),
                orgName: record["FULL_NAME"].ToString().Trim(),
                depIndex: record["SUBDIV_CODE"].ToString().Trim(), 
                depName: record["SUBDIV_NAME"].ToString().Trim(), 
                dismissalDate: DateTime.Parse(record["DIS_DATE"].ToString()), 
                dismissalDescription: record["DESCRIPTION"].ToString().Trim(), 
                ordNumber: record["ORD_NUMBER"].ToString().Trim(), 
                ordDate:DateTime.Parse(record["ORD_DATE"].ToString())
            );
        }

        /// <inheritdoc/>
        protected override string[][] GetData(IEnumerable<IEntity> entities)
        {
            return entities.Cast<EntityDismissal>().Select(t => new string[] {
                t.TabNumber.Trim(),
                t.Login.Trim(),
                t.Fio.Trim(),
                t.DismissalDate.ToString("dd.MM.yyyy"),
                string.Format("{0}, {1} - {2}", t.Post.Trim(), t.DepIndex.Trim(), t.DepName.Trim()),
                string.Format("№{0} от {1:dd.MM.yyyy}", t.OrdNumber, t.OrdDate),
                t.OrgName.Trim(),
                t.DismissalDescription.Trim(),
            }).ToArray();
        }

        /// <inheritdoc/>
        protected override string[] GetLabels()
        {
            return new string[] {
                "Табельный номер", // TabNumber
                "Учетная запись", // Login
                "ФИО", // Fio
                "Дата увольнения", // DismissalDate
                "Должность, отдел", // Post + DepIndex + DepName
                "Номер и дата приказа", // OrdNumber + OrdDate
                "Налоговый орган", // OrgName
                "Причина", // DismissalDescription
            };
        }               

        /// <inheritdoc/>
        protected override void SaveToStorage(IEnumerable<IEntity> entities)
        {
            foreach(EntityDismissal entity in entities)
            {
                //storage.Add(entity.TypeEntity(), entity.GetUnique(), entity.TabNumber.Trim(), entity.Login, entity.Fio.Trim(), entity.DismissalDescription.Trim());
                string description = string.Format("табельный номер: {0}, логин: {1}, ФИО: {2}, дата и время: {3}",
                    entity.TabNumber.Trim(), entity.Login.Trim(), entity.Fio.Trim(), DateTime.Now.ToString());
                storage.Add(entity.TypeEntity(), entity.GetUnique(), description);
            }
        }

    }
}
