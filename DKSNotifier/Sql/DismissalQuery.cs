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
	/// Скрипт по уволенным
	/// </summary>
    public class DismissalQuery: IQuery
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
        public DismissalQuery(string codeNO, int days, string[] listOrdTypes)
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
				
				SELECT DISTINCT 		
					 EMPLOYERS.LINK
					,D.FULL_NAME
					,FACES_MAIN_TBL.FIO_SHORT [FIO]
					,EMPL.[LOGIN]	
					,STF.CODE [SUBDIV_CODE]	
					,DIS.[SUBDIV] [SUBDIV_NAME]
					,DIS.[POST] [POST_NAME]
					,DIS.[TAB_NUM]
					,DIS.[DIS_DATE]
					,REASON.[SNAME] [DESCRIPTION]	
					,ORD.NUMBER ORD_NUMBER
					,ORD.[DATE] ORD_DATE
				FROM ORDERS_TBL ORD
					LEFT JOIN ITEM_DIS_FACE DIS ON ORD.LINK = DIS.ORDER_LINK
					INNER JOIN DICTIONARY_PUV AS REASON ON DIS.REASON = REASON.LINK	
					LEFT JOIN SUBDIV STF ON STF.NAME = DIS.SUBDIV AND STF.DATE_END > GETDATE()
					LEFT JOIN SUBDIV_HEAD STF_HEAD ON STF_HEAD.LINK = STF.LINK_UP
					INNER JOIN DEPARTMENTS_TBL AS D ON ORD.LINK_DEP_OWN = D.LINK	
					LEFT JOIN EMPLOYERS_SID_TBL EMPL ON EMPL.LINK_EMPL = DIS.LINK_EMPL
					LEFT JOIN EMPLOYERS_TBL EMPLOYERS ON EMPLOYERS.LINK = EMPL.LINK_EMPL
					LEFT JOIN FACES_MAIN_TBL ON FACES_MAIN_TBL.LINK = EMPLOYERS.FACE_LINK
				WHERE ORD.[TYPE] IN (%ORDTYPES%)
					AND (DIS.ACTIVE = 1)	
					AND LEFT(EMPL.[LOGIN], 4) = @codeNO
					AND DIS.DIS_DATE BETWEEN @dDate1 AND @dDate2 
					AND EMPLOYERS.LINK IS NOT NULL
					AND STF_HEAD.DEP_LINK = ORD.LINK_DEP_OWN
				ORDER BY DIS.DIS_DATE
            ";
        }


    }
}
