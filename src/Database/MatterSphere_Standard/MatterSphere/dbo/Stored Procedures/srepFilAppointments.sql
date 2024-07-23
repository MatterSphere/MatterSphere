

CREATE PROCEDURE [dbo].[srepFilAppointments]
(
	@UI uUICultureInfo='{default}',
	@APPTYPE nvarchar(50) = null,
	@LOCATION nvarchar(50) = null,
	@CLNO nvarchar(50)= null,
	@ALLOCATEDTO int = null,
	@APPDESC nvarchar(50) = null,
	@STARTDATE datetime = null,
	@ENDDATE datetime = null
)

AS 

DECLARE @Sql nvarchar(4000)
DECLARE @Select nvarchar(2000)
DECLARE @Where nvarchar(1000)
DECLARE @OrderBy nvarchar(100)

--- Select Statement for the base Query
SET @Select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	A.appDate, 
	A.appLocation, 
	A.appDesc, 
	A.appType, 
    A.appLocation, 
	X.cdDesc AS AppTypeDesc,
	CL.clNo, 
	F.fileNo, 
	U.usrInits, 
	U.usrFullName, 
   	A.feeusrID, 
	U1.usrInits AS InitsWith, 
	U1.usrFullName AS FullNameWith
		, A.appAllDay
FROM    
	dbo.dbClient CL
INNER JOIN
    dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
    dbo.dbUser U ON F.filePrincipleID = U.usrID 
INNER JOIN
    dbo.dbAppointments A ON F.fileID = A.fileID 
INNER JOIN
    dbUser U1 ON A.feeusrID = U1.usrID
LEFT OUTER JOIN
	dbo.GetCodeLookupDescription ( ''APPTYPE'' , @UI ) X  ON X.cdCode = A.appType '


---- Build Where Clause
SET @Where = ''

--- CLNO Clause
IF(@CLNO IS NOT NULL AND @CLNO <> '')
BEGIN
	SET @Where = @Where + ' CL.clNo = @CLNO '
END

--- STARTDATE/ENDDATE Clause
IF(@STARTDATE IS NOT NULL)
BEGIN
	IF(@Where <> '' )
	BEGIN
		SET @Where = @Where + ' AND (A.appDate >= @STARTDATE AND A.appDate < @ENDDATE) '
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' (A.appDate >= @STARTDATE AND A.appDate < @ENDDATE) '
	END
END

--- ALLOCATEDTO Clause
IF(@ALLOCATEDTO IS NOT NULL)
BEGIN
	IF(@Where <> '' )
	BEGIN
		SET @Where = @Where + ' AND U1.usrID = @ALLOCATEDTO '
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' U1.usrID = @ALLOCATEDTO '
	END
END

--- APPTYPE Clause
IF(@APPTYPE IS NOT NULL)
BEGIN
	IF(@Where <> '' )
	BEGIN
	     set @Where = @Where + ' AND A.appType = @APPTYPE '
	END
	ELSE
	BEGIN
	     set @Where = @Where + ' A.appType = @APPTYPE '
	END
END

--- APPDESC Clause
--- A further check is required in case the user enters a value initially and then deletes it
IF(@APPDESC IS NOT NULL AND @APPDESC <> '')
BEGIN
	IF(@Where <> '' )
	BEGIN
	     set @Where = @Where + ' AND A.appDesc LIKE ''%'' + @APPDESC + ''%'' '
	END
	ELSE
	BEGIN
	     set @Where = @Where + ' A.appDesc LIKE ''%'' + @APPDESC + ''%'' '
	END
END

--- LOCATION Clause
IF(@LOCATION IS NOT NULL AND @LOCATION <> '')
BEGIN
	IF(@Where <> '' )
	BEGIN
	     set @Where = @Where + ' AND A.appLocation LIKE ''%'' + @LOCATION + ''%'' '
	END
	ELSE
	BEGIN
	     set @Where = @Where + ' A.appLocation LIKE ''%'' + @LOCATION + ''%'' '
	END
END

--- Complete Where clause
IF(@Where <> '')
BEGIN
	SET @Where = N' WHERE ' + @Where
END

--- Build the Order By clause
SET @OrderBy = N' ORDER BY A.appDate '

--- Build the command
SET @Sql = Rtrim(@Select) + Rtrim(@Where) + Rtrim(@OrderBy)

--- Debug Print
-- PRINT @Sql

exec sp_executesql @Sql,
N'
	@UI nvarchar(10)
	, @APPTYPE nvarchar(50)
	, @LOCATION nvarchar(50)
	, @CLNO nvarchar(50)
	, @ALLOCATEDTO int
	, @APPDESC nvarchar(50)
	, @STARTDATE datetime
	, @ENDDATE datetime'
	, @UI
	, @APPTYPE
	, @LOCATION
	, @CLNO
	, @ALLOCATEDTO
	, @APPDESC
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilAppointments] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilAppointments] TO [OMSAdminRole]
    AS [dbo];

