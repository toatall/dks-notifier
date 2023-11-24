using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Model
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IEntity
    {
        /// <summary>
        /// Уникальный номер записи
        /// </summary>
        /// <returns></returns>
        string GetUnique();

        /// <summary>
        /// Тип записи
        /// (краткое наименование)
        /// </summary>
        /// <returns></returns>
        string TypeEntity();

    }
}
