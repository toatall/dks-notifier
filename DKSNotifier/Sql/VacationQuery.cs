﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Sql
{
	/// <summary>
	/// Скрипт по отпускам
	/// </summary>
    public class VacationQuery: IQuery
    {
		/// <summary>
		/// Код НО
		/// </summary>
		string codeNO;

		/// <summary>
		/// Количество дней
		/// </summary>
        private int days;

		/// <summary>
		/// Виды приказов
		/// </summary>
		private string[] listOrdTypes;

		/// <summary>
		/// Типы отпусков
		/// </summary>
		private string[] listTypeVacations;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="codeNO">код НО</param>
		/// <param name="days">кол-во дней</param>
		/// <param name="listOrdTypes">виды приказов</param>
		/// <param name="listTypeVacations">типы отпусков</param>
        public VacationQuery(string codeNO, int days, string[] listOrdTypes, string[] listTypeVacations)
        {
			this.codeNO = codeNO;
            this.days = days;
			this.listOrdTypes = listOrdTypes;
			this.listTypeVacations = listTypeVacations;
        }

		/// <inheritdoc/>
		public void PrepareSqlCommand(SqlCommand sqlCommand)
		{
            sqlCommand.Parameters.AddWithValue("@codeNO", this.codeNO);

            InCauseParamSql<string> inOrdTypes = new InCauseParamSql<string>(SqlDbType.NVarChar, this.listOrdTypes, "OrdType");
			InCauseParamSql<string> inVacationTypes = new InCauseParamSql<string>(SqlDbType.NVarChar, this.listTypeVacations, "VacationType");
            sqlCommand.CommandText = GetQuery()
				.Replace("%ORDTYPES%", inOrdTypes.GetParamNames())
				.Replace("%VACATIONYPES%", inVacationTypes.GetParamNames());			
			sqlCommand.Parameters.AddRange(inOrdTypes.GetParams());
			sqlCommand.Parameters.AddRange(inVacationTypes.GetParams());

            sqlCommand.Parameters.AddWithValue("@days1", days);
            sqlCommand.Parameters.AddWithValue("@days2", days);
        }		

		/// <summary>
		/// Возвращает текст скрипта
		/// </summary>
		/// <returns></returns>
        protected string GetQuery()
        {
            return @"
				DECLARE @dNow DATETIME, @dDate1 DATETIME, @dDate2 DATETIME
				SET @dNow = GETDATE()
				SET @dDate1 = DATEADD(DAY, -@days1, GETDATE())
				SET @dDate2 = DATEADD(DAY,  @days2, GETDATE())
				
				SELECT
					V.[NAME] AS [TYPE_NAME], 
					DEP.FULL_NAME [DEP_NAME],
					DATEDIFF(DAY, ITV.DATE_BEGIN, ITV.DATE_END) + 1 [COUNT_DAYS],
					ORD4.NUMBER ORD_NUMBER,
					ORD4.[DATE] ORD_DATE,
					ITV.DATE_BEGIN, 
					ITV.DATE_END [DATE_END_REAL],	
					EMPL.[LOGIN],
					FACES_MAIN_TBL.FIO_SHORT [FIO],
					ITV.TAB_NUM, 
					ITV.SUBDIV, 
					ITV.POST,	
					ORD4.LINK, 
					ITV.LINK ITV_LINK
				FROM DEPARTMENTS_TBL DEP, ORDERS_TBL ORD4, ITEM_VACATION ITV
					INNER JOIN DICTIONARY_VACATION AS V ON ITV.TYPE_LINK = V.LINK 
					LEFT JOIN EMPLOYERS_TBL EMPLOYERS ON EMPLOYERS.LINK = ITV.LINK_EMPL
					LEFT JOIN FACES_MAIN_TBL ON FACES_MAIN_TBL.LINK = EMPLOYERS.FACE_LINK
					LEFT JOIN EMPLOYERS_SID_TBL EMPL ON EMPL.LINK_EMPL = ITV.LINK_EMPL
				WHERE (ORD4.LINK = ITV.ORDER_LINK  AND (ITV.ACTIVE = 1))
					AND LEFT(EMPL.[LOGIN], 4) = @codeNO
					AND ITV.DATE_BEGIN BETWEEN @dDate1 AND @dDate2
					AND ORD4.LINK_DEP_OWN = DEP.LINK
					AND ORD4.TYPE IN (%ORDTYPES%)
					AND V.CODE IN (%VACATIONYPES%)
				ORDER BY ITV.DATE_BEGIN
            ";
        }


    }
}
