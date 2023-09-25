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
    /// Работа с MS SQL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Mssql<T>
    {
        private string connectionString;
        private Log log;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="log"></param>
        public Mssql(string connectionString, Log log)
        {
            this.connectionString = connectionString;
            this.log = log;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryText"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public IEnumerable<T> Select(string queryText, Func<IDataRecord, T> func)
        {
            List<T> res = new List<T>();
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {                
                cnn.Open();
                SqlCommand command = new SqlCommand(queryText, cnn);
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
