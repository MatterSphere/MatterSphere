
CREATE VIEW [external].[vwdbAssociates]
AS
	SELECT     Entity.*
	FROM         
		config.dbAssociates AS Entity  JOIN
		[external].[vwdbContact] Access on Access.ContID = Entity.contID


GO
GRANT UPDATE
    ON OBJECT::[external].[vwdbAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbAssociates] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbAssociates] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[vwdbAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[vwdbAssociates] TO [OMSApplicationRole]
    AS [dbo];

