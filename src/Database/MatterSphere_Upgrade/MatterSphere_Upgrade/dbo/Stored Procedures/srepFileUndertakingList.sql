CREATE PROCEDURE [dbo].[srepFileUndertakingList]
(
	@UI uUICultureInfo = '{default}'
	, @FILEID bigint = NULL
)

AS 

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

SELECT     
	CL.clNo
	, F.fileNo
	, COALESCE(U.undRef, '') as undRef
	, U.undDesc
	, U.undCompleted
	, U.undEstCompletion
	, COALESCE(CL1.cdDesc, '~' + NULLIF(U.undType, '') + '~') as undType
	, CL.clName
	, UAUTH.usrInits AS AuthByInits
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUndertakings U ON F.fileID = U.fileID
INNER JOIN
	dbo.dbUser UAUTH ON U.undAuthBy = UAUTH.usrID
INNER JOIN [dbo].[GetCodeLookupDescription]('UNDERTYPE', @UI) cl1 ON cl1.cdCode = U.undType
WHERE
	U.undActive = 1 AND
	F.fileID = @FILEID
