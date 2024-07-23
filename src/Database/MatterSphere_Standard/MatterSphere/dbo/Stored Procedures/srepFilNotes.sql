

CREATE PROCEDURE [dbo].[srepFilNotes] 
(
	@UI uUICultureInfo='{default}'
	, @FILEID bigint
)

AS 
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	CASE
		WHEN U.usrInits IS NULL THEN 'Unknown'
		ELSE U.usrInits
	END AS CreatedInits, 
	CASE
		WHEN U1.usrInits IS NULL THEN 'Unknown'
		ELSE U1.usrInits
	END AS UpdatedInits, 
	CASE
		WHEN U2.usrInits IS NULL THEN 'Unknown'
		ELSE U2.usrInits
	END AS FileHandler, 
	F.Created, 
	F.Updated, 
	CL.clNo, 
	F.fileNo, 
	CONVERT ( NVARCHAR ( MAX ) , F.fileNotes ) AS fileNotes, 
    CONVERT ( NVARCHAR ( MAX ) , F.fileExternalNotes ) AS fileExternalNotes,	
	F.fileDesc
FROM    
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
LEFT OUTER JOIN
    dbo.dbUser U1 ON F.Updatedby = U1.usrID 
LEFT OUTER JOIN
	dbo.dbUser U ON F.CreatedBy = U.usrID
LEFT OUTER JOIN
	dbo.dbUser U2 ON F.filePrincipleID = U2.usrID
WHERE
	F.fileID = @FILEID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilNotes] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilNotes] TO [OMSAdminRole]
    AS [dbo];

