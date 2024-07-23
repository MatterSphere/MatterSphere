SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DECLARE @BASECHEMATABLENAME NVARCHAR(MAX)
	, @SQL NVARCHAR(MAX)


IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbClient')
BEGIN
	SET @BASECHEMATABLENAME = N'config'
	SET @SQL = N'CREATE TRIGGER [config].[tgrClientNumberGenerator] ON [config].[dbClient]'
END
ELSE 
BEGIN
	SET @BASECHEMATABLENAME = N'dbo'
	SET @SQL = N'CREATE TRIGGER [dbo].[tgrClientNumberGenerator] ON [dbo].[dbClient]'
END

IF EXISTS(SELECT * from sys.triggers WHERE name = 'tgrClientNumberGenerator')
BEGIN
	IF @BASECHEMATABLENAME = N'config'
		DROP TRIGGER config.tgrClientNumberGenerator
	ELSE
		DROP TRIGGER dbo.tgrClientNumberGenerator
END

SET @SQL = @SQL + N'
FOR INSERT  NOT FOR REPLICATION
AS
declare @clid bigint
declare @cltype uCodeLookup
declare @number nvarchar(12)
declare @branch int
declare @usrid int
select @clid = clid , @number = clno, @cltype = cltypecode, @usrid = createdby , @branch = brID from inserted 


-- If the branchID is specifically set i.e. the value is not 0 or -1 set the branchID to equal that value
-- =========================================================
IF (SELECT TOP 1 regBranchConfig FROM dbRegInfo)  > 0
	SET @branch = ( SELECT TOP 1 regBranchConfig FROM dbRegInfo R JOIN dbBranch B ON B.brID = R.regBranchConfig )

-- If using site specfic database (value 0)
-- ======================
IF (SELECT TOP 1 regBranchConfig FROM dbRegInfo) = 0 OR @branch IS NULL
	set @branch = (select top 1 brid from dbreginfo)


if @number is null or @number = ''''
begin
 declare @newnum nvarchar(12)
 declare @seed uCodeLookup
 declare @ret int
 
 set @seed = IsNull((select typeseed from dbclienttype where typecode = @cltype), ''CL'')
 execute @ret = sprGetNextSeedNo @branch, @seed, null, @newnum output, @clid
 if @ret = 0
 BEGIN
	WHILE (EXISTS (SELECT 1 FROM config.dbClient WHERE clNo = @newnum))
	BEGIN
		execute @ret = sprGetNextSeedNo @branch, @seed, null, @newnum output, @clid
	END
	update config.dbClient set clno = @newnum where clid = @clid
 END
 else
 begin
  declare @msg nvarchar(500)
  declare @severity tinyint
  declare @UI uUICultureInfo
  set @UI  = (select usruicultureinfo from dbuser where usrid = @usrid)
  execute @severity = sprRaiseError ''MSGCLSEEDNO'', @UI, @msg out
  raiserror (@msg, @severity, 1, @seed)
  rollback transaction
 end
end
'

EXEC sp_executesql @SQL

