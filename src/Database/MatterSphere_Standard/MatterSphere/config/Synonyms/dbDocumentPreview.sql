CREATE SYNONYM [config].[dbDocumentPreview] FOR [dbo].[dbDocumentPreview];




GO
GRANT UPDATE
    ON OBJECT::[config].[dbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[dbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[dbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[dbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];

