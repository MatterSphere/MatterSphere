SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE Name = 'tgrUpdateToContactNumber' AND type = 'TR') 
       DROP TRIGGER [dbo].[tgrUpdateToContactNumber]
GO
CREATE TRIGGER [dbo].[tgrUpdateToContactNumber] ON [dbo].[dbContactNumbers]
FOR INSERT , UPDATE NOT FOR REPLICATION
AS
SET NOCOUNT ON
BEGIN
       --Update contact Update flag where dbContactNumbers has changed for the contact
       UPDATE 
              CONT
       SET 
              CONT.Updated = GETUTCDATE ()
       FROM 
              [dbo].[dbContact] CONT 
       JOIN
              [Inserted] I ON I.contID = CONT.contID
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE Name = 'tgrUpdateToContactEmail' AND type = 'TR') 
       DROP TRIGGER [dbo].[tgrUpdateToContactEmail]
GO
CREATE TRIGGER [dbo].[tgrUpdateToContactEmail] ON [dbo].[dbContactEmails]
FOR INSERT , UPDATE NOT FOR REPLICATION
AS
SET NOCOUNT ON
BEGIN
       --Update contact Update flag where dbContactEmails has changed for the contact
       UPDATE 
              CONT
       SET 
              CONT.Updated = GETUTCDATE ()
       FROM 
              [dbo].[dbContact] CONT 
       JOIN
              [Inserted] I ON I.contID = CONT.contID
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE Name = 'tgrUpdateToContactIndividual' AND type = 'TR') 
       DROP TRIGGER [dbo].[tgrUpdateToContactIndividual]
GO
CREATE TRIGGER [dbo].[tgrUpdateToContactIndividual] ON [dbo].[dbContactIndividual]
FOR INSERT , UPDATE NOT FOR REPLICATION
AS
SET NOCOUNT ON
BEGIN
       --Update contact Update flag where a dbContactIndividual has changed for the contact
       UPDATE 
              CONT
       SET 
              CONT.Updated = GETUTCDATE ()
       FROM 
              [dbo].[dbContact] CONT 
       JOIN
              [Inserted] I ON I.contID = CONT.contID
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE Name = 'tgrUpdateToContactCompany' AND type = 'TR') 
       DROP TRIGGER [dbo].[tgrUpdateToContactCompany]
GO
CREATE TRIGGER [dbo].[tgrUpdateToContactCompany] ON [dbo].[dbContactCompany]
FOR INSERT , UPDATE NOT FOR REPLICATION
AS
SET NOCOUNT ON
BEGIN
       --Update contact Update flag where dbContactCompany has changed for the contact
       UPDATE 
              CONT
       SET 
              CONT.Updated = GETUTCDATE ()
       FROM 
              [dbo].[dbContact] CONT 
       JOIN
              [Inserted] I ON I.contID = CONT.contID
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE Name = 'tgrUpdateToContactAddress' AND type = 'TR') 
       DROP TRIGGER [dbo].[tgrUpdateToContactAddress]
GO
CREATE TRIGGER [dbo].[tgrUpdateToContactAddress] ON [dbo].[dbAddress]
FOR INSERT , UPDATE NOT FOR REPLICATION
AS
SET NOCOUNT ON
BEGIN
       --Update contact Update flag where dbContactAddress has changed for the contact
       UPDATE 
              CONT
       SET 
              CONT.Updated = GETUTCDATE ()
       FROM 
              [dbo].[dbContact] CONT 
       JOIN
              [dbo].[dbContactAddresses] CA ON CA.CONTID = CONT.CONTID
       JOIN
              [Inserted] I ON I.addID = CA.contAddID
END

