

CREATE VIEW [external].[vwdbDocumentPreview]
AS
	SELECT     Entity.*
	FROM         
		config.dbDocumentPreview AS Entity JOIN 
		[external].[DocumentAccess]() Access on Access.DocumentID = Entity.docID		


GO
GRANT UPDATE
    ON OBJECT::[external].[vwdbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbDocumentPreview] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbDocumentPreview] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[vwdbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[vwdbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];

