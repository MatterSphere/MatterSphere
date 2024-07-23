

CREATE PROCEDURE [dbo].[fwbsServerProcesses]

AS

SELECT 
	sp.spid , sp.blocked , sp.waittime , lower (substring ( sp.lastwaittype ,  1 , len ( sp.lastwaittype) -1 )) as lastwaittype , sp.cpu , sp.physical_io , sp.[memusage] , sp.status , sd.[name] , sp.hostname  , sp.program_name , sp.loginame , sp.open_tran ,
	case  when sp.blocked  <> 0 then 14 when open_tran = 1 then 13 else 10 end
FROM 
	master..sysprocesses sp join master..sysdatabases sd on sp.dbid = sd.dbid

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsServerProcesses] TO [OMSAdminRole]
    AS [dbo];

