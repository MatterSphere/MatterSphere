CREATE TABLE [dbo].[dbState] (
    [stateID]   INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [stateCode] NVARCHAR (50)    NULL,
    [brID]      INT              NULL,
    [usrID]     INT              NULL,
    [stateData] SQL_VARIANT      NULL,
    [rowguid]   UNIQUEIDENTIFIER CONSTRAINT [DF_dbState_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbState] PRIMARY KEY CLUSTERED ([stateID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbState_rowguid]
    ON [dbo].[dbState]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbState]
    ON [dbo].[dbState]([stateCode] ASC, [brID] ASC, [usrID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];

