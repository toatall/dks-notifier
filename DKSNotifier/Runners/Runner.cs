using System;
using System.Collections.Generic;
using System.Linq;
using DKSNotifier.Logs;
using DKSNotifier.Model;
using DKSNotifier.Sql;
using DKSNotifier.Storage;
using System.IO;
using System.Data;
using DKSNotifier.Formatter;

namespace DKSNotifier.Runners
{
	/// <summary>
	/// Выполнение процесса получения данных о сотруднике
	/// </summary>
    internal abstract class Runner
    {
        #region Поля

        /// <summary>
        /// объект лога
        /// </summary>
        protected readonly Log log;

		/// <summary>
		/// строка подключения к MS SQL Server
		/// </summary>
		protected readonly string connectionStringMssql;

		/// <summary>
		/// объект для хранения полученных данных о сотрудниках
		/// </summary>
		protected readonly IStorage storage;		

		/// <summary>
		/// Экземпляр класса реализованный от IQuery
		/// </summary>
		protected readonly IQuery query;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionStringMssql">строка подключения MS SQL Server</param>
        /// <param name="storage">хранилище данных о сотрудниках</param>
        /// <param name="log">лог</param>
		/// <param name="query">экземпляр класса реализованный от IQuery</param>
        public Runner(string connectionStringMssql, IStorage storage, Log log, IQuery query)
        {
			this.log = log;            
			this.connectionStringMssql = connectionStringMssql;
			this.storage = storage;
			this.query = query;
        }		

		/// <summary>
		/// Запуск процесса получения, сохранения данных
		/// </summary>
		/// <param name="title">заголовок</param>
		/// <param name="formatter">объект форматирования данных</param>
		/// <returns>отформатированные данные для уведомления</returns>
		public string Start(string title, IFormatter formatter)
        {			
			try
			{
				this.log.Info(title + ": запуск...");
                #region работа с MS SQL Server
				// инициализация sql-сервера
                Mssql<IEntity> mssql = new Mssql<IEntity>(connectionStringMssql, log, this.query);
				// заполнение списка объектами из данных, полученных от sql сервера  
				IEnumerable<IEntity> list = mssql.Select(FillEntity);
				// фильтрация данных (удаление записей, которые были добавлены в хранилище)
				IEnumerable<IEntity> filtered = FilterEntities(list);
                #endregion
                this.log.Info(string.Format("Найдено записей: {0}", filtered.Count()));
				string text = null;
                if (filtered.Count() > 0)
				{
					// форматирование данных
					text = formatter.GetText(title, this.GetLabels(), this.GetData(filtered));
					// сохранение данных в хранилище
					this.SaveToStorage(filtered);
				}
				this.log.Info(title + ": завершение");
				return text;
			}
			catch (Exception e)
            {               
				if (e.Message.ToUpper().Contains("LOGIN FAILED"))
				{
					throw new SqlLoginFailedException(e.Message);
                }
				this.log.Error("Произошла ошибка: " + e.Message);
				this.log.Error(e.StackTrace);
            }
			return null;
        }

        /// <summary>
        /// Создает объект IEntity из данных, полученных от sql-сервера
        /// </summary>
        /// <param name="record">данные, полученные от sql-сервера</param>
        /// <returns>созданный новый объект, заполненный данными, полученными от sql-сервера</returns>
        protected abstract IEntity FillEntity(IDataRecord record);

		/// <summary>
		/// Возвращает список залоговков
		/// </summary>
		/// <returns>список залоговков</returns>
		protected abstract string[] GetLabels();

        /// <summary>
        /// Формирует двумерный массив из коллекции объектов IEntity
        /// </summary>
        /// <param name="entities">коллекция объектов</param>
        /// <returns>двумерный массив</returns>        
        protected abstract string[][] GetData(IEnumerable<IEntity> entities);

        /// <summary>
        /// Сохранение данных в хранилище
        /// </summary>
        /// <param name="entities">коллекция объектов</param>
        protected abstract void SaveToStorage(IEnumerable<IEntity> entities);

		/// <summary>
		/// Фильтрация коллекции объектов IEntry
		/// (для удаления записей, которые сохранены в хранилище)
		/// </summary>
		/// <param name="entities">коллекция объектов</param>
		/// <returns>отфильтрованная коллекция объектов</returns>
		protected IEnumerable<IEntity> FilterEntities(IEnumerable<IEntity> entities)
        {
			return entities.Where(t => !storage.CheckEntity(t.TypeEntity(), t.GetUnique()));
        }		
	
    }
}
