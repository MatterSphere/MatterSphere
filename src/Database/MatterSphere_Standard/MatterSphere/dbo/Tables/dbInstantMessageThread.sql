CREATE TABLE [dbo].[dbInstantMessageThread] (
    [ID]        UNIQUEIDENTIFIER CONSTRAINT [DF_dbInstantMessageThread_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [keyID]     BIGINT           NOT NULL,
    [Reference] NVARCHAR (50)    NULL,
    [Name]      NVARCHAR (150)   NOT NULL,
    [Created]   DATETIME         CONSTRAINT [DF_dbInstantMessageThread_Created] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy] INT              NOT NULL,
    [Updated]   DATETIME         CONSTRAINT [DF_dbInstantMessageThread_LastUpdated] DEFAULT (getutcdate()) NOT NULL,
    [Active]    BIT              NULL,
    [Private]   BIT              CONSTRAINT [DF_dbInstantMessageThread_Private] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbInstantMessageThread] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_dbInstantMessageThread_dbUser] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbInstantMessageThread] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbInstantMessageThread] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbInstantMessageThread] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbInstantMessageThread] TO [OMSApplicationRole]
    AS [dbo];

