SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DECLARE @BASECHEMATABLENAME NVARCHAR(MAX)
	, @SQL NVARCHAR(MAX)


IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbFile')
BEGIN
	SET @BASECHEMATABLENAME = N'config'
	SET @SQL = N'CREATE TRIGGER [config].[tgrFileNumberGenerator] ON [config].[dbFile]'
END
ELSE 
BEGIN
	SET @BASECHEMATABLENAME = N'dbo'
	SET @SQL = N'CREATE TRIGGER [dbo].[tgrFileNumberGenerator] ON [dbo].[dbFile]'
END

IF EXISTS(SELECT * from sys.triggers WHERE name = 'tgrFileNumberGenerator')
BEGIN
	IF @BASECHEMATABLENAME = N'config'
		DROP TRIGGER config.tgrFileNumberGenerator
	ELSE
		DROP TRIGGER dbo.tgrFileNumberGenerator
END

SET @SQL = @SQL + N'
FOR INSERT  NOT FOR REPLICATION
AS
declare @clid bigint
declare @fileid bigint
declare @filetype uCodeLookup
declare @number nvarchar(20)
declare @branch int
declare @usrid int
--set @fileid = scope_identity()
select @fileid = fileid, @clid = clid, @number = fileno, @filetype = filetype, @usrid = createdby , @branch = brID from inserted --where fileid = @fileid

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
	declare @newnum nvarchar(20)
	declare @lastnum nvarchar(20)
	declare @seed uCodeLookup
	declare @ret int
	declare @ctr int
	
	set @seed = IsNull((select typeseed from dbfiletype where typecode = @filetype), ''FI'')

	-- *** NOTE ***
	--The file record has already been written so must filter the file id that has just been added.
	set @lastnum = IsNull((select top 1 fileno from config.dbFile where clid = @clid and fileid <> @fileid order by created desc), ''N/A'')
		
	execute @ret = sprGetNextSeedNo @branch, @seed, @lastnum, @newnum output, @fileid
	if @ret = 0
	begin
		set @ctr = 0
		while exists(select fileid from config.dbFile where clid = @clid and fileno = @newnum)
		begin
			execute @ret = sprGetNextSeedNo @branch, @seed, @newnum, @newnum output
			if @ret <> 0 break
			set @ctr = @ctr + 1
			if @ctr > 1000 break
		end
		update config.dbFile set fileno = @newnum where fileid = @fileid
	end
	else
	begin
		declare @msg nvarchar(500)
		declare @severity tinyint
		declare @UI uUICultureInfo
		set @UI  = (select usruicultureinfo from dbuser where usrid = @usrid)
		execute @severity = sprRaiseError ''MSGFILESEEDNO'', @UI, @msg out
		raiserror (@msg, @severity, 1, @seed)
		rollback transaction
	end
end
'

EXEC sp_executesql @SQL

