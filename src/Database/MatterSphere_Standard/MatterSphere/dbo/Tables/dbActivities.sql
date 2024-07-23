CREATE TABLE [dbo].[dbActivities] (
    [actCode]           [dbo].[uCodeLookup] NOT NULL,
    [actAccCode]        NVARCHAR (16)       NULL,
    [actChargeable]     BIT                 CONSTRAINT [DF_dbActivities_ActivityChargeable] DEFAULT ((1)) NOT NULL,
    [actFixedRateLegal] BIT                 CONSTRAINT [DF_dbActivities_ActivityFixedRate] DEFAULT ((0)) NOT NULL,
    [actTemplateMatch]  NVARCHAR (150)      NULL,
    [actLegalAidFilter] NVARCHAR (50)       NULL,
    [actFixedValue]     MONEY               NULL,
    [actActive]         BIT                 CONSTRAINT [DF_dbActivities_ActivityActive] DEFAULT ((1)) NOT NULL,
    [actcurISOCode]     CHAR (3)            NULL,
    [actfileTypecode]   [dbo].[uCodeLookup] NULL,
    [rowguid]           UNIQUEIDENTIFIER    CONSTRAINT [DF_dbActivities_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [actBillable]       BIT                 CONSTRAINT [DF_dbActivities_actBillable] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_dbActivities] PRIMARY KEY CLUSTERED ([actCode] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbActivities_dbCurrency] FOREIGN KEY ([actcurISOCode]) REFERENCES [dbo].[dbCurrency] ([curISOCode]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbActivities_rowguid]
    ON [dbo].[dbActivities]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbActivities] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbActivities] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbActivities] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbActivities] TO [OMSApplicationRole]
    AS [dbo];

