/****** Object:  StoredProcedure [dbo].[AddQueueItem]    Script Date: 24/05/2013 08:21:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddQueueItem]
	(
	@UserEmail nvarchar(50),
	@FolderID nvarchar(max),
	@MessageID nvarchar(max),
	@UserID bigint,
	@FileID bigint,
	@Created datetime,
	@Updated datetime,
	@Processed bit
	)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @VarMessageID VARBINARY(MAX)
	SET @VarMessageID = CONVERT(VARBINARY(MAX),@MessageID)

	IF NOT EXISTS (Select * from [MCEP].[Queue] Where CONVERT(VARBINARY(MAX),MessageID) = @VarMessageID)
	BEGIN
    INSERT INTO MCEP.Queue
	(UserEmail,
	FolderID,
	MessageID,
	UserID,
	FileID,
	Created,
	Updated,
	Processed)
	VALUES
	(@UserEmail,
	@FolderID,
	@MessageID,
	@UserID,
	@FileID,
	@Created,
	@Updated,
	@Processed)
	END

END

GO

/****** Object:  StoredProcedure [dbo].[AddUser]    Script Date: 24/05/2013 08:21:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddUser]
	(
	@UserID bigint,
	@Email nvarchar(150),
	@RootFolderName nvarchar(50),
	@Created datetime,
	@Active bit
	)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF NOT EXISTS (Select * from [MCEP].[User] Where UserID = @UserID)
	BEGIN
    INSERT INTO MCEP.[User]
	(UserID
	,Email
	,RootFolderName
	,Created
	,Active
	)
	VALUES
	(@UserID
	,@Email
	,@RootFolderName
	,@Created
	,@Active)
	END

END

GO

/****** Object:  StoredProcedure [dbo].[MCEPGetItems]    Script Date: 24/05/2013 08:21:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[MCEPGetItems]

AS
BEGIN

	SELECT 
		[ID]
      ,[UserEmail]
      ,[FolderID]
      ,[MessageID]
      ,[UserID]
      ,[FileID]
      ,[AssocID]
      ,[DocID]
      ,[Created]
      ,[Updated]
      ,[Processed]
      ,[Result]
      ,[ErrorMessage]
  FROM [MCEP].[Queue] 
  Where ([Processed] = 0 or [Processed] is Null)
  Order by Created, ID ASC

END

GO

/****** Object:  StoredProcedure [dbo].[MCEPGetUsers]    Script Date: 24/05/2013 08:21:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[MCEPGetUsers]

AS
BEGIN

	SELECT 
		[UserID]
      ,[Email]
      ,[RootFolderName]
      ,[RootFolderID]
      ,[Created]
      ,[Updated]
      ,[LastRan]
      ,[Active]
  FROM [MCEP].[User] 
  Where [Active] = 1
  Order by LastRan, UserID ASC

END

GO

/****** Object:  StoredProcedure [dbo].[MCEPGetUsersADMIN]    Script Date: 24/05/2013 08:21:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[MCEPGetUsersADMIN]

AS
BEGIN

	SELECT 
		[UserID]
      ,[Email]
      ,[RootFolderName]
      ,[RootFolderID]
      ,[Created]
      ,[Updated]
      ,[LastRan]
      ,[Active]
  FROM [MCEP].[User] 
  Order by Active DESC, Email ASC

END

GO


