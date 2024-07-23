CREATE TABLE [dbo].[dbPosition] (
    [posID]          INT                IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [posCode]        NVARCHAR (15)      NOT NULL,
    [posTitle]       NVARCHAR (50)      NOT NULL,
    [posOrder]       SMALLINT           CONSTRAINT [DF_dbPosition_posOrder] DEFAULT ((0)) NOT NULL,
    [posLevel]       SMALLINT           CONSTRAINT [DF_dbPosition_posLevel] DEFAULT ((0)) NOT NULL,
    [posDescription] NVARCHAR (500)     NULL,
    [Created]        [dbo].[uCreated]   NULL,
    [CreatedBy]      [dbo].[uCreatedBy] NULL,
    [Updated]        [dbo].[uCreated]   NULL,
    [UpdatedBy]      [dbo].[uCreatedBy] NULL,
    [rowguid]        UNIQUEIDENTIFIER   CONSTRAINT [DF_dbPosition_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbPosition] PRIMARY KEY CLUSTERED ([posID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [IX_dbPosition_Code] UNIQUE NONCLUSTERED ([posCode] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbPosition_rowguid]
    ON [dbo].[dbPosition]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPosition] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPosition] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPosition] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPosition] TO [OMSApplicationRole]
    AS [dbo];

