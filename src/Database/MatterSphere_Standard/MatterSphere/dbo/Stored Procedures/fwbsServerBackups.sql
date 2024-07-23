

CREATE PROCEDURE [dbo].[fwbsServerBackups]
AS

SELECT 
	database_name ,start_time , end_time , 
	case succeeded when 0 then 14 else 0 end as succeeded , 
	replace (substring ( message , charindex ( '[' , message) + 1 ,   200) , ']' , '') as message , 
	duration   
FROM 
	msdb..sysdbmaintplan_history
WHERE 
	start_time > getutcdate() - 3 and database_name <> '' and activity = 'Backup database'


UNION ALL

SELECT 
	database_name + ' log' as database_name ,start_time , end_time , 
	case succeeded when 0 then 14 else 0 end as succeeded , 
	replace (substring ( message , charindex ( '[' , message) + 1 ,   200) , ']' , '') as message , 
	duration   
FROM 
	msdb..sysdbmaintplan_history
WHERE 
	start_time > getutcdate() - 3 and database_name <> '' and activity = 'Backup transaction log'
ORDER BY 
	database_name , start_time desc

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsServerBackups] TO [OMSAdminRole]
    AS [dbo];

