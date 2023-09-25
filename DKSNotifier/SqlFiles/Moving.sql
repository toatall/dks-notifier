DECLARE @dDate1 DATETIME, @dDate2 DATETIME,	@dDate_1 DATETIME, @nOtborLink1 INT, @nOtborLink2 INT

DECLARE @Days INT 
SET @Days = 4


SET @dDate1 = DATEADD(DAY, @Days * -1, GETDATE())
SET @dDate2 = DATEADD(DAY, @Days, GETDATE())
SET @dDate_1 = CAST(CONVERT(VARCHAR, GETDATE(), 104) AS DATETIME)
SET @nOtborLink1 = 0
SET @nOtborLink2 = 0

DECLARE
		@nOrgMode TINYINT /*ВИД ОРГАНИЗАЦИИ*/,
		@nFaceMode TINYINT /*Сотрудники за период*/,
		@dReportWorkingDate DATETIME /*Рабочая дата (или конец периода)*/,
		@nExpirMode INT /*Используемые виды стажа*/,
		@nOrderItemStatus INT /*Статус пунктов приказа*/

	SELECT @nOrgMode = 0, @nFaceMode = 1, @nExpirMode = 0
	SET @dReportWorkingDate = (SELECT REP_DATE FROM dbo.OBJECT_VAR_DATE WHERE SID = SUSER_SID() )
	SET @nOrderItemStatus = -1

	EXEC dbo.BufferClear

	SET @nFaceMode = 1

	SET @nOrderItemStatus =1

	SET @nOrgMode  = 2

	IF OBJECT_ID('tempdb..#WORKERS') IS NOT NULL
		DROP TABLE #WORKERS
	
	CREATE TABLE #WORKERS(
		LINK INT, 
		FACE_LINK INT, 	
		TAB_NUM VARCHAR (6),
		KONTR_DATE DATETIME,
		KONTR_NUM VARCHAR(25),
		NUMBER_LD VARCHAR(25),
		GRAPH_LINK INT,
		GRAPH_NAME VARCHAR(254),
		POLIS_SER VARCHAR(25),
		POLIS_NUM VARCHAR(25),
		START_DATE DATETIME, 
		END_DATE DATETIME,
		STATUS INT,
		STATUS_NAME VARCHAR(60),
		LINK_DEP INT,
		FM VARCHAR(25),
		IM VARCHAR(25),
		OT VARCHAR(25),
		SEX INT,
		SEX_TXT VARCHAR(10),
		SEX_TXT_S CHAR(1),
		[BIRTHDAY] [datetime],
		[BIRTH_YEAR] INT,
		[PLACEBIRTH] [varchar] (254),
		[DOC_LINK] [int],
		[DOC_NUM] [varchar] (30),
		[DOC_DATE] [datetime],
		[WHOM] VARCHAR(254),
		[GRAZD_LINK] [int],
		[NAT_LINK] [int],
		[NUMB_PF] [varchar] (14),
		[INN] [varchar] (12),
		[SEM_LINK] [int],
		[APROP_LINK] [int],
		[HOUSE_PROP] [varchar] (10),
		[CORPUS_PROP] [varchar] (10),
		[FLAT_PROP] [varchar] (10),
		[PHONE_PROP] [varchar] (20),
		[REGIST_DATE] [datetime],
		[AFACT_LINK] [int],
		[HOUSE_FACT] [varchar] (10),
		[CORPUS_FACT] [varchar] (10),
		[FLAT_FACT] [varchar] (10),
		[PHONE_FACT] [varchar] (20),
		[EDUC_LINK] [int],
		[PROFESS] [varchar] (254),
		[NAV_PM] [TINYINT],
		[NAV_PM_TXT] VARCHAR(3),
		[NAV_K] [TINYINT],
		[NAV_K_TXT] VARCHAR(3),
		[NAV_S] [TINYINT],
		[NAV_S_TXT] VARCHAR(3),
		[NAV_P] [varchar] (10),
		[CLIMAT] [tinyint],
		[DOP_SVED] varchar(254),
		[IMNS] varchar(4),
		[OTVO_LINK] int,
		[VKAT_LINK] int,
		[VZVAN_LINK] int,
		[VSOST_LINK] int ,
		[VOEN_VUS] varchar(12),
		[VGODN_LINK] int ,
		[VK_LINK]  int,
		[VGRUP_LINK] int,
		[VSPUCET] [varchar] (30),
		[VROD] [varchar] (60),
		[VBILET] [varchar] (15),
		[RES] [TINYINT],
		[RES_TXT] VARCHAR(3),
		[RES_NUM] [varchar] (25),
		[MOB] [TINYINT],
		[MOB_TXT] VARCHAR(3),
		[NUM_CP] [varchar] (254),
		STAFF_LINK INT
	)

EXEC KIR_FILL_T_WORKERS @nOrgMode, @dReportWorkingDate


SELECT 
	WR.FM,
	WR.TAB_NUM,
	WR.IM,
	WR.[OT],
	DEP.NAME,
	dbo.SQUERY_GET_SUBDIV_NAME(ITM.STAFF_LINK, ITM.DATE_BEGIN) NEW_SUBDIV,
	--ITM.OKLAD,
	dbo.SQUERY_GET_POST_BY_STAFF(ITM.STAFF_LINK) NEW_POST,
	dbo.SQUERY_GET_PREV_POST(itm.link_empl, itm.date_begin) OLD_POST,
	dbo.SQUERY_GET_PREV_SUBDIV(ITM.LINK_EMPL, ITM.DATE_BEGIN) OLD_SUBDIV,
	ITM.DATE_BEGIN,
	WR.LINK
FROM #WORKERS WR, DEPARTMENTS DEP, ORDERS ORD2, dbo.MOVES ITM
WHERE (ORD2.LINK = ITM.ORDER_LINK AND (@nOrderItemStatus < 0 OR ITM.ACTIVE = @nOrderItemStatus) )
	AND (WR.start_date <= @dDate_1) and (WR.end_date >= @dDate_1 or WR.end_date is null)
	AND DEP.LINK=(SELECT DEP_LINK FROM USER_INFO)
	AND ITM.DATE_BEGIN BETWEEN  @dDate1 AND @dDate2
	AND ORD2.DEP_LINK = DEP.LINK
	AND ORD2.TYPE IN (2, 102)
	AND WR.LINK = ITM.LINK_EMPL
	AND WR.LINK_DEP=DEP.LINK
	AND (@nOtborLink1=0 OR EXISTS(SELECT RECORD FROM OBJECT_VAR_FICTION WHERE LINK_UP=@nOtborLink1 AND ITM.TYPE = RECORD))

DROP TABLE #WORKERS
EXEC dbo.BufferClear