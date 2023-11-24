using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Storage
{
    /// <summary>
    /// Хранилище полученной информации о сотрудниках
    /// </summary>
    internal interface IStorage
    {
        /// <summary>
        /// Проверка наличия записи в хранилище
        /// </summary>
        /// <param name="type">тип записи</param>
        /// <param name="unid">идентификатор</param>
        /// <returns></returns>
        bool CheckEntity(string type, string unid);

        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="type">тип записи</param>
        /// <param name="unid">идентификатор</param>
        /// <param name="description">дополнительная информация</param>
        void Add(string type, string unid, string description = "");

    }

}
