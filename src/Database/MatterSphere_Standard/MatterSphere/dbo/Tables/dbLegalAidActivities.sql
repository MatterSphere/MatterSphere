CREATE TABLE [dbo].[dbLegalAidActivities] (
    [ActivityCode]          [dbo].[uCodeLookup] NOT NULL,
    [ActivityLegalAidCat]   SMALLINT            NOT NULL,
    [ActivityLegalAidGrade] TINYINT             NOT NULL,
    [ActivityCharge]        MONEY               CONSTRAINT [DF_dbLegalAidActivities_ActivityCharge] DEFAULT ((0)) NOT NULL,
    [ActivityCurISOCode]    CHAR (3)            CONSTRAINT [DF_dbLegalAidActivities_ActivityCurID] DEFAULT ((1)) NULL,
    [rowguid]               UNIQUEIDENTIFIER    CONSTRAINT [DF_dbLegalAidActivities_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbLegalAidActivities] PRIMARY KEY CLUSTERED ([ActivityCode] ASC, [ActivityLegalAidCat] ASC, [ActivityLegalAidGrade] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbLegalAidActivities_dbActivities] FOREIGN KEY ([ActivityCode]) REFERENCES [dbo].[dbActivities] ([actCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbLegalAidActivities_dbLegalAidCategory] FOREIGN KEY ([ActivityLegalAidCat]) REFERENCES [dbo].[dbLegalAidCategory] ([LegAidCategory]) ON UPDATE CASCADE NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbLegalAidActivities_rowguid]
    ON [dbo].[dbLegalAidActivities]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbLegalAidActivities] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbLegalAidActivities] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbLegalAidActivities] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbLegalAidActivities] TO [OMSApplicationRole]
    AS [dbo];

