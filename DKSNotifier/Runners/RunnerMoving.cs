using DKSNotifier.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKSNotifier.Logs;
using DKSNotifier.XML;


namespace DKSNotifier.Runners
{
    /// <summary>
    /// Перемещение сотрудников
    /// </summary>
    internal class RunnerMoving : Runner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringMssql"></param>
        /// <param name="xmlStorage"></param>
        /// <param name="log"></param>
        /// <param name="sqlQueryFile"></param>
        public RunnerMoving(string connectionStringMssql, XmlStorage xmlStorage, Log log, string sqlQueryFile)
            : base(connectionStringMssql, xmlStorage, log, sqlQueryFile) { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override IEntity FillEntity(IDataRecord record)
        {
            return new EntityMoving()
            {
                Id = record["LINK"].ToString().Trim(),
                OrgName = record["DEP_NAME"].ToString().Trim(),
                Login = record["LOGIN"].ToString().Trim(),
                Fio = record["FIO"].ToString().Trim(),
                TabNum = record["TAB_NUM"].ToString().Trim(),
                DepNameOld = record["SUBDIV"].ToString(),
                PostOld = record["POST"].ToString(),
                DepNameNew = record["NEW_SUBDIV"].ToString(),
                PostNew = record["NEW_POST"].ToString(),
                Date = DateTime.Parse(record["DATE_BEGIN"].ToString()),
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
                            <h3>Переводы</h3>
                            <table style='margin-bottom: 5rem;'>                                
	                            <tr>
		                            <th>Табельный номер</th>
                                    <th>Учетная запись</th>
                                    <th>ФИО</th>
                                    <th>Дата</th>
                                    <th>Из отдела, должность</th>
                                    <th>В отдел, должность</th>
                                </tr>";
                foreach (EntityMoving entity in entities)
                {
                    result += string.Format(@"
                        <tr>
		                    <td>{0}</td>
		                    <td>{1}</td>
                            <td>{2}</td>
                            <td>{3:dd.MM.yyyy}</td>
                            <td>{4}, {5}</td>
                            <td>{6}, {7}</td>
                        </tr>",
                        entity.TabNum,
                        entity.Login,
                        entity.Fio,
                        entity.Date,
                        entity.DepNameOld,
                        entity.PostOld,
                        entity.DepNameNew,
                        entity.PostNew);
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
            foreach (EntityMoving entity in entities)
            {
                xmlStorage.Add(entity.TypeEntity(), entity.GetUnique(), entity.TabNum.Trim(), entity.Login, entity.Fio.Trim(),
                    string.Format("дата: {0}, предыдущий отдел: {1}, новый отдел: {2}, предыдущая должность: {3}, новая должность: {4}",
                        entity.Date.ToShortDateString(), entity.DepNameOld.Trim(), entity.DepNameNew.Trim(), entity.PostOld.Trim(), entity.PostNew.Trim())
                );
            }
        }

    }
}
