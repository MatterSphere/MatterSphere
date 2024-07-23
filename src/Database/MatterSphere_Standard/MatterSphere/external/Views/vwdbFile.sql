
CREATE VIEW [external].[vwdbFile]
AS
		SELECT     Entity.*
		FROM         
		config.dbFile AS Entity JOIN 
		[external].[FileAccess] ( ) Access on Access.fileID = Entity.fileID
		--WHERE 
		--Entity.SecurityOptions & 2 = 2

GO
GRANT UPDATE
    ON OBJECT::[external].[vwdbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbFile] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbFile] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[vwdbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[vwdbFile] TO [OMSApplicationRole]
    AS [dbo];

