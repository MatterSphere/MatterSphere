DENY SELECT ON [config].[dbClient] TO OMSApplicationRole
GO

DENY SELECT ON [config].[dbFile] TO OMSApplicationRole
GO

DENY SELECT ON [config].[dbDocument] TO OMSApplicationRole
GO

DENY SELECT ON [config].[dbDocumentPreview] TO OMSApplicationRole
GO

DENY SELECT ON [config].[dbAssociates] TO OMSApplicationRole
GO

DENY SELECT ON [config].[dbContact] TO OMSApplicationRole
GO


GRANT DELETE ON SCHEMA::[config] TO [OMSApplicationRole]
GO
GRANT INSERT ON SCHEMA::[config] TO [OMSApplicationRole]
GO
GRANT SELECT ON SCHEMA::[config] TO [OMSApplicationRole]
GO

GRANT DELETE ON SCHEMA::[item] TO [OMSApplicationRole]
GO
GRANT INSERT ON SCHEMA::[item] TO [OMSApplicationRole]
GO
GRANT SELECT ON SCHEMA::[item] TO [OMSApplicationRole]
GO

GRANT DELETE ON SCHEMA::[dbo] TO [OMSApplicationRole]
GO
GRANT INSERT ON SCHEMA::[dbo] TO [OMSApplicationRole]
GO
GRANT SELECT ON SCHEMA::[dbo] TO [OMSApplicationRole]
GO