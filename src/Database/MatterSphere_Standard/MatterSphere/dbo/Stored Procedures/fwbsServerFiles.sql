

CREATE PROCEDURE [dbo].[fwbsServerFiles]

AS 

SET NOCOUNT ON


DECLARE @pagesperMB decimal (15) , @bytesperpage decimal(15) , @cmd nvarchar(1024)

IF EXISTS (SELECT * FROM tempdb..sysobjects
 WHERE id = object_id(N'[tempdb]..[#logSpace]'))
DROP TABLE #logSpace

CREATE TABLE #logSpace
	(
	Databasename nvarchar(100) , 
	LogSize real ,
	LogSpaceUsed real , 
	Status int
	)
	
SELECT @cmd = 'dbcc sqlperf (logspace)'
INSERT INTO #logSpace EXECUTE (@cmd)

SET NOCOUNT ON

SELECT @bytesperpage = low
FROM master.dbo.spt_values
WHERE number = 1 AND type = 'E'
SET @pagesperMB = 1048576 / @bytesperpage


SELECT
	 [name]
	, [filename]
	, filegroup_name ( [groupid] ) as filegroup
	, convert(nvarchar(15),convert(decimal(8,1), (([size] * 8)/1024))) + 'Mb' as Filesize
	, case when filegroup_name (groupid) is not null then ( select convert ( nvarchar (20) ,  convert ( decimal (15,2) ,  sum ( used)/ @pagesperMB)) + 'Mb' from sysindexes where groupid = sysfiles.groupid) 
		else (select  convert ( nvarchar (20) , convert (decimal (15,2) , LogSize *  LogSpaceUsed/100 )) + 'Mb' From #logSpace where Databasename = db_name() ) end as Used
	, case [maxsize]
		when 0 then 'Fixed growth'
		when -1 then 'Limited to disk size'
	  end as 'Max size'
	, convert(nvarchar(10),[growth]) + '%' as 'File growth' 
FROM sysfiles 


ORDER BY filegroup

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsServerFiles] TO [OMSAdminRole]
    AS [dbo];

