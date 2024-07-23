


--Moved to separate package to allow support for Matter Sphere
/*

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

GRANT DELETE ON SCHEMA::[relationship] TO [OMSApplicationRole]
GO
GRANT INSERT ON SCHEMA::[relationship] TO [OMSApplicationRole]
GO
GRANT SELECT ON SCHEMA::[relationship] TO [OMSApplicationRole]
GO


GRANT INSERT ON [dbo].[dbAssociates] TO [OMSApplicationRole]
GO
GRANT SELECT ON [dbo].[dbAssociates] TO [OMSApplicationRole]
GO
GRANT UPDATE ON [dbo].[dbAssociates] TO [OMSApplicationRole]
GO

GRANT INSERT ON [dbo].[dbClient] TO [OMSApplicationRole]
GO
GRANT SELECT ON [dbo].[dbClient] TO [OMSApplicationRole]
GO
GRANT UPDATE ON [dbo].[dbClient] TO [OMSApplicationRole]
GO

GRANT INSERT ON [dbo].[dbContact] TO [OMSApplicationRole]
GO
GRANT SELECT ON [dbo].[dbContact] TO [OMSApplicationRole]
GO
GRANT UPDATE ON [dbo].[dbContact] TO [OMSApplicationRole]
GO

GRANT INSERT ON [dbo].[dbDocument] TO [OMSApplicationRole]
GO
GRANT SELECT ON [dbo].[dbDocument] TO [OMSApplicationRole]
GO
GRANT UPDATE ON [dbo].[dbDocument] TO [OMSApplicationRole]
GO


GRANT INSERT ON [dbo].[dbDocumentPreview] TO [OMSApplicationRole]
GO
GRANT SELECT ON [dbo].[dbDocumentPreview] TO [OMSApplicationRole]
GO
GRANT UPDATE ON [dbo].[dbDocumentPreview] TO [OMSApplicationRole]
GO


GRANT INSERT ON [dbo].[dbFile] TO [OMSApplicationRole]
GO
GRANT SELECT ON [dbo].[dbFile] TO [OMSApplicationRole]
GO
GRANT UPDATE ON [dbo].[dbFile] TO [OMSApplicationRole]
GO


*/
