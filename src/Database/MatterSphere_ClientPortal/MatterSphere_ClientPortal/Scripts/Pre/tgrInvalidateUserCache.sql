SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tgrInvalidateUserCache]'))
	DROP TRIGGER [dbo].[tgrInvalidateUserCache]
GO


CREATE TRIGGER [dbo].[tgrInvalidateUserCache]
   ON  [dbo].[dbUser]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;

	--Only update on certain changes because matter sphere desktop client frequently updates the dbUser table
	IF UPDATE(usrPassword) OR UPDATE(AccessType) OR UPDATE(usrActive) OR UPDATE(usrEmail) OR UPDATE(usrDocumentNotification) OR UPDATE (usrLastLogin)
	BEGIN
		PRINT 'Trigger Updating'
		IF EXISTS (SELECT * FROM [dbo].[dbTableMonitor] WHERE [TableName] = 'dbUser')
		BEGIN
			UPDATE [dbo].[dbTableMonitor]
			SET [LastUpdated] =  DateAdd(minute, 1, GetUtcDate())
			WHERE Tablename = 'dbUser'
		END
		ELSE
		BEGIN
		INSERT INTO [dbo].[dbTableMonitor]
				   (
					   [TableName]
					   ,[Category]
					   ,[LastUpdated]
				   )
			 VALUES
				   (
					   'dbUser'
					   ,'Updated'
					   ,GetUtcDate()
				   )
		END
	END
END

GO
