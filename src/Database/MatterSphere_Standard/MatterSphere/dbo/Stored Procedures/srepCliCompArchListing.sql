

CREATE PROCEDURE [dbo].[srepCliCompArchListing]
(
	@UI uUICultureInfo='{default}',
	@ARCHIVETYPE nvarchar(50) = null,
	@LOCATION nvarchar(50) = null,
	@AUTHBY bigint = null,
	@FEEEARNER nvarchar(50) = null
)

AS 

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

declare @SQL nvarchar(4000)
declare @SELECT nvarchar(4000)
declare @WHERE nvarchar(2000)
declare @ORDERBY nvarchar(100)

--- Select Statement for the base Query
set @SELECT = N'
SELECT   
	X.archID,  
	X.clNo, 
	X.clName, 
	X.archRef, 
	X.archDesc,
	X.archInStorage,
	X.Created, 
	X.usrInits AS AuthInits, 
	X.ArchTypeDesc,
	X.archType,
	X.ArchLocDesc,
	X.archStorageLoc,
	X.archNote,
	X.contSurname,
	X.archActive,
	X.archAuthBy,
	B.checkedOutBy as CheckedOutTo ,
	B.IssuedTo
FROM
	(
	SELECT   
		A.archID,  
		CL.clNo, 
		CL.clName, 
		A.archRef, 
		A.archDesc,
		A.archInStorage,
		A.Created, 
		U.usrInits,
		GCLD1.cdDesc AS ArchTypeDesc,
		GCLD2.cdDesc AS ArchLocDesc,
		CASE
			WHEN A.archNote > '''' THEN ''Notes: '' + A.archNote
		END AS archNote,
		CI.contSurname,
		A.archType,
		A.archStorageLoc,
		A.archActive,
		A.archAuthBy
	FROM         
		dbo.dbClient CL
	INNER JOIN
		dbo.dbArchive A ON CL.clID = A.clID 
	LEFT OUTER JOIN
		dbo.dbUser U ON A.archAuthBy = U.usrID
	LEFT OUTER JOIN
		dbo.dbContactIndividual CI ON CL.clDefaultContact = CI.contID
	LEFT JOIN
		dbo.GetCodeLookupDescription (N''ARCHTYPE'' , @UI ) GCLD1 ON GCLD1.cdCode = A.archType
	LEFT JOIN
		dbo.GetCodeLookupDescription (N''LOCTYPE'' , @UI ) GCLD2 ON GCLD2.cdCode = A.archStorageLoc
	) X 
LEFT OUTER JOIN
	(
	SELECT
		T1.logID,
		U1.usrFullName AS CheckedOutBy,
		C.contName AS IssuedTo,
		U1.usrID
	FROM
		dbTracking T1 
	INNER JOIN
		dbArchive A1 ON A1.archID = T1.logID
	INNER JOIN
		dbUser U1 on U1.usrID = T1.trackCheckedOutBy
	LEFT OUTER JOIN
		dbContact C ON C.contID = T1.trackIssuedTo
	WHERE
		T1.trackCheckedIn IS NULL AND
		T1.trackCheckedOut IS NOT NULL
	GROUP BY
		T1.logID,
		U1.usrFullName,
		C.contName,
		U1.usrID 
	) B ON X.archID = B.logID '

---- Debug Print
--- PRINT @SELECT

---- Build the Where Clause
SET @WHERE = ' WHERE X.archActive = 1 '

--- archiveType parm
IF (@ARCHIVETYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND X.archType = @ARCHIVETYPE '
END

--- LOCTION parm
IF (@LOCATION IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND X.archStorageLoc = @LOCATION '
END

--- AUTHBY parm
IF (@AUTHBY IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND X.archAuthBy = @AUTHBY '
END

--- FEEEARNER parm
IF (@FEEEARNER IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND B.usrID = @FEEEARNER '
END

--- Order by clause
SET @ORDERBY = N'
ORDER BY
	CASE
		WHEN X.contSurname IS NOT NULL THEN X.contSurname
		ELSE X.clName
	END ASC ' 

--- Add Statements together
set @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

--- Debug Print
-- PRINT @SQL

exec sp_executesql @sql,
 N'
	@UI nvarchar(10)
	, @ARCHIVETYPE nvarchar(50)
	, @LOCATION nvarchar(50)
	, @AUTHBY bigint
	, @FEEEARNER nvarchar(50)'
	, @UI
	, @ARCHIVETYPE
	, @LOCATION
	, @AUTHBY
	, @FEEEARNER

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliCompArchListing] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliCompArchListing] TO [OMSAdminRole]
    AS [dbo];

