CREATE PROCEDURE dbo.SCHFILREMOTEACC
(
	@FILEID BIGINT 
	, @ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @SELECT NVARCHAR(MAX)

SET @SELECT = N'
SELECT  
	dbo.dbInteractiveFileProfile.ID as FILPROID, 
	dbo.dbInteractiveFileProfile.ID,
	dbo.dbInteractiveProfile.contID, 
	dbo.dbInteractiveProfile.proUserName, 
	dbo.dbInteractiveProfile.proEmail, 
	dbo.dbInteractiveFileProfile.proContact AS blnContact, 
	dbo.dbInteractiveFileProfile.proActionable AS blnEnquiry, 
	dbo.dbInteractiveFileProfile.proMilestones AS blnMilestone, 
	dbo.dbInteractiveFileProfile.proDocs AS blnDocuments, 
	dbo.dbInteractiveFileProfile.proSMS AS blnSMS, 
	dbo.dbInteractiveFileProfile.proNotes as blnNotes,
	dbo.dbInteractiveFileProfile.fileid
FROM dbo.dbInteractiveFileProfile 
	INNER JOIN dbo.dbInteractiveProfile ON dbo.dbInteractiveProfile.contID = dbo.dbInteractiveFileProfile.contID
WHERE fileid = @fileid
'

IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY FILPROID'
ELSE 
	IF @ORDERBY NOT LIKE '%FILPROID%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', FILPROID'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@FILEID BIGINT', @FILEID

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILREMOTEACC] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILREMOTEACC] TO [OMSAdminRole]
    AS [dbo];
