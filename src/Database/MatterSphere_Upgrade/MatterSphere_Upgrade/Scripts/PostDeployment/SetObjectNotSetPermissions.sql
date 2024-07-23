/******************** Type 1 will set un-set object permissions to DENY *******************************/
/******************** Type 0 will set un-set object  permissions to Allow ****************************/
/****************************************************************************************************/

DECLARE @type bit = 1
	DECLARE @allow varbinary(32), @deny varbinary(32), @byte int, @bitvalue int,@newallow varbinary(32),@pid uniqueidentifier

	DECLARE P1Cursor CURSOR FOR 
	SELECT
		op.id
	FROM  [config].[ObjectPolicy] OP 
	OPEN P1Cursor;
	FETCH NEXT FROM P1Cursor into @pid;
	WHILE @@FETCH_STATUS = 0
	BEGIN
			set @newallow = null

			DECLARE PolicyCursor CURSOR FOR 
			SELECT
				op.[AllowMask],
				op.[DenyMask],
				OPC.[Byte],
				OPC.[BitValue]
			FROM  [config].[ObjectPolicy] OP
			CROSS JOIN [config].[ObjectPolicyConfig] OPC 
			WHERE 
				SecurableType IS NOT NULL 
			AND
				[Permission] IS NOT NULL
			AND
				( SELECT [config].[GetSecurityLevel]() & OPC.SecurityLevel ) <> 0
			AND Substring ( op.[AllowMask] , OPC.[Byte] , 1 ) & OPC.[BitValue] <> OPC.[BitValue] 
			AND Substring (  op.[DenyMask] , OPC.[Byte] , 1 ) & OPC.[BitValue] <> OPC.[BitValue] 
			and op.id = @pid
			AND Permission NOT like 'FULL%'

			OPEN PolicyCursor;
			FETCH NEXT FROM PolicyCursor into @allow,@deny,@byte,@bitvalue;
			WHILE @@FETCH_STATUS = 0
			BEGIN
					if ISNULL(DATALENGTH(@newallow), -1) = -1
						set @newallow = case when @type = 0 then @allow else @deny end
					set @newallow=dbo.ConvertBinary ( @newallow , @byte , @bitvalue) 
				FETCH NEXT FROM PolicyCursor into @allow,@deny,@byte,@bitvalue;
			END;
			CLOSE PolicyCursor;
			DEALLOCATE PolicyCursor;

			IF @newallow IS NOT NULL
			If @type = 0
				Update [config].[ObjectPolicy] set [AllowMask] = @newallow where id = @pid
			else
				Update [config].[ObjectPolicy] set [DenyMask] = @newallow where id = @pid
			
			print @pid
			print @allow
			print @deny
			print @newallow

		FETCH NEXT FROM P1Cursor into @pid;
	END;
	CLOSE P1Cursor;
	DEALLOCATE P1Cursor;
