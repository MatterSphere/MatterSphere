

CREATE PROCEDURE [dbo].[sprCodeLookupCache](@UI nvarchar(10)= null)

AS
SET NOCOUNT ON
DECLARE @EDITION nvarchar(15)

SELECT TOP 1 @edition = regedition from dbo.dbreginfo

SELECT @EDITION as [type], '' as code, @UI as ui, 1 as brief, 0 as includenull
UNION ALL
SELECT 'RESOURCE', '', @UI, 1, 0
UNION ALL
SELECT 'MESSAGE', '', @UI, 1, 0
UNION ALL
SELECT 'USRROLES', '', @UI, 1, 0
UNION ALL
SELECT 'PACKAGE', '', @UI, 1, 0
UNION ALL
SELECT 'SLP', '', @UI, 1, 0
UNION ALL
SELECT 'SLBUTTON', '', @UI, 1, 0
UNION ALL
SELECT 'SLPBUTTON', '', @UI, 1, 0

 

EXEC sprcodelookuplist @EDITION, null, @UI
EXEC sprcodelookuplist 'RESOURCE', null, @UI
EXEC sprcodelookuplist 'MESSAGE', null, @UI
EXEC sprcodelookuplist 'USRROLES', null, @UI
EXEC sprcodelookuplist 'PACKAGE', null, @UI
EXEC sprcodelookuplist 'SLP', null, @UI
EXEC sprcodelookuplist 'SLBUTTON', null, @UI
EXEC sprcodelookuplist 'SLPBUTTON', null, @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCodeLookupCache] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCodeLookupCache] TO [OMSAdminRole]
    AS [dbo];

