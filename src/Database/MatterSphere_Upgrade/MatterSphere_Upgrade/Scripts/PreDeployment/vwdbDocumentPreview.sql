IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[config].[vwdbDocumentPreview]'))
	DROP VIEW [config].[vwdbDocumentPreview]
GO


CREATE VIEW [config].[vwdbDocumentPreview]
AS

WITH [DocumentAllowDeny] ( [DocumentID], [Allow], [Deny], [Secure]) AS
(
	SELECT 
			RUGD.[DocumentID],
			MAX(CASE WHEN Substring ( PC.AllowMask , 9 , 1 ) & 128 = 128 AND UGM.ID IS NOT NULL THEN 1 ELSE NULL END) as [Allow] ,
			MAX(CASE WHEN Substring ( PC.DenyMask , 9 , 1 ) & 128 = 128 AND UGM.ID IS NOT NULL THEN 1 ELSE NULL END) as [Deny] ,
			CASE WHEN MAX(CASE WHEN UGM.ID IS NOT NULL THEN 1 END) IS NULL THEN 1 END AS Secure
	FROM
			[relationship].[UserGroup_Document] RUGD
	 CROSS APPLY config.IsAdministratorTbl_NS() admins
	JOIN
			[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID]
	LEFT JOIN
			[config].[GetUserAndGroupMembershipNT_NS] () UGM ON RUGD.[UserGroupID] = UGM.[ID]
    WHERE admins.IsAdmin = 0 AND (PC.IsRemote = 0 or PC.IsRemote is Null)
	GROUP BY [DocumentID]
)

	SELECT D.*
	FROM config.dbDocumentPreview AS D
	LEFT OUTER JOIN (
		SELECT [DocumentID]
			, CASE WHEN [Allow] IS NULL AND [Deny] IS NULL THEN 1 ELSE [Deny] END AS [Deny] 
			, [Secure]
		FROM [DocumentAllowDeny]
		) AS DA ON D.DocID = DA.DocumentID	--JOIN / APPLY to table valued function or ADO.NET dynamic SQL fails in matter centre	
	WHERE (DA.[Deny] IS NULL) AND (DA.Secure IS NULL) 


GO
CREATE TRIGGER [config].[DocumentPreviewDelete]
    ON [config].[vwdbDocumentPreview]
    WITH ENCRYPTION
    INSTEAD OF DELETE
    NOT FOR REPLICATION
    AS 
BEGIN
--The script body was encrypted and cannot be reproduced here.
    RETURN
END


GO
GRANT UPDATE
    ON OBJECT::[config].[vwdbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbDocumentPreview] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbDocumentPreview] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[vwdbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[vwdbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];

