

	CREATE VIEW [external].[vwdbDocument]
	AS
	SELECT     Entity.*
	FROM         
		config.dbDocument AS Entity JOIN 
		[external].[DocumentAccess]() Access on Access.DocumentID = Entity.docID
	WHERE 
		Entity.SecurityOptions & 2 = 2 


GO
GRANT UPDATE
    ON OBJECT::[external].[vwdbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbDocument] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbDocument] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[vwdbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[vwdbDocument] TO [OMSApplicationRole]
    AS [dbo];

