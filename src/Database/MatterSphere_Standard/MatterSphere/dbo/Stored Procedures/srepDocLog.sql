

CREATE PROCEDURE [dbo].[srepDocLog]
(
	@UI uUICultureInfo='{default}'
	, @CREATEDBY uCreatedBy = NULL
	, @DOCTYPE uCodeLookup = NULL
	, @DIRECTION bit = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS 

DECLARE @SELECT nvarchar(1900)
DECLARE @WHERE nvarchar(2000)
DECLARE @ORDERBY nvarchar(100)


--- BUILD THE SELECT CLAUSE
SET @SELECT = N' 
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	CASE
		WHEN DOC.docAuthored IS NOT NULL THEN DOC.docAuthored
		ELSE DOC.Created     
	END AS Created
	, DOC.Createdby
	, CL.clNo
	, F.fileNo
	, U.usrInits
	, DOC.docType
    , DOC.docDirection
	, CL.clName
	, U.usrFullName
	, C.contName
	, DOC.docID
FROM
	dbo.dbDocument DOC
INNER JOIN
	dbo.dbFile F ON DOC.fileID = F.fileID 
INNER JOIN
	dbo.dbClient CL ON F.clID = CL.clID         
INNER JOIN
	dbo.dbAssociates ASS ON DOC.assocID = ASS.assocID 
INNER JOIN
	dbo.dbContact C ON ASS.contID = C.contID
INNER JOIN
	dbo.dbUser U ON DOC.Createdby = U.usrID '

--- SET WHERE CLAUSE
SET @WHERE = ''

--- DOCUMENT CREATED START DATE AND END DATE
IF(@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + '
		CASE
			WHEN DOC.docAuthored IS NOT NULL THEN DOC.docAuthored
			ELSE DOC.Created
		END BETWEEN @STARTDATE AND DateAdd(ss,-1,@ENDDATE) '
END

--- DOCUMENT CREATED BY CLAUSE
IF(@CREATEDBY IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND DOC.CreatedBy = @CREATEDBY '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' DOC.CreatedBy = @CREATEDBY '
	END
END

--- DOCUMENT TYPE CLAUSE
IF(@DOCTYPE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND DOC.docType = @DOCTYPE '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' DOC.docType = @DOCTYPE '
	END
END

--- DOCUMENT DIRECTION CLAUSE
IF(@DIRECTION IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND DOC.docDirection = @DIRECTION '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' DOC.docDirection = @DIRECTION '
	END
END

--- BUILD WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

--- SET ORDER BY CLAUSE
SET @ORDERBY = N' 
ORDER BY 
	Created '

DECLARE @SQL nvarchar(4000)
--- ADD CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

--- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @CREATEDBY uCreatedBy
	, @DOCTYPE uCodeLookup
	, @DIRECTION bit
	, @STARTDATE datetime
	, @ENDDATE datetime'
	, @UI
	, @CREATEDBY
	, @DOCTYPE
	, @DIRECTION
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepDocLog] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepDocLog] TO [OMSAdminRole]
    AS [dbo];

