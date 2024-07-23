

CREATE PROCEDURE [dbo].[srepPrvNoBand3]

AS 
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT     
	F.FileBanding, 
	F.fileFundCode,
	CL.clNo + '/'+ F.fileNo AS Ref, 
	CL.clName, 
	U.usrInits, 
	F.fileDesc, 
	U.usrFullName
FROM         
	dbo.dbFundType FT
INNER JOIN
	dbo.dbFile F ON FT.ftCode = F.fileFundCode AND FT.ftCurISOCode = F.fileCurISOCode
INNER JOIN
	dbo.dbClient CL ON F.clID = CL.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
WHERE     
	F.FileBanding <> 3
AND
	F.fileStatus LIKE '%LIVE%'
AND
	FT.FTLegalAidCharged = 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPrvNoBand3] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPrvNoBand3] TO [OMSAdminRole]
    AS [dbo];

