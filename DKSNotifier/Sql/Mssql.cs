using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DKSNotifier.Logs;
using System.Data;

namespace DKSNotifier.Sql
{
    /// <summary> 
    /// Работа с базой MS SQL Server
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Mssql<T>
    {
        /// <summary>
        /// строка подключения 
        /// </summary>
        private string connectionString;

        /// <summary>
        /// лог
        /// </summary>
        private Log log;

        /// <summary>
        /// 
        /// </summary>
        private IQuery query;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionString">строка подключения</param>
        /// <param name="log">лог</param>
        public Mssql(string connectionString, Log log, IQuery query)
        {
            this.connectionString = connectionString;
            this.log = log;
            this.query = query;
        }

        /// <summary>
        /// Чтение данных из базы данных и заполнение коллекции объектов T
        /// </summary>
        /// <param name="queryText">текст запроса</param>
        /// <param name="func">функция заполнения объекта T</param>
        /// <returns></returns>
        public IEnumerable<T> Select(Func<IDataRecord, T> func)
        {
            List<T> res = new List<T>();
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {                
                cnn.Open();
                SqlCommand command = new SqlCommand(); 
                command.Connection = cnn;
                query.PrepareSqlCommand(command);
                command.CommandTimeout = 60 * 5;
                log.Info("Выполнение sql-запроса...");
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    res.Add(func(reader));
                }
                cnn.Close();
            }            
            return res;
        }
       

    }
}
