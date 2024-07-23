

CREATE PROCEDURE [dbo].[sprExtendedDataList]
(@UI uUICultureInfo = '{default}')
AS
	SELECT dbo.dbExtendedData.extCode, COALESCE(CL1.cdDesc, '~' + NULLIF(dbExtendedData.extCode, '') + '~') AS cddesc
FROM dbo.dbExtendedData
LEFT JOIN dbo.GetCodeLookupDescription ( 'EXTENDEDDATA', @UI ) CL1 ON CL1.[cdCode] =  dbExtendedData.extCode

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprExtendedDataList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprExtendedDataList] TO [OMSAdminRole]
    AS [dbo];

