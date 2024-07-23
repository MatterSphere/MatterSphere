print 'Starting dbUser_SecurityID_Populate.sql'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

UPDATE [dbo].[dbUser] SET [dbo].[dbUser].SecurityID = IU.ID
FROM [dbo].[dbUser] dbU JOIN [item].[user] IU ON IU.NTLogin = dbU.usrADID
WHERE dbU.SecurityID is null or dbU.SecurityID <> IU.ID

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO