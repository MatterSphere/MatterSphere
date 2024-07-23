CREATE PROC [dbo].[GETAUDITHISTORY] 

	@AUDIT_TABLE_NAME sysname 
	, @ID nvarchar(max) 
	, @TYPE nvarchar(15) = ''
	, @USERID bigint = NULL
	, @FROM datetime = NULL
	, @TO datetime = NULL
	, @FIELDFILTER nvarchar(max) = ''
WITH EXECUTE AS OWNER
 AS

BEGIN
DECLARE @SOURCEDATABASE sysname,
		@SOURCESCHEMA sysname,
		@SOURCETABLE sysname,
		@AUDITDATABASE sysname,
		@AUDITSCHEMA sysname,
		@AUDITTABLE sysname,
		@SQL nvarchar(max),
		@Pos    INT,
		@OldPos INT,
		@keypos INT,
		@PRIMARY_KEY_SQL NVARCHAR ( 200 ),
		@VALUE nvarchar(200),
	    @AUDITID uniqueidentifier,
		@FIELDNAME sysname,
		@SPECIAL nvarchar(500),
		@NEWVALUE nvarchar(max),
		@SELVALUE nvarchar(Max),
		@FROM_STRING nvarchar(20) = ISNULL(Convert(nvarchar, @FROM), '1 Jan 1900'),
		@TO_STRING nvarchar(20) = ISNULL(Convert(nvarchar, @TO), '31 Dec 2100'),
		@OLDVALUE nvarchar(max)


SELECT @SOURCEDATABASE=left([acTableName],charindex('.',[acTableName])-1),
@SOURCESCHEMA=right(left([acTableName],charindex('.',[acTableName],charindex('.',[acTableName])+1)-1),
charindex('.',reverse(left([acTableName],charindex('.',[acTableName],charindex('.',[acTableName])+1)-1)))-1),
@SOURCETABLE=right([acTableName],charindex('.',REVERSE([acTableName]))-1),
@AUDITDATABASE=left([acAuditTableName],charindex('.',[acAuditTableName])-1),
@AUDITSCHEMA=right(left([acAuditTableName],charindex('.',[acAuditTableName],charindex('.',[acAuditTableName])+1)-1),
charindex('.',reverse(left([acAuditTableName],charindex('.',[acAuditTableName],charindex('.',[acAuditTableName])+1)-1)))-1),
@AUDITTABLE=right([acAuditTableName],charindex('.',REVERSE([acAuditTableName]))-1)
  FROM [audit].[Configuration] where right([acAuditTableName],charindex('.',REVERSE([acAuditTableName]))-1) = QUOTENAME(@AUDIT_TABLE_NAME)



