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
    /// Декретные отпуска
    /// </summary>
    internal class RunnerVacation : Runner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringMssql"></param>
        /// <param name="xmlStorage"></param>
        /// <param name="log"></param>
        /// <param name="sqlQueryFile"></param>
        public RunnerVacation(string connectionStringMssql, XmlStorage xmlStorage, Log log, string sqlQueryFile)
            : base(connectionStringMssql, xmlStorage, log, sqlQueryFile) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override IEntity FillEntity(IDataRecord record)
        {
            return new EntityVacation()
            {
                Id = record["LINK"].ToString().Trim(),
                Fio = record["FIO"].ToString().Trim(),
                Post = record["POST"].ToString().Trim(),
                Login = record["LOGIN"].ToString().Trim(),
                Department = record["SUBDIV"].ToString().Trim(),
                TabNum = record["TAB_NUM"].ToString().Trim(),
                TypeName = record["TYPE_NAME"].ToString().Trim(),
                Days = int.Parse(record["COUNT_DAYS"].ToString().Trim()),
                DateBegin = DateTime.Parse(record["DATE_BEGIN"].ToString()),
                DateEnd = DateTime.Parse(record["DATE_END_REAL"].ToString())
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
                            <h3>Отпуск по беременности и родам</h3>
                            <table style='margin-bottom: 5rem;'>                                
	                            <tr>
		                            <th>Табельный номер</th>
                                    <th>Учетная запись</th>
                                    <th>ФИО</th>
                                    <th>Отдел</th>
                                    <th>Должность</th>
                                    <th>Период</th>
                                    <th>Количество дней</th>
                                    <th>Вид отпуска</th>
                                </tr>";
                foreach (EntityVacation entity in entities)
                {
                    result += string.Format(@"
                        <tr>
		                    <td>{0}</td>
		                    <td>{1}</td>
                            <td>{2}</td>
                            <td>{3}</td>
                            <td>{4}</td>
                            <td>{5:dd.MM.yyyy} - {6:dd.MM.yyyy}</td>
                            <td>{7}</td>
                            <td>{8}</td>
                        </tr>",
                        entity.TabNum,
                        entity.Login,
                        entity.Fio,
                        entity.Department,
                        entity.Post,
                        entity.DateBegin,
                        entity.DateEnd,
                        entity.Days,
                        entity.TypeName);
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
            foreach (EntityVacation entity in entities)
            {
                xmlStorage.Add(entity.TypeEntity(), entity.GetUnique(), entity.TabNum.Trim(), entity.Login, entity.Fio.Trim(),
                    string.Format("дата начала: {0}, дата окончания: {1}, количество дней: {2}, вид отпуска: {3}",
                        entity.DateBegin.ToShortDateString(), entity.DateEnd.ToShortDateString(), entity.Days, entity.TypeName.Trim())
                );
            }
        }

    }
}
