CREATE TABLE [dbo].[dbSpecificData] (
    [spLookup] NVARCHAR (30)    NOT NULL,
    [brID]     INT              NOT NULL,
    [spData]   NVARCHAR (255)   NULL,
    [rowguid]  UNIQUEIDENTIFIER CONSTRAINT [DF_dbSpecificData_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbSpecificData] PRIMARY KEY CLUSTERED ([spLookup] ASC, [brID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [CK_SpecificDataBranchCheck] CHECK ([DBO].[CheckBranchForSpecificData]()=(1))
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbSpecificData_rowguid]
    ON [dbo].[dbSpecificData]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbSpecificData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbSpecificData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbSpecificData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbSpecificData] TO [OMSApplicationRole]
    AS [dbo];

