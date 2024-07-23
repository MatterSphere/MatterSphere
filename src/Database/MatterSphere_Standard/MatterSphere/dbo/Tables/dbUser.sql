CREATE TABLE [dbo].[dbUser] (
    [usrID]                        INT                    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [usrInits]                     NVARCHAR (30)          NOT NULL,
    [usrAlias]                     NVARCHAR (36)          NULL,
    [usrADID]                      NVARCHAR (50)          CONSTRAINT [DF_dbUser_usrADID] DEFAULT (newid()) NULL,
    [usrSQLID]                     NVARCHAR (50)          CONSTRAINT [DF_dbUser_usrSQLID] DEFAULT (newid()) NULL,
    [usrFullName]                  NVARCHAR (50)          NOT NULL,
    [usrPassword]                  NVARCHAR (15)          NULL,
    [usrType]                      [dbo].[uCodeLookup]    CONSTRAINT [DF_dbUser_usrType] DEFAULT (N'STANDARD') NOT NULL,
    [usrCCType]                    [dbo].[uCodeLookup]    CONSTRAINT [DF_dbUser_usrCCType] DEFAULT (N'STANDARD') NOT NULL,
    [usrEmail]                     [dbo].[uEmail]         NULL,
    [usrWorksFor]                  INT                    NOT NULL,
    [usrRole]                      [dbo].[uCodeLookup]    CONSTRAINT [DF_dbUser_usrRole] DEFAULT ('SYSTEM') NOT NULL,
    [brID]                         INT                    NULL,
    [usrprintID]                   INT                    NULL,
    [usrLoggedIn]                  BIT                    CONSTRAINT [DF_dbUser_usrLoggedIn] DEFAULT ((0)) NOT NULL,
    [usrLastLogin]                 SMALLDATETIME          NULL,
    [usrtermName]                  NVARCHAR (50)          NULL,
    [usrExtension]                 VARCHAR (10)           NULL,
    [usrDDI]                       [dbo].[uTelephone]     NULL,
    [usrDDIFax]                    [dbo].[uTelephone]     NULL,
    [usrFailed]                    TINYINT                CONSTRAINT [DF_dbUser_uFailed] DEFAULT ((0)) NOT NULL,
    [usrRegisteredFor]             NVARCHAR (1000)        NULL,
    [usrActive]                    BIT                    CONSTRAINT [DF_dbUser_uActive] DEFAULT ((1)) NOT NULL,
    [usrScript]                    [dbo].[uCodeLookup]    NULL,
    [usrWelcomeWizard]             BIT                    CONSTRAINT [DF_dbUser_usrWelcomeWizard] DEFAULT ((1)) NOT NULL,
    [usrUICultureInfo]             [dbo].[uUICultureInfo] NULL,
    [usrcurISOCode]                CHAR (3)               NULL,
    [usrRTL]                       BIT                    CONSTRAINT [DF_dbUser_usrRTL] DEFAULT ((0)) NOT NULL,
    [usrMRIListcount]              SMALLINT               CONSTRAINT [DF_dbUser_usrMRIListcount] DEFAULT ((10)) NOT NULL,
    [usrMAPIMailbox]               NVARCHAR (50)          NULL,
    [usrMAPIEntryID]               NVARCHAR (255)         NULL,
    [usrMAPIAppointmentStore]      NVARCHAR (255)         NULL,
    [usrXML]                       [dbo].[uXML]           CONSTRAINT [DF_dbUser_usrXML] DEFAULT (N'<config/>') NULL,
    [usrExchangeSync]              BIT                    CONSTRAINT [DF_dbUser_usrExchangeSync] DEFAULT ((0)) NOT NULL,
    [usrPrecCat]                   [dbo].[uCodeLookup]    NULL,
    [usrPrecSubCat]                [dbo].[uCodeLookup]    NULL,
	[usrPrecMinorCat]              [dbo].[uCodeLookup]    NULL,
    [Created]                      [dbo].[uCreated]       NULL,
    [CreatedBy]                    [dbo].[uCreatedBy]     NULL,
    [Updated]                      [dbo].[uCreated]       NULL,
    [UpdatedBy]                    [dbo].[uCreatedBy]     NULL,
    [usrHomePage]                  NVARCHAR (100)         NULL,
    [usrJobTitle]                  NVARCHAR (100)         NULL,
    [usrMobile]                    [dbo].[uTelephone]     NULL,
    [usrDOB]                       DATETIME               NULL,
    [usrExtID]                     INT                    NULL,
    [rowguid]                      UNIQUEIDENTIFIER       CONSTRAINT [DF_dbUser_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [usrImageFolder]               NVARCHAR (255)         NULL,
    [usrImageFolderDone]           NVARCHAR (255)         NULL,
    [AccessType]                   [dbo].[uCodeLookup]    CONSTRAINT [DF_dbUser_AccessType] DEFAULT ('INTERNAL') NOT NULL,
    [SecurityID]                   UNIQUEIDENTIFIER       NULL,
    [usrMCPToken]                  NVARCHAR (50)          NULL,
    [usrMCPPWReset]                DATETIME               NULL,
    [usrDocumentNotification]      BIT                    NULL,
    [usrDocNotifyFeeEarnerManager] BIT                    NULL,
    [usrMSUserType]                [dbo].[uCodeLookup]    DEFAULT ('INTERNAL') NOT NULL,
    [usrDept]                      [dbo].[uCodeLookup]    NULL,
    [usrNeedsExport]               BIT                    CONSTRAINT [DF_dbUser_usrNeedsExport] DEFAULT ((1)) NOT NULL,
    [usrSystemLicense]             NVARCHAR (50)          NULL,
    [usrPowerUserProfileID] INT NULL, 
    CONSTRAINT [PK_dbUser] PRIMARY KEY CLUSTERED ([usrID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbUser_dbBranch] FOREIGN KEY ([brID]) REFERENCES [dbo].[dbBranch] ([brID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbUser_dbCommandCentreType] FOREIGN KEY ([usrCCType]) REFERENCES [dbo].[dbCommandCentreType] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbUser_dbCurrency] FOREIGN KEY ([usrcurISOCode]) REFERENCES [dbo].[dbCurrency] ([curISOCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbUser_dbLanguage] FOREIGN KEY ([usrUICultureInfo]) REFERENCES [dbo].[dbLanguage] ([langCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbUser_dbPrinter] FOREIGN KEY ([usrprintID]) REFERENCES [dbo].[dbPrinter] ([printID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbUser_dbSecurityRole] FOREIGN KEY ([usrRole]) REFERENCES [dbo].[dbSecurityRole] ([roleCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbUser_dbUserType] FOREIGN KEY ([usrType]) REFERENCES [dbo].[dbUserType] ([typeCode]) NOT FOR REPLICATION,
	CONSTRAINT [FK_dbUser_dbPowerUserProfiles] FOREIGN KEY ([usrPowerUserProfileID]) REFERENCES [dbo].[dbPowerUserProfiles] ([id]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbUser_rowguid]
    ON [dbo].[dbUser]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbUser_usrADID]
    ON [dbo].[dbUser]([usrADID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbUser_usrAlias]
    ON [dbo].[dbUser]([usrAlias] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbUser_usrSQLID]
    ON [dbo].[dbUser]([usrSQLID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[SyncNewUser] ON [dbo].[dbUser]
AFTER INSERT NOT FOR REPLICATION
AS

BEGIN
	-- Get Default System Policy
	DECLARE @TYPE NVARCHAR(15) 
	SET @TYPE = ( SELECT ISNULL([Type], 'GLOBALSYSDEF') FROM [config].[SystemPolicy] WHERE IsDefault = 1 )
	
	--Assign Default System Policy to new User
	INSERT [item].[User] ( [NTLogin] , [Name] , [Active] , [PolicyID] )
	SELECT  U.[usrADID] , U.[usrFullName] , U.[usrActive] , A.[ID] FROM Inserted U
	CROSS JOIN  (SELECT [ID] FROM [config].[SystemPolicy] WHERE [Type] = @TYPE)  A;
	
	-- Manage the 'All Internal' Group
	if exists (SELECT * FROM item.[Group] WHERE [Description] = 'AllInternal')
	begin	
		INSERT [relationship].[Group_User] (GroupID,UserID)
		SELECT  A.ID,IU.[ID] FROM [item].[User] iu join Inserted U on iu.NTLogin = U.usrADID
		CROSS JOIN (select id from [item].[Group] WHERE Description = 'AllInternal') A
		Where U.AccessType = 'Internal'
	end
END



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




SET ANSI_NULLS ON

GO


CREATE TRIGGER [dbo].[tgrFlagUserForExport] ON [dbo].[dbUser]
FOR  UPDATE NOT FOR REPLICATION
AS
IF NOT UPDATE (usrNeedsExport)
BEGIN
	UPDATE U SET U.usrNeedsExport = 1
	FROM dbo.dbUser U JOIN Inserted I ON I.usrID = U.usrID
END

GO


CREATE TRIGGER [dbo].[tgrUpdateSecurityUser] ON [dbo].[DBUser] 
FOR UPDATE NOT FOR REPLICATION
AS
BEGIN
	IF EXISTS ( SELECT [usrADID] , [usrFullName] , [usrActive] FROM Inserted EXCEPT SELECT [usrADID] , [usrFullName] , [usrActive] FROM Deleted )
	BEGIN
		UPDATE IU
		SET IU.[NTLogin] = I.[usrADID] , IU.[Name] = I.[usrFullName] , IU.[Active] = I.[usrActive]
		FROM [item].[User] IU JOIN 
		( SELECT A.[usrADID] , A.[usrFullName] , A.[usrActive] , D.[usrADID_old] FROM 
		 ( SELECT [usrID] , [usrADID] , [usrFullName] , [usrActive] FROM Inserted EXCEPT SELECT [usrID] , [usrADID] , [usrFullName] , [usrActive] FROM Deleted) A
		JOIN
		( SELECT [usrID] , [usrADID] as [usrADID_old] FROM DELETED ) D ON D.[usrID] = A.[usrID] ) I ON I.[usrADID_old] = IU.[NTLogin] 
	END
END

SET ANSI_NULLS ON

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbUser] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbUser] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbUser] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbUser] TO [OMSApplicationRole]
    AS [dbo];

