CREATE PROC [dbo].[GETAUDITHISTORY_ClientContacts] 

	@CLID nvarchar(max) 
	, @TYPE nvarchar(15) = ''
	, @USERID bigint = NULL
	, @FROM datetime = NULL
	, @TO datetime = NULL
	, @FIELDFILTER nvarchar(max) = ''
WITH EXECUTE AS OWNER
	AS

BEGIN
		declare
				@FIELDNAME sysname,
				@SPECIAL nvarchar(500),
				@NEWVALUE nvarchar(max),
				@SELVALUE nvarchar(Max),
				@FROM_STRING nvarchar(20) = ISNULL(Convert(nvarchar, @FROM), '1 Jan 1900'),
				@TO_STRING nvarchar(20) = ISNULL(Convert(nvarchar, @TO), '31 Dec 2100'),
				@OLDVALUE nvarchar(max),
				@AUDITID uniqueidentifier,
				@SQL nvarchar(max) = ''


		DECLARE  @AuditMapping TABLE
		(
			[TableName] [nvarchar](256),
			[AUDITID] [uniqueidentifier],
			[CONTID] [bigint],
			[CONTNAME] [nvarchar](max),
			[TYPE] [char](1),
			[FRIENDLYTYPE] [nvarchar](1000),
			[FIELDNAME] [nvarchar](1000),
			[OLDVALUE] [nvarchar](max) NULL,
			[NEWVALUE] [nvarchar](max) NULL,
			[UPDATEDATE] [datetime] NULL,
			[APPNAME] [nvarchar](max) NULL,
			[USERID] [bigint] NULL,
			[USERNAME] [nvarchar](200) NULL,
			[FRIENDLYNAME] [nvarchar](256) NULL,
			[LINKEDVALUE] [nvarchar](max) NULL,
			[SPECIAL] [nvarchar](max) NULL, 
			[MAPPEDOLDVALUE] [nvarchar](max) NULL
		)


		set @SQL = 'select 
					''dbo_dbClientContacts'' as TableName
					, AUDITID as [AUDITID]
					, CC.CONTID  as contID
					, CONT.CONTNAME as contName
					, AT.Type as [TYPE]
					, cl.cdDesc AS [FRIENDLYTYPE]
					, AT.FieldName as [FIELDNAME]
					, Convert(nvarchar(max), AT.OLDVALUE) as [OLDVALUE]
					, Convert(nvarchar(max), AT.NEWVALUE) as [NEWVALUE]
					, AT.UpdateDate as [UPDATEDATE]
					, AT.AppName
					, isnull(AT.UserID, -999) as [UserID]
					, isnull(u.usrFullName+ '' ('' + CASE WHEN u.[AccessType] = ''INTERNAL'' then  u.usrADID else u.[AccessType] end + '')'', AT.UserName)  as [USERNAME]
					, (CASE 
						WHEN AT.FRIENDLYNAME = '''' then COALESCE (CM.FRIENDLYNAME , Fieldname ) 
						else AT.FRIENDLYNAME 
						end) as [FRIENDLYNAME]
					,  AT.LINKEDVALUE as [LINKEDVALUE]
					, CASE WHEN ( LINKFIELD ) is null then '''' else ( LINKFIELD ) END AS [SPECIAL]
					, NULL as [MAPPEDOLDVALUE]
				from 
					dbclientcontacts CC
				inner join 
					audit.dbo_dbClientContacts AT on CC.id = AT.id 
				INNER JOIN 
					DBCONTACT CONT ON CONT.CONTID = CC.CONTID
				left join
					dbUser u on AT.UserID = u.usrID
				left join
					audit.ColumnNameMapping cm on cm.columnName = AT.fieldName and cm.TableName = ''[' + DB_NAME() + '].[dbo].[dbClientContacts]''
				left join
					dbCodeLookup cl on cl.cdCode = AT.[type] and cdType = ''AUDITINGTYPE''
				where 
					CC.clid = ' + Convert(nvarchar(max), @CLID) + ' 
					and isnull(cm.Exclude, 0) = 0 
					and AT.[type] = (case
											when ''' + @TYPE + ''' = '''' then AT.[type]
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
								AT.UPDATEDATE DESC
								, COALESCE ( AT.FRIENDLYNAME ,CM.FRIENDLYNAME , Fieldname )'


		print @SQL
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
			, [CONTID]
			, [CONTNAME]
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

END