

CREATE PROCEDURE [dbo].[sprSystemInfo] AS
declare @brid int
set @brid = (select top 1 brid from dbreginfo)
--REGINFO
select top 1 * from dbreginfo
--BRANCH
select * from dbbranch where brid = @brid
--CAPTAINSLOG
select top 0 * from dbcaptainslog
--CAPTAINSLOGTYPE
select * from dbcaptainslogtype
-- DIRECTORIES
select * from dbdirectory

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprSystemInfo] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprSystemInfo] TO [OMSAdminRole]
    AS [dbo];

