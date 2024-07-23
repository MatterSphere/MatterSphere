Print 'Starting dbUser_AccessType_Populate.sql'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

UPDATE [dbo].[dbUser] SET AccessType = 'INTERNAL'
WHERE AccessType is null AND (usrType <> 'REMOTE' or usrType <> 'CLIENT')
GO

UPDATE [dbo].[dbUser] SET AccessType = 'EXTERNAL'
WHERE AccessType is null AND (usrType = 'REMOTE' or usrType = 'CLIENT')

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO