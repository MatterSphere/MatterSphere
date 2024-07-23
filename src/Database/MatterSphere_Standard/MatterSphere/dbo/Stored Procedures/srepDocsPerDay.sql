

CREATE PROCEDURE [dbo].[srepDocsPerDay]
(
	@UI uUICultureInfo = '{default}'
	, @DEPARTMENT nvarchar(15) = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS 

DECLARE @SELECT nvarchar(2000)
DECLARE @WHERE nvarchar(2000)
DECLARE @GROUPBY nvarchar(200)
DECLARE @ORDERBY nvarchar(200)

--- BUILD THE SELECT CLAUSE
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	Convert ( varchar(20) , D.Created , 103 ) as Created
	, A.cdDesc AS DeptDesc
	, D.docType 
	, Count(D.docID) as DocCount
FROM         
	dbo.dbDocument D
INNER JOIN
	dbo.dbFile F ON D.fileID = F.fileID 
LEFT JOIN
	dbo.GetCodeLookupDescription ( ''DEPT'' , @UI ) A  ON A.cdCode = F.fileDepartment '

---- SET THE WHERE CLAUSE dates are mandatory
SET @WHERE = ' WHERE (D.Created >= @STARTDATE AND D.Created < @ENDDATE) '

--- DEPARTMENT CLAUSE
IF(@DEPARTMENT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPARTMENT '
END

-- Group By Clause
SET @GROUPBY = N'
GROUP BY
	Convert ( varchar(20) , D.Created , 103 )
	, A.cdDesc
	, D.docType '

-- Order By Clause
SET @ORDERBY = N'
ORDER BY
	1
	, A.cdDesc
	, D.docType '
	
DECLARE @SQL nvarchar(4000)
--- ADD CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@GROUPBY) + Rtrim(@ORDERBY)

--- DEBUG PRINT
--  PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI uUICultureInfo
	, @DEPARTMENT nvarchar(15)
	, @STARTDATE datetime
	, @ENDDATE datetime'
	, @UI
	, @DEPARTMENT
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepDocsPerDay] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepDocsPerDay] TO [OMSAdminRole]
    AS [dbo];

