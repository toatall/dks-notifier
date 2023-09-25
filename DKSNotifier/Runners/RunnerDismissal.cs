using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKSNotifier.Logs;
using DKSNotifier.Model;
using DKSNotifier.XML;
using System.Data;

namespace DKSNotifier.Runners
{
    /// <summary>
    /// Увольнение сотрудников
    /// </summary>
    internal class RunnerDismissal: Runner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringMssql"></param>
        /// <param name="xmlStorage"></param>
        /// <param name="log"></param>
        /// <param name="sqlQueryFile"></param>
        public RunnerDismissal(string connectionStringMssql, XmlStorage xmlStorage, Log log, string sqlQueryFile) 
            : base(connectionStringMssql, xmlStorage, log, sqlQueryFile) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override IEntity FillEntity(IDataRecord record)
        {
            return new EntityDismissal()
            {
                /*
                Id = record["LINK"].ToString().Trim(),
                OrgName = record["FULL_NAME"].ToString().Trim(),
                Fio = string.Format("{0} {1} {2}",
                    record["FM"].ToString().Trim(), record["IM"].ToString().Trim(), record["OT"].ToString().Trim()),
                DepIndex = record["SUBDIV_CODE"].ToString().Trim(),
                DepName = record["SUBDIV_NAME"].ToString().Trim(),
                Post = record["POST_NAME"].ToString().Trim(),
                TabNumber = record["TAB_NUM"].ToString().Trim(),
                DismissalDate = DateTime.Parse(record["DIS_DATE"].ToString()),
                DismissalDescription = record["DESCRIPTION"].ToString().Trim(),
                */
                Id = record["LINK"].ToString().Trim(),
                OrgName = record["FULL_NAME"].ToString().Trim(),
                Fio = record["FIO"].ToString().Trim(),
                Login = record["LOGIN"].ToString().Trim(),
                DepIndex = record["SUBDIV_CODE"].ToString().Trim(),
                DepName = record["SUBDIV_NAME"].ToString().Trim(),
                Post = record["POST_NAME"].ToString().Trim(),
                TabNumber = record["TAB_NUM"].ToString().Trim(),
                DismissalDate = DateTime.Parse(record["DIS_DATE"].ToString()),
                DismissalDescription = record["DESCRIPTION"].ToString().Trim(),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected override string FormatEmailMessage(IEnumerable<IEntity> entities)
        {           
            string result = "";
            if (entities.Count() > 0)
            {
                result += @"
                            <h3>Увольнение</h3>
                            <table style='margin-bottom: 5rem;'>                                       
	                            <tr>
		                            <th>Табельный номер</th>
                                    <th>ФИО</th>
                                    <th>Дата увольнения</th>
                                    <th>Должность, отдел</th>
                                    <th>Налоговый орган</th>
                                    <th>Причина</th>
                                </tr>";
                foreach (EntityDismissal entity in entities)
                {                   
                    result += string.Format(@"
                        <tr>
		                    <td>{0}</td>
		                    <td>{1}</td>
                            <td>{2:dd.MM.yyyy}</td>
                            <td>{3}, {4} - {5}</td>
                            <td>{6}</td>
                            <td>{7}</td>
                        </tr>", 
                        entity.TabNumber, 
                        entity.Fio, 
                        entity.DismissalDate,
                        entity.Post.Trim(),
                        entity.DepIndex.Trim(),
                        entity.DepName.Trim(),
                        entity.OrgName.Trim(),
                        entity.DismissalDescription.Trim());
                }
                result += "</table>";
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        protected override void SaveToXml(IEnumerable<IEntity> entities)
        {
            foreach(EntityDismissal entity in entities)
            {
                xmlStorage.Add(entity.TypeEntity(), entity.GetUnique(), entity.TabNumber.Trim(), entity.Fio.Trim(), entity.DismissalDescription.Trim());
            }
        }

    }
}