SELECT b.COLUMN_NAME,
	b.ORDINAL_POSITION as key_pos into #T1
	FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE b 
			JOIN information_schema.table_constraints as u ON b.CONSTRAINT_NAME = u.CONSTRAINT_NAME and CONSTRAINT_TYPE = 'PRIMARY KEY'
	where QUOTENAME(b.TABLE_SCHEMA) =  @SOURCESCHEMA 
	and QUOTENAME(b.TABLE_NAME) = @SOURCETABLE
	order by b.TABLE_SCHEMA, b.TABLE_NAME, b.ORDINAL_POSITION


   SET  @ID  = @id + ','
   SET @PRIMARY_KEY_SQL = ''
    SELECT  @Pos    = 1,
            @OldPos = 1,
			@KEYPOS = 1

    WHILE   @Pos < LEN(@ID)
        BEGIN
            SELECT  @Pos = CHARINDEX(',', @ID, @OldPos)
            SELECT @PRIMARY_KEY_SQL= @PRIMARY_KEY_SQL+CASE WHEN @keypos = 1 then 'WHERE ' else ' AND ' end +COLUMN_NAME+' = '''+REPLACE(LTRIM(RTRIM(SUBSTRING(@ID, @OldPos, @Pos - @OldPos))),'''','''''')+'''' from #T1 where KEY_POS = @keypos

            SELECT  @OldPos = @Pos + 1
			SELECT @keypos = @keypos + 1
        END


DECLARE  @AuditMapping TABLE(
	[TableName] nvarchar(256),
	[AUDITID] [uniqueidentifier],
	[TYPE] [char](1),
	[FRIENDLYTYPE] [nvarchar](1000),
	[FIELDNAME] [nvarchar](1000),
	[OLDVALUE] [nvarchar](max) NULL,
	[NEWVALUE] [nvarchar](max) NULL,
	[UPDATEDATE] [datetime] NULL,
	[APPNAME] [nvarchar](max) NULL,
	[UserID] [bigint] NULL,
	[USERNAME] [nvarchar](200) NULL,
	[FRIENDLYNAME] [nvarchar](256) NULL,
	[LINKEDVALUE] [nvarchar](max) NULL,
	[SPECIAL] [nvarchar](max) NULL, 
	[MAPPEDOLDVALUE] [nvarchar](max) NULL)


SET @SQL = '
SELECT
	   ''[' + @AUDIT_TABLE_NAME + ']'' as [TableName] 
       , AT.AUDITID
       , AT.TYPE
	   , cl.cdDesc AS [FRIENDLYTYPE]
       , AT.FIELDNAME
       , Convert(nvarchar(max), AT.OLDVALUE) as [OLDVALUE]
		, Convert(nvarchar(max), AT.NEWVALUE) as [NEWVALUE]
       , AT.UPDATEDATE
       , AT.AppName
       , isnull(AT.UserID, -999) as [UserID]
       , isnull(u.usrFullName+ '' ('' + CASE WHEN u.[AccessType] = ''INTERNAL'' then  u.usrADID else u.[AccessType] end + '')'', AT.UserName)  as [USERNAME]
       , CASE 
			WHEN AT.FRIENDLYNAME = '''' then
											(CASE 
												WHEN CM.FRIENDLYNAME = '''' THEN Fieldname
												ELSE COALESCE (CM.FRIENDLYNAME , Fieldname) 
												END)
			else AT.FRIENDLYNAME 
			end as FRIENDLYNAME
       , AT.LINKEDVALUE
       , CASE WHEN ( LINKFIELD ) is null then '''' else ( LINKFIELD ) END AS SPECIAL
	   , NULL as [MAPPEDOLDVALUE]
FROM    '+@AUDITDATABASE+'.'+@AUDITSCHEMA+'.'+@AUDITTABLE+' as AT
left join
		dbCodeLookup cl on cl.cdCode = AT.[type] and cdType = ''AUDITINGTYPE''
left join
		dbUser u on AT.UserID = u.usrID
LEFT JOIN 
       '+@SOURCEDATABASE+'.Audit.COLUMNNAMEMAPPING AS CM
	   ON CM.COLUMNNAME = AT.FIELDNAME AND CM.TABLENAME = ''' + @SOURCEDATABASE + '.' + @SOURCESCHEMA + '.' + @SOURCETABLE + '''
       '+@PRIMARY_KEY_SQL+'
       AND ISNULL ( CM.EXCLUDE , 0 ) = 0

	   and AT.[Type] = (case 
									when ''' + @TYPE + ''' = '''' then AT.[Type]
									else ''' + @TYPE + ''' 
									end)
		and AT.UpdateDate >= ''' + @FROM_STRING + ''' 
		and AT.UpdateDate <= ''' + @TO_STRING + ''''									

		if (@USERID IS NOT NULL)
		begin
			set @SQL = @SQL + ' and isnull(u.usrID, -999) = ' + Convert(nvarchar(max), @USERID)
		end

		IF (@FIELDFILTER IS NOT NULL)
		BEGIN
			SET @SQL = @SQL + ' AND (CASE WHEN AT.FRIENDLYNAME = '''' then COALESCE (CM.FRIENDLYNAME , Fieldname ) else AT.FRIENDLYNAME end) = (CASE
																																						WHEN ''' + @FIELDFILTER + ''' = '''' THEN (CASE WHEN AT.FRIENDLYNAME = '''' then COALESCE (CM.FRIENDLYNAME , Fieldname ) else AT.FRIENDLYNAME end)
																																						ELSE ''' + @FIELDFILTER + '''																																		
																																						END) '
				END

		SET @SQL = @SQL + 'ORDER BY 
								UPDATEDATE DESC
								, COALESCE ( AT.FRIENDLYNAME ,CM.FRIENDLYNAME , Fieldname )'

	   print (@SQL)
	   insert into @AuditMapping execute (@SQL)


		

		DECLARE Column_Cursor CURSOR FOR
		select AuditID,FIELDNAME,SPECIAL,NEWVALUE,OLDVALUE from @AuditMapping where LINKEDVALUE = '' and SPECIAL <>''
		OPEN Column_Cursor;
		FETCH NEXT FROM Column_Cursor into @AUDITID,@FIELDNAME,@SPECIAL,@NEWVALUE,@OLDVALUE
		WHILE @@FETCH_STATUS = 0
		BEGIN
			if  charindex('NEWVALUE',@SPECIAL) > 0
			BEGIN
			-- Get the NewValue
				set @SQL = (REPLACE (@SPECIAL,'NEWVALUE', ''''+@NEWVALUE+''''))
				EXEC sp_executesql @sql,N'@VALUE NVARCHAR(max) OUTPUT',@SELVALUE OUTPUT
				update @AuditMapping set LINKEDVALUE = @SELVALUE WHERE AUDITID = @AUDITID

				if (@OLDVALUE IS NOT NULL)
				begin
					-- Get the old value
					set @SQL = (REPLACE (@SPECIAL,'NEWVALUE', ''''+@OLDVALUE+''''))
					EXEC sp_executesql @sql,N'@VALUE NVARCHAR(max) OUTPUT',@SELVALUE OUTPUT
					update @AuditMapping set MAPPEDOLDVALUE = @SELVALUE WHERE AUDITID = @AUDITID
				end
			end
			FETCH NEXT FROM Column_Cursor into @AUDITID,@FIELDNAME,@SPECIAL,@NEWVALUE,@OLDVALUE
		END;
		CLOSE Column_Cursor;
		DEALLOCATE Column_Cursor;

		
		select	
			[Tablename]
			, [AUDITID]
			, [TYPE]
			, [FRIENDLYTYPE]
			, [FIELDNAME]
			, [OLDVALUE] as [OLDVALUE_PREFORMAT]
			, [NEWVALUE] as [NEWVALUE_PREFORMAT]
			, case 
				when [MAPPEDOLDVALUE] != '' THEN [MAPPEDOLDVALUE]
				else [OLDVALUE]
				end as [OLDVALUE]
			, case 
				when [LINKEDVALUE] != '' THEN [LINKEDVALUE]
				else [NEWVALUE]
				end as [NEWVALUE]
			, [UPDATEDATE]
			, case 
				when [UserID] = -999 then [USERNAME] + ' (Non-MatterSphere User)' 
				else [USERNAME] 
			  end as [USERNAME] 
			, [FRIENDLYNAME]
			, [LINKEDVALUE] 
		from 
			@AuditMapping

	end