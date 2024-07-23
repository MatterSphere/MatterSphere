IF EXISTS (SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID(N'[dbo].[schDocuSignEnvelopes]'))
	DROP PROCEDURE [dbo].[schDocuSignEnvelopes] 
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[schDocuSignEnvelopes]
(
	@UI uUICultureInfo = '{default}'
	, @fileID BIGINT
	, @MAX_RECORDS INT = 50
	, @PageNo INT = NULL
	, @ORDERBY NVARCHAR(100) = NULL
)
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @SELECT NVARCHAR(MAX)

SET @SELECT = N'
DECLARE @OFFSET INT = 0
	, @TOP INT
	, @Total INT
	
IF @MAX_RECORDS > 0
	SET @TOP = @MAX_RECORDS
ELSE
	SET @TOP = 50

IF @PageNo IS NULL
	SET @OFFSET = 0
ELSE
	SET @OFFSET = @TOP * (@PageNo - 1)

DECLARE @Res TABLE(
	Id BIGINT PRIMARY KEY
	, envID INT
);

WITH Res AS
(
SELECT
	E.envID
	, E.envSubject
	, E.Created
	, E.Updated
	, COALESCE(CL.cdDesc, ''~'' + NULLIF(E.envStatus, '''') + ''~'') AS envStatusDesc
	, UC.usrFullName AS CreatedByName
	, UU.usrFullName AS UpdatedByName
FROM dbo.dbDocuSignEnvelopes E
	LEFT JOIN dbo.GetCodeLookupDescription(''DCSGNENVLSTATUS'', @UI) CL ON CL.[cdCode] = E.envStatus
	LEFT JOIN dbo.dbUser UC ON UC.usrID = E.CreatedBy
	LEFT JOIN dbo.dbUser UU ON UU.usrID = E.UpdatedBy
WHERE E.fileID = @fileID
'

SET @SELECT = @SELECT + N'
)
INSERT INTO @Res (envID, Id)
SELECT 
	envID
'

IF @ORDERBY IS NULL
	SET @ORDERBY = 'envID DESC'

IF @ORDERBY LIKE 'envID%'
	SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N')'
ELSE
	SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N', envID DESC)'

SET @SELECT = @SELECT + N'
FROM Res
SET @Total = @@ROWCOUNT

SELECT TOP(@TOP)
	E.*
	, COALESCE(CL.cdDesc, ''~'' + NULLIF(E.envStatus, '''') + ''~'') AS envStatusDesc
	, UC.usrFullName AS CreatedByName
	, UU.usrFullName AS UpdatedByName
	, CASE envStatus 
		WHEN ''Voided'' THEN 21
		WHEN ''Created'' THEN 0
		WHEN ''Deleted'' THEN 81
		WHEN ''Sent'' THEN 34
		WHEN ''Delivered'' THEN 75
		WHEN ''Signed'' THEN 105
		WHEN ''Completed'' THEN 7
		WHEN ''Declined'' THEN 117
		WHEN ''TimedOut'' THEN 62
		WHEN ''Template'' THEN 19
		WHEN ''Processing'' THEN 22
		WHEN ''Downloaded'' THEN 2
		ELSE 84 
	  END AS Icon
	, @Total AS Total
FROM @RES res
	INNER JOIN dbo.dbDocuSignEnvelopes E ON E.envID = res.envID
	LEFT JOIN dbo.GetCodeLookupDescription(''DCSGNENVLSTATUS'', @UI) CL ON CL.[cdCode] = E.envStatus
	LEFT JOIN dbo.dbUser UC ON UC.usrID = E.CreatedBy
	LEFT JOIN dbo.dbUser UU ON UU.usrID = E.UpdatedBy
WHERE res.Id > @OFFSET 
ORDER BY res.Id
'

--PRINT @SELECT 

EXEC sp_executesql @SELECT,  N'@UI uUICultureInfo, @fileID BIGINT, @MAX_RECORDS INT, @PageNo INT',
	@UI, @fileID, @MAX_RECORDS, @PageNo

GO

GRANT EXECUTE
	ON OBJECT::[dbo].[schDocuSignEnvelopes] TO [OMSRole], [OMSAdminRole]
	AS [dbo];
GO
