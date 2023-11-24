using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Formatter
{
    /// <summary>
    /// Форматирование данных
    /// </summary>
    internal interface IFormatter
    {
        /// <summary>
        /// Получение отформатированных данных
        /// </summary>
        /// <param name="title">залоговок</param>
        /// <param name="headers">заголовки</param>
        /// <param name="rows">данные</param>
        /// <returns>отформатированные текстовые данные</returns>
        string GetText(string title, string[] headers, string[][] rows);
    }
}
