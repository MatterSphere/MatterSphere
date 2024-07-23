

CREATE PROCEDURE [dbo].[srepPercentTimeAct]
(
	@UI uUICultureInfo = '{default}'
	, @STARTDATE datetime
	, @ENDDATE datetime
	, @FEEEARNER bigint = null
	, @ACTIVITY nvarchar(30) = null
)

AS
DECLARE @sql1 nvarchar(4000) , @sqlWhere nvarchar(500) , @sql2 nvarchar(1500)
SET @sql1 = '
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT    
	U.usrFullName ,
	X.[cdDesc] as DeptDesc ,
	Y.[cdDesc] as actDesc ,
	Convert( decimal(5,2) , (B.TotalTime * 100) / A.TotalTime) as Percentage ,	
	B.[TotalTime] as Mins ,
	A.[TotalTime] as [Total Activity Time]
FROM  
	(SELECT Convert ( decimal(10,2) , Sum(timeMins)) as TotalTime FROM [dbo].[dbTimeLedger] WHERE [timeRecorded] >= @StartDate AND [timeRecorded] < @EndDate xxx ' 

SET @sqlWhere = ''

IF @FeeEarner IS NOT NULL
	SET @sqlWhere = 'AND [feeUsrID] = @FeeEarner '
IF @Activity IS NOT NULL
	SET @sqlWhere = ' AND [timeActivityCode] = @Activity '

SET @sqlWhere = @sqlWhere +  '  ) A '
SET @sql1 = REPLACE ( @sql1 , 'xxx' , @sqlWhere )

SET @sql2 = ' 
CROSS JOIN
	( SELECT Sum(T.timeMins) as TotalTime, T.[timeActivityCode] , T.[feeUsrID] , F.[fileDepartment] FROM [dbo].[dbTimeLedger] T JOIN [dbo].[dbFile] F ON F.[fileID] = T.[fileID] 
		 WHERE T.[timeRecorded] >= @StartDate AND T.[timeRecorded] < @EndDate GROUP BY T.[timeActivityCode] , F.[fileDepartment] , T.[feeUsrID]) B
JOIN
	[dbo].[dbUser] U ON B.[feeUsrID] = U.[usrID]
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''DEPT'' , @UI) X ON X.[cdCode] = B.[fileDepartment]
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''TIMEACTCODE'' , @UI ) Y ON Y.[cdCode] = B.[timeActivityCode] '

SET @sqlWhere = ''

IF @FeeEarner IS NOT NULL
	SET @sqlWhere = @sqlWhere +  'AND B.[feeUsrID] = @FeeEarner '
IF @Activity IS NOT NULL
	SET @sqlWhere = @sqlWhere + ' AND B.[timeActivityCode] = @Activity '

IF @sqlWhere <> ''
	SET @sqlWhere = ' WHERE ' + Substring ( @sqlWhere , 5 , 496 )
SET @sql2 = @sql2 + @sqlWhere

SET @sql1 = @sql1 + @sql2 + ' ORDER BY 2 , 4 Desc'

 PRINT @sql1
	EXEC sp_executesql @sql1, 
N'
	@UI nvarchar(10)
	, @STARTDATE datetime
	, @ENDDATE datetime
	, @FEEEARNER bigint
	, @ACTIVITY nvarchar(30) ' 
	, @UI
	, @STARTDATE
	, @ENDDATE
	, @FEEEARNER
	, @ACTIVITY

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPercentTimeAct] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPercentTimeAct] TO [OMSAdminRole]
    AS [dbo];

