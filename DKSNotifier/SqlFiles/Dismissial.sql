DECLARE @dDate1 DATETIME, @dDate2 DATETIME,	@nOtborLink1 INT, @nOtborLink2 INT

DECLARE @Days INT 
SET @Days = 4


SET @dDate1 = DATEADD(DAY, @Days * -1, GETDATE())
SET @dDate2 = DATEADD(DAY, @Days, GETDATE())
SET @nOtborLink1 = 0
SET @nOtborLink2 = 0

DECLARE
		@nOrgMode TINYINT /*ВИД ОРГАНИЗАЦИИ*/,
		@nFaceMode TINYINT /*Сотрудники за период*/,
		@dReportWorkingDate DATETIME /*Рабочая дата (или конец периода)*/,
		@nExpirMode INT /*Используемые виды стажа*/,
		@nOrderItemStatus INT /*Статус пунктов приказа*/

	SELECT @nOrgMode = 0, @nFaceMode = 1, @nExpirMode = 0
	SET @dReportWorkingDate = GETDATE()--(SELECT REP_DATE FROM dbo.OBJECT_VAR_DATE WHERE SID = SUSER_SID() )
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
	DEP.FULL_NAME, 
	WR.TAB_NUM, 
	WR.IM, 
	WR.FM, 
	WR.[OT], 
	stf.SUBDIV_NAME, 
	STF.POST_NAME, 
	STF.SUBDIV_CODE, 
	ITDF.DIS_DATE, 
	(SELECT [SNAME] FROM dbo.DICTIONARY_PUV WHERE LINK = ITDF.REASON_LINK) [DESCRIPTION], 
	--DEP.LINK, 
	WR.LINK
	--STF.LINK, 
	--ORD3.LINK, 
	--ITDF.LINK
FROM dbo.DICTIONARY_STAFF_FN(@dReportWorkingDate) STF, #WORKERS WR, DEPARTMENTS DEP, ORDERS ORD3, dbo.DIS_FACE ITDF
WHERE (ORD3.LINK = ITDF.ORDER_LINK AND (@nOrderItemStatus < 0 OR  ITDF.ACTIVE = @nOrderItemStatus) )
		AND (STF.DEP_LINK = DEP.LINK)
		AND (STF.STAFF_LINK = WR.staff_link)
		AND DEP.LINK=(SELECT DEP_LINK FROM USER_INFO)
		AND ITDF.DIS_DATE BETWEEN @dDate1 AND @dDate2
		AND ORD3.DEP_LINK = DEP.LINK
		AND ORD3.TYPE IN (3, 103)
		AND WR.LINK = ITDF.LINK_EMPL
		AND WR.LINK_DEP=DEP.LINK
		AND (@nOtborLink1=0 OR EXISTS(SELECT RECORD FROM OBJECT_VAR_RECORDS WHERE LINK_UP=@nOtborLink1 AND RECORD = STF.SUBDIV_LINK))
		AND (@nOtborLink2=0 OR EXISTS(SELECT RECORD FROM OBJECT_VAR_DICTIONARY_PUV_TABLE WHERE LINK_UP=@nOtborLink2 AND RECORD = ITDF.REASON_LINK))

DROP TABLE #WORKERS
EXEC dbo.BufferClear