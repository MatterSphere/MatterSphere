

CREATE PROCEDURE [dbo].[srepClUpTypes]
(
	@UI uUICultureInfo = '{default}' ,
	@TYPE nvarchar(30) = NULL ,
	@CULTURE uUICultureInfo = NULL
)

AS 

declare @SQL nvarchar(4000)
declare @SELECT nvarchar(2000)
declare @WHERE nvarchar(2000)
declare @ORDERBY nvarchar(30)

--- Select Statement for the base Query
set @Select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	X.cdDesc AS clupDesc,
	CLUP.cdCode AS Code,
	CLUP.cdDesc AS Description,
	CLUP.cdHelp AS ToolTips,
	CLUP.cdUICultureInfo,
	CLUP.cdAddLink,
	CLUP.cdHelp,
	CLUP.cdNotes
FROM         
	dbCodeLookup CLUP
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''CLUPTYPES'', @UI) AS X ON X.cdCode = CLUP.cdType '

--- Build Where Clause
SET @WHERE = ''

IF (@TYPE IS NOT NULL)
     SET @where = ' WHERE CLUP.cdType = @TYPE '
ELSE
	SET @where =  ' WHERE CLUP.cdType IN (''SOURCE'',''SUBASSOC'',''CONTTYPE'',''CLTYPE'',''FILETYPE'',
	      		      ''DEPT'',''FUNDTYPE'',''IDENT'',''INFOADDTYPE'',''INFOTYPE'',
					  ''LOCTYPE'',''PRECCAT'',''PRECSUBCAT'',''PRECMINORCAT'',''SUBCONTACT'',
				  ''TASKTYPE'',''USRROLES'') '

IF (@CULTURE IS NOT NULL)
BEGIN
     SET @where = @where + ' AND CLUP.cdUICultureInfo = @CULTURE '
END

--- Group By Clause
set @ORDERBY = N' ORDER BY CLUP.cdCode '

--- Add Statements together
set @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

-- Debug Print
-- PRINT @SQL


exec sp_executesql @SQL, 
N'@UI uUICultureInfo,
	@TYPE nvarchar(30),
	@CULTURE uUICultureInfo',
	@UI,
	@TYPE,
	@CULTURE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClUpTypes] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClUpTypes] TO [OMSAdminRole]
    AS [dbo];

