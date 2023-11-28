using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Sql
{
    /// <summary>
    /// Интерфейс представляет метод, который должен передавать скрипт и параметры
    /// </summary>
    internal interface IQuery
    {
        /// <summary>
        /// Подготовка скрипта и параметров и передача их в объект sqlCommand
        /// </summary>
        /// <param name="sqlCommand">экземпляр объекта SqlCommand</param>
        void PrepareSqlCommand(SqlCommand sqlCommand);

    }
}
