

CREATE PROCEDURE [config].[CheckAccess] ( @USER nvarchar(200),@TYPE nvarchar(200),@ID bigint  )

AS

begin

DECLARE @SAMSG nvarchar(max)
DECLARE @MSG nvarchar(max)
DECLARE @UMSG nvarchar(max)
DECLARE @TMSG nvarchar(max)

DECLARE @WORKTABLE AS TABLE
( VDENY smallint null,
  VALLOW smallint null,
  UDENY smallint null,
  UALLOW smallint null, 
  SECURE smallint null,
  USERGROUP Nvarchar(200) null)
  
DECLARE @WORKSECADMIN AS TABLE
( [DENY] smallint null,
  ALLOW smallint null,
  USERGROUP Nvarchar(200) null)


INSERT INTO @WORKSECADMIN
SELECT [DENY],[ALLOW],[Name] from config.CheckIsAdministrator (@User) a;

IF EXISTS (select 1 from @WORKSECADMIN where Allow = 1)
begin
	If Not Exists (select 1 from @WORKSECADMIN where [DENY] = 1)
	begin
		select @SAMSG='Security Admin User set'+ CHAR(13)+CHAR(10) 
		select @SAMSG
		Return
	end
	select top 1 @SAMSG='Security Admin User set but denied by '+ Usergroup + CHAR(13)+CHAR(10)  from @WORKSECADMIN where [Deny] = 1
end

IF @TYPE = 'Contact'
begin
	INSERT INTO @WORKTABLE
	SELECT VDENY,VALLOW,UDENY,UALLOW,SECURE,USERGROUP from config.CheckContactAccess (@User,@ID) a;
end


IF @TYPE = 'Client'
begin
	INSERT INTO @WORKTABLE
	SELECT VDENY,VALLOW,UDENY,UALLOW,SECURE,USERGROUP from config.CheckClientAccess (@User,@ID) a1;
end


IF @TYPE = 'File'
begin
	INSERT INTO @WORKTABLE
	SELECT VDENY,VALLOW,UDENY,UALLOW,SECURE,USERGROUP from config.CheckFileAccess (@User,@ID) a;
end

IF @TYPE = 'Document'
begin
	
		INSERT INTO @WORKTABLE
		SELECT VDENY,VALLOW,UDENY,UALLOW,SECURE,USERGROUP from config.CheckDocumentAccess (@User,@ID) a;
 
 end

 

	IF NOT EXISTS (SELECT 1 FROM @WORKTABLE a)
	begin
		select isnull(@SAMSG,'')+'Implicit View and Update Permitted'
		return
	end
	IF EXISTS (SELECT 1 FROM @WORKTABLE a where isnull(SECURE,1) = 1)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM @WORKTABLE a where a.[VDENY] = 1 or a.[VALLOW] = 1 )
			select @MSG = 'Implicit View Denied'

		DECLARE  C1 CURSOR FOR SELECT  
		'Explicit View Denied to ' + a.UserGroup+ CHAR(13)+CHAR(10) 
		from @WORKTABLE a
		where a.[VDENY] = 1 
		OPEN C1;
		FETCH NEXT FROM C1 into @TMSG;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			select @MSG = isnull(@MSG,'')+@TMSG
			FETCH NEXT FROM C1 into @TMSG;
		END;
		CLOSE C1;
		DEALLOCATE C1;

		if @MSG is not null begin SELECT isnull(@SAMSG,'')+@MSG return end

		SELECT top 1
		@MSG =   'Explicit View ALLOWED '+ CHAR(13)+CHAR(10) 
		from @WORKTABLE a
		where a.[VALLOW] = 1
	end 
	else
		select @MSG = 'Implicit View Allowed '+ CHAR(13)+CHAR(10)
	
	IF NOT EXISTS (SELECT 1 FROM @WORKTABLE a where a.[UDENY] = 1 or a.[UALLOW] = 1  )
	select @UMSG = 'Implicit Update Denied'
	IF @UMSG is null
	begin
		DECLARE  C1 CURSOR FOR SELECT  
		'Explicit Update Denied '+ case when isnull(SECURE,0) = 2 then ' by System Policy for ' else ' to ' end + a.UserGroup+ + CHAR(13)+CHAR(10) 
		from @WORKTABLE a
		where a.[UDENY] = 1 
		OPEN C1;
		FETCH NEXT FROM C1 into @TMSG;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			select @UMSG = isnull(@UMSG,'')+@TMSG
			FETCH NEXT FROM C1 into @TMSG;
		END;
		CLOSE C1;
		DEALLOCATE C1;
		;
	end
	IF @UMSG is null
	begin
		SELECT  top 1
		@UMSG = 'Explicit Update ALLOWED '
		from @WORKTABLE a
		where a.[UALLOW] = 1 ;
	end
	select isnull(@SAMSG,'')+isnull(@MSG,'')+@UMSG
end


GO
GRANT EXECUTE
    ON OBJECT::[config].[CheckAccess] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[CheckAccess] TO [OMSAdminRole]
    AS [dbo];

