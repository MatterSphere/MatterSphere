CREATE TABLE [dbo].[dbClientContacts] (
    [ID]         BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [clID]       BIGINT              NOT NULL,
    [contID]     BIGINT              NOT NULL,
    [clRelation] [dbo].[uCodeLookup] NULL,
    [clPosition] NVARCHAR (50)       NULL,
    [clNotePad]  NVARCHAR (200)      NULL,
    [clActive]   BIT                 CONSTRAINT [DF_dbClientContacts_clActive] DEFAULT ((1)) NOT NULL,
    [rowguid]    UNIQUEIDENTIFIER    CONSTRAINT [DF_dbClientContacts_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbClientContacts] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbClientContacts_dbClient] FOREIGN KEY ([clID]) REFERENCES [dbo].[dbClient] ([clID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbClientContacts_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbClientContacts]
    ON [dbo].[dbClientContacts]([clID] ASC, [contID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbClientContacts_rowguid]
    ON [dbo].[dbClientContacts]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


-- =============================================
-- Author:		<Conrad McLaughlin>
-- Create date: <23.04.2009>
-- Description:	<Contact Links Trigger>
-- =============================================
CREATE TRIGGER [dbo].[fdCreateContactLinks] ON  [dbo].[dbClientContacts]
-- ** INSERT ** --
AFTER INSERT
AS 
	--Client type used for contact links
	DECLARE @cltype nvarchar(15)
	SET @cltype = 'ENT'
	--Contact link codes
	DECLARE @clilinkcode nvarchar(15)
	DECLARE @contlinkcode nvarchar(15)
	SET @clilinkcode = 'OMSCLI'
	SET @contlinkcode = 'OMSCON'
	--Client/Contact IDs
	DECLARE @clid bigint	
	DECLARE @contid bigint
	SELECT	@clid = clid, @contid = contid FROM	INSERTED
	--Client's default contact ID
	DECLARE @cl_contid bigint
	SET @cl_contid = (SELECT TOP 1 cldefaultcontact FROM dbclient WHERE clid = @clid)
	--Date
	DECLARE @date DateTime
	SET @date = GETUTCDATE()
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--Add contact links
	IF (SELECT TOP 1 cltypecode FROM dbClient WHERE clid = @clid) = @cltype
	BEGIN
		EXEC fdsprContactLinks_Add @contid, @cl_contid, @clilinkcode, @contlinkcode, @date, '-1'
	END
END


SET ANSI_NULLS ON

GO


-- =============================================
-- Author:		<Conrad McLaughlin>
-- Create date: <23.04.2009>
-- Description:	<Contact Links Update Trigger>
-- =============================================
CREATE TRIGGER [dbo].[fdUpdateContactLinks] ON  [dbo].[dbClientContacts]
-- Delete/Add contact link when client contact is made Inactive/Active (respectfully)
AFTER UPDATE
AS
	--Client/Contact IDs
	DECLARE @clid bigint	
	DECLARE @contid bigint	
	SELECT	@clid = clid, @contid = contid FROM	INSERTED
	--Client's default contact ID
	DECLARE @cl_contid bigint
	SET @cl_contid = (SELECT TOP 1 cldefaultcontact FROM dbclient WHERE clid = @clid)
	--Active
	DECLARE @clactive bit
	SELECT @clactive = clactive FROM INSERTED
	--Contact link codes
	DECLARE @clilinkcode nvarchar(15)
	DECLARE @contlinkcode nvarchar(15)
	SET @clilinkcode = 'OMSCLI'
	SET @contlinkcode = 'OMSCON'
	--Date
	DECLARE @date DateTime
	SET @date = GETUTCDATE()
BEGIN
	IF (@clactive) = 0
	BEGIN
		-- Delete the link
		EXEC fdsprContactLinks_Delete @contid, @cl_contid
	END

	IF (@clactive) = 1
	BEGIN
		-- If contact link has been made active
		EXEC fdsprContactLinks_Add @contid, @cl_contid, @clilinkcode, @contlinkcode, @date, '-1'
	END
END

SET ANSI_NULLS ON

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbClientContacts] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbClientContacts] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbClientContacts] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbClientContacts] TO [OMSApplicationRole]
    AS [dbo];

