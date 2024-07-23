CREATE SYNONYM [external].[dbDocumentPreview] FOR [external].[vwdbDocumentPreview];




GO
GRANT UPDATE
    ON OBJECT::[external].[dbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[dbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[dbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[dbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];

