using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Sql
{
    /// <summary>
    /// Создание списка параметров для оператора IN в SQL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class InCauseParamSql<T>
    {
        /// <summary>
        /// Имя параметра
        /// </summary>
        private string pName = "param";

        /// <summary>
        /// Префикс имени параметра
        /// </summary>
        private string prefix;

        /// <summary>
        /// Тип данных
        /// </summary>
        private SqlDbType dbType;

        /// <summary>
        /// Массив значений
        /// </summary>
        private T[] data;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sqlDbType">тип данных sql</param>
        /// <param name="data">массив значений</param>
        /// <param name="prefix">префикс имени параметра</param>
        public InCauseParamSql(SqlDbType sqlDbType, T[] data, string prefix)
        {
            this.dbType = sqlDbType;
            this.data = data;
            this.prefix = prefix;
        }

        /// <summary>
        /// Создание параметров для sql
        /// </summary>
        /// <returns></returns>
        protected List<SqlParameter> GenerateParams()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            for (int i = 0; i < data.Length; i++)
            {                
                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = GetParamName(i),
                    SqlDbType = this.dbType,
                    Value = data[i],
                });
            }
            return sqlParameters;
        }

        /// <summary>
        /// Генерирует имя параметра
        /// </summary>
        /// <param name="index">индекс параметра</param>
        /// <returns></returns>
        private string GetParamName(int index)
        {
            return string.Format("@{0}_{1}_{2}", this.prefix, this.pName, index);
        }

        /// <summary>
        /// Возвращает список параметров
        /// </summary>
        /// <returns></returns>
        public SqlParameter[] GetParams()
        {
            return this.GenerateParams().ToArray();
        }

        /// <summary>
        /// Возвращает список имен параметров
        /// </summary>
        /// <returns></returns>
        public string GetParamNames()
        {
            string result = "";
            for(int i = 0; i < data.Length; i++)
            {
                result += (result != "" ? ", " : "")
                    + GetParamName(i);
            }
            return result;
        }
    }
}
