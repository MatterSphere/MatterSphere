print 'Starting dbUser_SecurityID.sql'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT [name] from sys.Columns WHERE object_id = object_id('dbUser') AND [name] = 'SecurityID' )
BEGIN
		ALTER TABLE [dbo].[dbUser]
		ADD SecurityID uniqueidentifier NULL
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO