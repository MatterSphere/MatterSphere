
IF EXISTS (SELECT * FROM sys.triggers WHERE Name = 'NEW_MS_USER' AND type = 'TR') 
BEGIN
	DROP TRIGGER NEW_MS_USER  ON DATABASE
END

GO
CREATE TRIGGER [NEW_MS_USER]
ON DATABASE
AFTER ADD_ROLE_MEMBER
AS

set nocount on;
DECLARE @SQL table (SQLTXT nvarchar(200));
DECLARE @DSQL nvarchar(max)

INSERT INTO @SQL
SELECT distinct U.loginname
FROM SYS.database_principals P JOIN 
sys.database_role_members R ON P.principal_id=R.member_principal_id OR P.principal_id=R.role_principal_id AND type<>'R'
JOIN sys.syslogins U ON u.sid = p.sid 
WHERE EXISTS (SELECT 1
			FROM SYS.database_principals RP
					 join sys.database_role_members RR
			on RP.principal_id=RR.member_principal_id OR RP.principal_id=RR.role_principal_id AND type='R'
					where Rp.name in ('OMSROLE','OMSAdminRole','OMSApplicationRole') AND R.role_principal_id = RR.role_principal_id )
and not exists(SELECT 1
					FROM sys.server_permissions pe
					JOIN sys.server_principals pr
						ON pe.grantee_principal_id = pr.principal_Id
					JOIN sys.server_principals pr2
						ON pe.grantor_principal_id = pr2.principal_Id
					WHERE pe.type = 'IM'
					AND pr2.name = 'MSAUDITOR'
					and pr.name = U.loginname)
and u.loginname <> 'MSAUDITOR';


DECLARE @P NVARCHAR(100),@TEXT NVARCHAR(MAX)='USE [master] ; '
DECLARE @APPUSER NVARCHAR(100)='MSAUDITOR'

DECLARE C CURSOR FOR SELECT sqltxt FROM @SQL
OPEN C
FETCH NEXT FROM C INTO @P
WHILE(@@FETCH_STATUS=0)
	BEGIN
		SET @TEXT+='GRANT IMPERSONATE ON LOGIN::'+QUOTENAME(@APPUSER)+' TO '+QUOTENAME(@P)+';' +char(10)+char(13)
		FETCH NEXT FROM C INTO @P
	END
CLOSE C
DEALLOCATE C

EXEC sp_executesql @TEXT

SET @TEXT = 'USE [' + DB_NAME()+'_Audit];'
DECLARE C CURSOR FOR SELECT sqltxt FROM @SQL
OPEN C
FETCH NEXT FROM C INTO @P
WHILE(@@FETCH_STATUS=0)
	BEGIN
		SET @TEXT+='IF NOT EXISTS (SELECT 1 FROM SYS.database_principals u where u.name = '''+@P+''') BEGIN '
		SET @TEXT+='CREATE USER '+QUOTENAME(@P)+' FOR LOGIN '+QUOTENAME(@P)+';' +char(10)+char(13)
		SET @TEXT+='EXEC sp_addrolemember N''db_datareader'', N'''+@P+''';' +char(10)+char(13)
		SET @TEXT+='EXEC sp_addrolemember N''db_denydatawriter'', N'''+@P+''';' +char(10)+char(13)
		SET @TEXT+='END' +char(10)+char(13)
		FETCH NEXT FROM C INTO @P
	END
CLOSE C
DEALLOCATE C

EXEC sp_executesql @TEXT

GO
