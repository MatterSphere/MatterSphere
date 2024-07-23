

CREATE PROCEDURE [dbo].[sprFieldTypes] (@UI uUICultureInfo = '{default}') 
AS

--COMMON
exec sprFieldList '.', @UI
--MACROS
exec sprFieldList '#', @UI
--REFLECTED_MACROS
exec sprFieldList '~', @UI
--REGINFO
exec sprFieldList 'REG', @UI
exec sprFieldList 'BR', @UI
exec sprFieldList 'USR', @UI
exec sprFieldList 'FEE', @UI
exec sprFieldList 'CONT', @UI
exec sprFieldList 'COMP', @UI
exec sprFieldList 'IND', @UI
exec sprFieldList 'CL', @UI
exec sprFieldList 'CAD', @UI
exec sprFieldList 'FILE', @UI
exec sprFieldList 'FAD', @UI
exec sprFieldList 'ASSOC', @UI
exec sprFieldList '@@', @UI
exec sprFieldList '$$', @UI
exec sprFieldList 'APP', @UI
exec sprFieldList 'DATETIME', @UI
exec sprFieldList 'LOOKUP', @UI
exec sprFieldList 'PH', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFieldTypes] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFieldTypes] TO [OMSAdminRole]
    AS [dbo];

