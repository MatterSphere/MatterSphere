CREATE PROCEDURE [dbo].[MatterList]
(
@PAGE_NUM INT = NULL
,@PAGE_SIZE INT = NULL
,@SEARCH_TEXT NVARCHAR(255) = NULL
,@LANGUAGE uUICultureInfo = NULL
,@CLID BIGINT = NULL
,@FEEID INT = NULL
,@FILEDEPT uCodeLookup = NULL
,@FILETYPE uCodeLookup = NULL
,@FILESTATUS uCodeLookup = NULL
,@CREATED_FROM DATE = NULL
,@CREATED_TO DATE = NULL
,@CLIENTNAME NVARCHAR(130) = NULL
)
AS
SET @SEARCH_TEXT = COALESCE(@SEARCH_TEXT, '')
SET @PAGE_NUM = COALESCE(@PAGE_NUM, 1)
SET @PAGE_SIZE = COALESCE(@PAGE_SIZE, (SELECT COUNT(fileid) FROM [DBO].[DBFILE]))
DECLARE @SEARCH_START_OR_ENDS nvarchar(260)
DECLARE @PAGE_ROW_FROM int
DECLARE @PAGE_ROW_TO int
SET @PAGE_ROW_FROM = ((@PAGE_NUM - 1) * (@PAGE_SIZE) + 1)
SET @PAGE_ROW_TO = (@PAGE_NUM * @PAGE_SIZE)
SET @SEARCH_START_OR_ENDS = '%' + @SEARCH_TEXT + '%'
SET @CLIENTNAME = '%' + NULLIF(@CLIENTNAME, '') + '%'
SET @CREATED_TO = DATEADD(DAY, 1, @CREATED_TO)
;
WITH [Data] AS
(
SELECT
				ROW_NUMBER() OVER (ORDER BY F.Updated DESC) AS [RowNum]
				,C.clNo + '/' + F.fileNo as [ClientFileNo]
				,F.fileID as [ID]
				,F.fileNo as [Number] 
				,F.fileDesc as [Description] 
				,F.fileType as [TypeCode]
				,FTR.cdDesc as [TypeDescription]	
				,F.fileStatus as [Status]	
				,F.Updated
				,F.Created		
				,F.clID as [ClientID]
				,C.clNo as [ClientNumber]
				,C.clName as [ClientName]			
				,CONT.contName as [ContactName]
				,A.addLine1 as [AddressLine1]
				,A.addLine2 as [AddressLine2]
				,A.addLine3 as [AddressLine3]
				,A.addLine4 as [AddressLine4]
				,A.addLine5 as [AddressLine5]
				,A.addPostCode as [AddressPostCode]
				,U1.usrFullName as [PrincipleName]
				,U2.usrFullName as [UpdatedByName]
				,U3.usrFullName as [CreatedByName]
				,FT.typeGlyph as [Glyph]
				,AA.assocID as [AccountAssociateID]
				,AA.AssociateReference as [Reference]
				,FST.cdDesc as [StatusDescription]
				
	FROM [DBO].[DBFILE] F
	INNER JOIN [DBO].[DBCLIENT] C ON C.clID = F.clID
	INNER JOIN [DBO].[DBCONTACT] CONT ON C.clDefaultContact = CONT.contID
	INNER JOIN dbAddress A ON A.addID = CONT.contDefaultAddress 
	INNER JOIN dbUser U1 ON U1.usrID = F.filePrincipleID
	INNER JOIN dbFileType FT ON FT.typeCode = F.fileType
	LEFT JOIN dbUser U2 ON U2.usrID = F.UpdatedBy
	LEFT JOIN dbUser U3 ON U3.usrID = F.CreatedBy
	LEFT JOIN vwRemoteAccountAssociate AA ON AA.fileID = F.fileID
	LEFT JOIN dbo.GetCodeLookupDescription('FILETYPE', @LANGUAGE) FTR ON FTR.cdCode = F.FileType
	LEFT JOIN dbo.GetCodeLookupDescription('FILESTATUS', @LANGUAGE) FST ON FST.cdCode = F.fileStatus
	WHERE 
		
		(C.clNo + '/' + F.fileNo LIKE @SEARCH_START_OR_ENDS OR F.fileDesc LIKE @SEARCH_START_OR_ENDS)
		AND (@CLID is null OR @CLID = F.clID) 
		AND (@FEEID IS NULL OR @FEEID = F.filePrincipleID)
		AND (@FILEDEPT IS NULL OR @FILEDEPT = F.fileDepartment)
		AND (@FILETYPE IS NULL OR @FILETYPE = F.fileType)
		AND (@FILESTATUS IS NULL OR @FILESTATUS = F.fileStatus)
		AND (@CREATED_FROM IS NULL OR @CREATED_FROM <= F.Created)
		AND (@CREATED_TO IS NULL OR @CREATED_TO > F.Created)
		AND (@CLIENTNAME IS NULL OR C.clName LIKE @CLIENTNAME)
)
SELECT * FROM [Data]
WHERE [RowNum]  BETWEEN @PAGE_ROW_FROM AND @PAGE_ROW_TO
ORDER BY Created DESC