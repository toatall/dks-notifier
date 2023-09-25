using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKSNotifier.Logs;
using DKSNotifier.Model;
using DKSNotifier.Sql;
using DKSNotifier.Email;
using DKSNotifier.XML;
using System.IO;
using System.Data;

namespace DKSNotifier.Runners
{
    internal abstract class Runner
    {
        protected readonly Log log;
		protected readonly string connectionStringMssql;
		protected readonly XmlStorage xmlStorage;
		protected readonly string sqlQueryFile;
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataFilePath"></param>
		/// <param name="connectionStringMssql"></param>
		/// <param name="xmlFileName"></param>
        public Runner(string connectionStringMssql, XmlStorage xmlStorage, Log log, string sqlQueryFile)
        {
			this.log = log;            
			this.connectionStringMssql = connectionStringMssql;
			this.xmlStorage = xmlStorage;
			this.sqlQueryFile = sqlQueryFile;
        }		

		/// <summary>
		/// Запуск процессов
		/// </summary>
		/// <returns></returns>
		public string Start(string title)
        {			
			try
			{
				log.Info(title + " запуск");
				Mssql<IEntity> mssql = new Mssql<IEntity>(connectionStringMssql, log);
				string query = File.ReadAllText(sqlQueryFile);				
				IEnumerable<IEntity> list = mssql.Select(query, FillEntity);
				IEnumerable<IEntity> filtered = FilterEntities(list);
				log.Info(string.Format("Найдено записей: {0}", filtered.Count()));
				string message = FormatEmailMessage(filtered);
				SaveToXml(filtered);
				log.Info(title + " завершение");
				return message;
			}
			catch (Exception e)
            {
				log.Error("Произошла ошибка: " + e.Message);
				log.Error(e.StackTrace);
            }
			return null;
        }

		/// <summary>
		/// Создание модели IEntity из данных, полученных из БД
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		protected abstract IEntity FillEntity(IDataRecord record);

		/// <summary>
		/// Формирование текстовой информации из данных модели IEntity
		/// для направления по почте
		/// </summary>
		/// <param name="entities"></param>
		/// <returns></returns>
		protected abstract string FormatEmailMessage(IEnumerable<IEntity> entities);

		/// <summary>
		/// Сохранение данных в XML-файле
		/// </summary>
		/// <param name="entities"></param>
		protected abstract void SaveToXml(IEnumerable<IEntity> entities);

		/// <summary>
		/// Проверяется направлялось ли по данному событию уведомление
		/// Если уже направлялось, то оно не берется в дальнейшую обработку
		/// </summary>
		/// <param name="entities"></param>
		/// <returns></returns>
		protected IEnumerable<IEntity> FilterEntities(IEnumerable<IEntity> entities)
        {
			return entities.Where(t => !xmlStorage.CheckEntity(t.TypeEntity(), t.GetUnique()));
        }		
	
    }
}
