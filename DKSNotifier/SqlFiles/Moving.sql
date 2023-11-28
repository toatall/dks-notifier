/* Переводы сотрудников (из отдела в отдел, с должности на должность и др) */

DECLARE @dDate1 DATETIME, @dDate2 DATETIME, @Days INT

SET @Days = 4 -- количество дней +- от текущей даты

SET @dDate1 = DATEADD(DAY, -@Days, GETDATE())
SET @dDate2 = DATEADD(DAY, @Days, GETDATE())

SELECT 
	 ORD.[LINK]
	,ORD.[TYPE]
	,DEP.FULL_NAME [DEP_NAME]
	,ORD.NUMBER ORD_NUMBER
	,ORD.[DATE] ORD_DATE
	,ORD.COMMENT
	,ORD.[STATUS]
	,M.[ACTIVE]
	,M.[STATUS]
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
WHERE ORD.[TYPE] IN (2, 102)
	AND (M.ACTIVE = 1)
	AND ORD.LINK_DEP_OWN IN (SELECT DISTINCT LINK FROM DICTIONARY_DEPARTMENT_ALL WHERE CODE = '8600')
	AND M.DATE_BEGIN BETWEEN @dDate1 AND @dDate2
	AND EMPLOYERS.LINK IS NOT NULL
