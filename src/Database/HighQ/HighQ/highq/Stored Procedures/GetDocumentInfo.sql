CREATE PROCEDURE [highq].[GetDocumentInfo]
	@docId BIGINT
AS
	SELECT D.[clID], CL.[clNo], F.[fileNo], D.[docFileName], DR.[dirPath], D.[docDesc]
	FROM [dbDocument] D
	JOIN [dbo].[dbDirectory] DR ON D.[docdirID]=DR.[dirID]
	JOIN [dbo].[dbFile] F ON D.[fileID]=F.[fileID]
	JOIN [dbo].[dbClient] CL ON D.[clID]=CL.[clID]
	WHERE D.[docID] = @docId
