CREATE TABLE [dbo].[dbInstantMessageThreadMembers] (
    [ID]       UNIQUEIDENTIFIER CONSTRAINT [DF_dbInstantMessageThreadMembers_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [threadID] UNIQUEIDENTIFIER NOT NULL,
    [usrID]    INT              NOT NULL,
    [Joined]   DATETIME         CONSTRAINT [DF_dbInstantMessageThreadMembers_Joined] DEFAULT (getutcdate()) NOT NULL,
    [Active]   BIT              CONSTRAINT [DF_dbInstantMessageThreadMembers_Active] DEFAULT ((1)) NOT NULL,
    [Colour]   CHAR (15)        NULL,
    [Status]   TINYINT          CONSTRAINT [DF_dbInstantMessageThreadMembers_Status] DEFAULT ((0)) NOT NULL,
    [Private]  BIT              CONSTRAINT [DF_dbInstantMessageThreadMembers_Private] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbInstantMessageThreadMembers] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_dbInstantMessageThreadMembers_dbInstantMessageThread] FOREIGN KEY ([threadID]) REFERENCES [dbo].[dbInstantMessageThread] ([ID]),
    CONSTRAINT [FK_dbInstantMessageThreadMembers_dbUser] FOREIGN KEY ([usrID]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbInstantMessageThreadMembers] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbInstantMessageThreadMembers] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbInstantMessageThreadMembers] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbInstantMessageThreadMembers] TO [OMSApplicationRole]
    AS [dbo];

