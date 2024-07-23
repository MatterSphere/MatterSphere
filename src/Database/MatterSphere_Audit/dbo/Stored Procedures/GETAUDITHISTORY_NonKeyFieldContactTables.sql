/*****************************************************************************************************************************************************************
	The original GETAUDITHISTORY stored procedure is used for building up audit data based on contID, fileID, clID, assocID etc. being the 
	primary keys on a table. This stored procedure has been written to pull back audit data for tables which are linked to key tables (such as dbClient or dbFile)
	but uses their own IDs, as they have been designed with a one to many relationship in mind e.g. dbContactEmails or dbContactNumbers.

	This has been written for the one to many Contact tables.
*****************************************************************************************************************************************************************/

CREATE PROC [dbo].[GETAUDITHISTORY_NonKeyFieldContactTables] 

	@AUDIT_TABLE_NAME sysname = null
	, @ID nvarchar(max) = null
	, @TYPE nvarchar(15) = ''
	, @USERID bigint = NULL
	, @FROM datetime = NULL
	, @TO datetime = NULL
	, @FIELDFILTER nvarchar(max) = ''
	, @ID_FIELD_NAME nvarchar(max) = 'contID'
WITH EXECUTE AS OWNER
	AS

BEGIN

	declare @SOURCEDATABASE sysname
	declare	@SOURCESCHEMA sysname
	declare	@SOURCETABLE sysname
	declare @FROM_STRING nvarchar(20) = ISNULL(Convert(nvarchar, @FROM), '1 Jan 1900')
	declare @TO_STRING nvarchar(20) = ISNULL(Convert(nvarchar, @TO), '31 Dec 2100')
	declare @VALUE nvarchar(200)
	declare @AUDITID uniqueidentifier
	declare @FIELDNAME sysname
	declare @SPECIAL nvarchar(500)
	declare @NEWVALUE nvarchar(max)
	declare @SELVALUE nvarchar(Max)
	declare @OLDVALUE nvarchar(max)

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
			[USERID] [bigint] NULL,
			[USERNAME] [nvarchar](200) NULL,
			[FRIENDLYNAME] [nvarchar](256) NULL,
			[LINKEDVALUE] [nvarchar](max) NULL,
			[SPECIAL] [nvarchar](max) NULL,
			[MAPPEDOLDVALUE] [nvarchar](max) NULL)

	SELECT @SOURCEDATABASE=left([acTableName],charindex('.',[acTableName])-1),
		   @SOURCESCHEMA=right(left([acTableName],charindex('.',[acTableName],charindex('.',[acTableName])+1)-1), charindex('.',reverse(left([acTableName],charindex('.',[acTableName],charindex('.',[acTableName])+1)-1)))-1),
		   @SOURCETABLE=right([acTableName],charindex('.',REVERSE([acTableName]))-1)
	FROM 
		[audit].[Configuration] 
	where 
		right([acAuditTableName],charindex('.',REVERSE([acAuditTableName]))-1) = '[' + @AUDIT_TABLE_NAME + ']'

	declare @SQL nvarchar(max) = ''



	/******************************************************************************************************************************************
												INCLUDE DBADDRESS DATA - START
												==============================
	The dbContactAddresses search list needs a different view to the other contact related search lists. This search list needs to include
	the related dbAddress fields. This section adds the dbAddress data only if the calling search list has supplied the table name of 
	dbo_dbContactAddresses. The resulting SQL is then unioned against the main data retrival from the dbo_dbContactAddresses table.
	******************************************************************************************************************************************/	
	if (@AUDIT_TABLE_NAME = 'dbo_dbContactAddresses')
	begin
		declare @ADDRESS_IDS nvarchar(200)

			select 
				@ADDRESS_IDS = COALESCE(@ADDRESS_IDS + ', ', '') + Convert(nvarchar(50), contAddID)
			from
				(SELECT 
					distinct contAddID
				FROM 
					[Audit].dbo_dbContactAddresses
				where 
					contID = @ID)  as contactAddressIDs

		if (@ADDRESS_IDS is not null AND @ADDRESS_IDS != '')
		begin
			set @SQL = 'select 
						''[dbo_dbAddress]'' as [TableName] 
						, AT.AuditID as [AUDITID]
						, AT.[Type] as [TYPE]
						, cl.cdDesc AS [FRIENDLYTYPE]
						, AT.FieldName  as [FIELDNAME]
						, Convert(nvarchar(max), AT.OLDVALUE) as [OLDVALUE]
						, Convert(nvarchar(max), AT.NEWVALUE) as [NEWVALUE]
						, AT.UpdateDate AS [UPDATEDATE]
						, AT.AppName
						, isnull(AT.UserID, -999) as [USERID]
						, isnull(u.usrFullName+ '' ('' + CASE WHEN u.[AccessType] = ''INTERNAL'' then  u.usrADID else u.[AccessType] end + '')'', AT.UserName)  as [USERNAME]
				    	, CASE 
							WHEN AT.FRIENDLYNAME = '''' then
											(CASE 
												WHEN CM.FRIENDLYNAME = '''' THEN Fieldname
												ELSE COALESCE (CM.FRIENDLYNAME , Fieldname) 
												END)
							else AT.FRIENDLYNAME 
							end as [FRIENDLYNAME]
						, AT.LinkedValue as [LINKEDVALUE]
						, CASE WHEN ( LINKFIELD ) is null then '''' else ( LINKFIELD ) END AS SPECIAL
						, NULL as [MAPPEDOLDVALUE]	
					from 
						[Audit].[dbo_dbAddress] AT
					left join
						dbUser u on AT.UserID = u.usrID
					left join
						audit.ColumnNameMapping cm on cm.columnName = AT.fieldName and cm.TableName = ''' + @SOURCEDATABASE +  '.' + @SOURCESCHEMA + '.[dbAddress]'' 
					left join
						dbCodeLookup cl on cl.cdCode = AT.[type] and cdType = ''AUDITINGTYPE''
					where
						AT.addID in ( ' + @ADDRESS_IDS + ' ) 
						and isnull(cm.Exclude, 0) = 0 
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
																																			END) 
																																			'
					END

					set @SQL = @SQL + ' union 
					'

			print (@SQL)
		end 
	end
	/******************************************************************************************************************************************
												INCLUDE DBADDRESS DATA - FINISH
	******************************************************************************************************************************************/



	/******************************************************************************************************************************************
												BUILD SQL BASED ON SUPPLIED @AUDIT_TABLE_NAME VALUE - START
	******************************************************************************************************************************************/
	
	set @SQL = @SQL + 'select 
					''[' + @AUDIT_TABLE_NAME + ']'' as [TableName] 
					, AT.AuditID as [AUDITID]
					, AT.[Type] as [TYPE]
					, cl.cdDesc AS [FRIENDLYTYPE]
					, AT.FieldName  as [FIELDNAME]
					, Convert(nvarchar(max), AT.OLDVALUE) as [OLDVALUE]
					, Convert(nvarchar(max), AT.NEWVALUE) as [NEWVALUE]
					, AT.UpdateDate AS [UPDATEDATE]
					, AT.AppName
					, isnull(AT.UserID, -999) as [USERID]
					, isnull(u.usrFullName+ '' ('' + CASE WHEN u.[AccessType] = ''INTERNAL'' then  u.usrADID else u.[AccessType] end + '')'', AT.UserName)  as [USERNAME]
					, CASE 
						WHEN AT.FRIENDLYNAME = '''' then
										(CASE 
											WHEN CM.FRIENDLYNAME = '''' THEN Fieldname
											ELSE COALESCE (CM.FRIENDLYNAME , Fieldname) 
											END)
						else AT.FRIENDLYNAME 
						end as [FRIENDLYNAME]
					, AT.LinkedValue as [LINKEDVALUE]
					, CASE WHEN ( LINKFIELD ) is null then '''' else ( LINKFIELD ) END AS SPECIAL
					, NULL as [MAPPEDOLDVALUE]	
				from 
					[Audit].[' + @AUDIT_TABLE_NAME + '] AT
				left join
					dbUser u on AT.UserID = u.usrID
				left join
					audit.ColumnNameMapping cm on cm.columnName = AT.fieldName and cm.TableName = ''' + @SOURCEDATABASE + '.' + @SOURCESCHEMA + '.' + @SOURCETABLE + '''
				left join
					dbCodeLookup cl on cl.cdCode = AT.[type] and cdType = ''AUDITINGTYPE''
				where
					AT.' + @ID_FIELD_NAME + ' = ' + @ID + 
					' and isnull(cm.Exclude, 0) = 0 
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

				SET @SQL = @SQL + 'order by 
									AT.UpdateDate desc'

	print @sql

	/******************************************************************************************************************************************
												BUILD SQL BASED ON SUPPLIED @AUDIT_TABLE_NAME VALUE - FINISH
	******************************************************************************************************************************************/



	insert into @AuditMapping execute (@sql)

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

END