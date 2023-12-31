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
	/// Скрипт по переводам
	/// </summary>
    public class MovingQuery: IQuery
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
		/// Конструктор
		/// </summary>
		/// <param name="days">кол-во дней</param>
		/// <param name="listOrdTypes">виды приказов</param>
        public MovingQuery(string codeNO, int days, string[] listOrdTypes)
        {
			this.codeNO = codeNO;
            this.days = days;
			this.listOrdTypes = listOrdTypes;
        }

		/// <inheritdoc/>
		public void PrepareSqlCommand(SqlCommand sqlCommand)
		{
            sqlCommand.Parameters.AddWithValue("@codeNO", this.codeNO);

            InCauseParamSql<string> inOrdTypes = new InCauseParamSql<string>(SqlDbType.NVarChar, this.listOrdTypes, "OrdType");            
            sqlCommand.CommandText = GetQuery().Replace("%ORDTYPES%", inOrdTypes.GetParamNames());
            sqlCommand.Parameters.AddRange(inOrdTypes.GetParams());

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
					 ORD.[LINK]					
					,DEP.FULL_NAME [DEP_NAME]
					,ORD.NUMBER ORD_NUMBER
					,ORD.[DATE] ORD_DATE
					,ORD.COMMENT					
					,M.[TAB_NUM]	
					,FACES_MAIN_TBL.FIO_SHORT [FIO]
					,M.[SUBDIV]
					,M.[POST]
					,M.[NEW_SUBDIV]
					,M.[NEW_POST]
					,M.[DATE_BEGIN]
					,EMPL.[LOGIN]
				FROM ORDERS_TBL ORD
					LEFT JOIN ITEM_MOVE M ON ORD.LINK = M.ORDER_LINK
					LEFT JOIN EMPLOYERS_SID_TBL EMPL ON EMPL.LINK_EMPL = M.LINK_EMPL
					LEFT JOIN EMPLOYERS_TBL EMPLOYERS ON EMPLOYERS.LINK = EMPL.LINK_EMPL
					LEFT JOIN FACES_MAIN_TBL ON FACES_MAIN_TBL.LINK = EMPLOYERS.FACE_LINK
					LEFT JOIN DEPARTMENTS_TBL DEP ON DEP.LINK = ORD.LINK_DEP_OWN
				WHERE ORD.[TYPE] IN (%ORDTYPES%)
					AND (M.ACTIVE = 1)
					AND LEFT(EMPL.[LOGIN], 4) = @codeNO
					AND M.DATE_BEGIN BETWEEN @dDate1 AND @dDate2 
					AND EMPLOYERS.LINK IS NOT NULL
				ORDER BY M.DATE_BEGIN
            ";
        }


    }
}
