using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Sql
{
    /// <summary>
    /// Исключение для ошибки "LOGIN FILED", которая может возникнуть при подключении к серверу MS SQL Server
    /// </summary>
    internal class SqlLoginFailedException: Exception
    {
        /// <inheritdoc/>
        public SqlLoginFailedException(string message) : base(message) { }
    }
}
