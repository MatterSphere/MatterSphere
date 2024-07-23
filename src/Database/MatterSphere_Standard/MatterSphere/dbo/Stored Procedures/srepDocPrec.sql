

CREATE PROCEDURE [dbo].[srepDocPrec] 
(
	@UI uUICultureInfo='{default}'
	, @USER nvarchar(50) = NULL
	, @DEPARTMENT nvarchar(50)= NULL
	, @FILETYPE nvarchar(50) = NULL
	, @FUNDTYPE nvarchar(50) = NULL
	, @STARTDATE datetime 
	, @ENDDATE datetime 
)

AS 

DECLARE @SELECT nVarChar(3000)
DECLARE @WHERE nvarchar(1500)
DECLARE @ORDERBY nvarchar(100)
DECLARE @SQL nVarChar(4000)

-- Start of the select
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	CL.clNo + ''/'' + F.fileNo AS Ref,
	X.cdDesc AS fileTypeDesc, 
	Y.cdDesc AS fundTypeDesc,
	Z.cdDesc AS fileDepartmentDesc, 
    U.usrFullName, 
	U.usrInits, 
	DOC.docID,
	CASE
		WHEN DOC.docAuthored IS NOT NULL THEN DOC.docAuthored
		ELSE DOC.Created
	END AS Created,
	CASE
		WHEN P.PrecTitle IS NULL THEN
			CASE
				WHEN P1.PrecTitle IS NOT NULL THEN P1.PrecTitle
				ELSE W.cdDesc
			END
		ELSE P.PrecTitle
	END as PrecTitle ,
	W.cdDesc as baseDesc
FROM         
	dbo.dbFile F
INNER JOIN
	dbo.dbDocument DOC ON F.fileID = DOC.fileID 
LEFT OUTER JOIN
	dbo.dbUser U ON DOC.CreatedBy = U.usrID 
LEFT OUTER JOIN
	dbo.dbPrecedents P ON DOC.docprecID = P.precID
LEFT OUTER JOIN
	dbo.dbPrecedents P1 ON DOC.docBasePrecID = P1.precID
INNER JOIN
	dbClient CL ON CL.clID = F.clID
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''FILETYPE'', @UI) AS X ON X.cdCode = F.fileType
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''FUNDTYPE'', @UI) AS Y ON Y.cdCode = F.fileFundCode
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''DEPT'', @UI) AS Z ON Z.cdCode = F.fileDepartment 
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''DOCTYPE'', @UI) AS W ON W.cdCode = DOC.docType '

-- Where clause, 
SET @WHERE = ' WHERE (DOC.Created >= @STARTDATE AND DOC.Created < @ENDDATE) '

-- User filter
IF (@USER IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND U.usrID = @USER '
END

-- Department
IF (@DEPARTMENT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPARTMENT '
END

-- FileType
IF (@FILETYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileType = @FILETYPE '
END

-- FundCode
IF (@FUNDTYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileFundCode = @FUNDTYPE '
END


-- Order by clause
SET @ORDERBY = N' ORDER BY DOC.Created '


-- build the query
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

-- print @sql

exec sp_executesql @sql,
 N'
	@UI nvarchar(10),
	@USER nvarchar(50),
	@DEPARTMENT nvarchar(50),
	@FILETYPE nvarchar(50),
	@FUNDTYPE nvarchar(50),
	@STARTDATE datetime,
	@ENDDATE datetime',
	@UI,
	@USER,
	@DEPARTMENT,
	@FILETYPE,
	@FUNDTYPE,
	@STARTDATE,
	@ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepDocPrec] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepDocPrec] TO [OMSAdminRole]
    AS [dbo];

