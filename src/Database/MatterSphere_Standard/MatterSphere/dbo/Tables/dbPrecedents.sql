CREATE TABLE [dbo].[dbPrecedents] (
    [PrecID]              BIGINT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [brID]                INT                    NULL,
    [PrecTitle]           NVARCHAR (50)          NOT NULL,
    [PrecType]            [dbo].[uCodeLookup]    NOT NULL,
    [PrecLibrary]         [dbo].[uCodeLookup]    NULL,
    [PrecCategory]        [dbo].[uCodeLookup]    NULL,
    [PrecSubCategory]     [dbo].[uCodeLookup]    NULL,
	[PrecMinorCategory]   [dbo].[uCodeLookup]    NULL,
    [PrecLanguage]        [dbo].[uUICultureInfo] NULL,
    [PrecText]            BIT                    CONSTRAINT [DF_dbPrecedents_PrecText] DEFAULT ((0)) NOT NULL,
    [PrecDesc]            NVARCHAR (100)         NOT NULL,
    [PrecPubName]         NVARCHAR (50)          NULL,
    [PrecAddressee]       [dbo].[uCodeLookup]    NULL,
    [PrecAssocType]       [dbo].[uCodeLookup]    NULL,
    [PrecAutoName]        NVARCHAR (50)          NULL,
    [PrecPath]            NVARCHAR (200)         NULL,
    [PrecLocation]        SMALLINT               NULL,
    [PrecDirID]           SMALLINT               NULL,
    [PrecReminder]        BIT                    CONSTRAINT [DF_dbPrecedents_PrecReminder] DEFAULT ((0)) NOT NULL,
    [PrecNoOfDays]        SMALLINT               CONSTRAINT [DF_dbPrecedents_PrecNoOfDays] DEFAULT ((-1)) NOT NULL,
    [PrecRemComment]      NVARCHAR (60)          NULL,
    [PrecReviewNoOfDays]  SMALLINT               CONSTRAINT [DF_dbPrecedents_PrecReviewNoDays] DEFAULT ((-1)) NULL,
    [PrecTimeRecUnits]    INT                    CONSTRAINT [DF_dbPrecedents_PrecTimeRecUnits] DEFAULT ((0)) NOT NULL,
    [PrecTimeRecDesc]     NVARCHAR (50)          NULL,
    [PrecTimeRecCode]     [dbo].[uCodeLookup]    NULL,
    [precTimeRecInCode]   [dbo].[uCodeLookup]    NULL,
    [PrecInternalProcess] BIT                    CONSTRAINT [DF_dbPrecedents_PrecInternalProcess] DEFAULT ((0)) NOT NULL,
    [PrecProgType]        SMALLINT               NULL,
    [PrecMS]              SMALLINT               CONSTRAINT [DF_dbPrecedents_PrecMS] DEFAULT ((0)) NOT NULL,
    [PrecMoveMS]          BIT                    CONSTRAINT [DF_dbPrecedents_PrecMoveMS] DEFAULT ((0)) NOT NULL,
    [PrecMSChangePrompt]  [dbo].[uCodeLookup]    NULL,
    [PrecMSNote]          NVARCHAR (MAX)         NULL,
    [PrecMSAuto]          BIT                    CONSTRAINT [DF_dbPrecedents_PrecMSAuto] DEFAULT ((0)) NOT NULL,
    [PrecMSCode]          [dbo].[uCodeLookup]    NULL,
    [PrecGlossary]        NVARCHAR (MAX)         NULL,
    [PrecSMSMessage]      NVARCHAR (150)         NULL,
    [PrecUpdateSQL]       NVARCHAR (MAX)         NULL,
    [PrecMultiPrec]       BIT                    CONSTRAINT [DF_dbPrecedents_PrecMultiPrec] DEFAULT ((0)) NOT NULL,
    [PrecPassword]        NVARCHAR (25)          NULL,
    [PrecPasswordHint]    NVARCHAR (75)          NULL,
    [PrecScript]          [dbo].[uCodeLookup]    NULL,
    [PrecPreview]         NVARCHAR (MAX)         NULL,
    [precExtension]       NVARCHAR (15)          NULL,
    [precDirection]       BIT                    CONSTRAINT [DF__dbprecedents_precDirection] DEFAULT ((0)) NOT NULL,
    [precXML]             [dbo].[uXML]           CONSTRAINT [DF_dbPrecedents_precXML] DEFAULT (N'<config/>') NOT NULL,
    [Created]             [dbo].[uCreated]       CONSTRAINT [DF_dbPrecedents_Created] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]           [dbo].[uCreatedBy]     NULL,
    [Updated]             [dbo].[uCreated]       CONSTRAINT [DF_dbPrecedents_Updated] DEFAULT (getutcdate()) NULL,
    [UpdatedBy]           [dbo].[uCreatedBy]     NULL,
    [precDeleted]         BIT                    CONSTRAINT [DF_dbPrecedents_precDeleted] DEFAULT ((0)) NOT NULL,
    [precRetain]          DATETIME               NULL,
    [rowguid]             UNIQUEIDENTIFIER       CONSTRAINT [DF_dbPrecedents_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [precCurrentVersion]  UNIQUEIDENTIFIER       NULL,
    [precChecksum]        VARCHAR (50)           NULL,
    [precCheckedOut]      DATETIME            NULL,
    [precCheckedOutBy]    INT                 NULL,
    [precCheckedOutlocation] NVARCHAR (255)      NULL,
    CONSTRAINT [PK_dbPrecedents] PRIMARY KEY CLUSTERED ([PrecID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbPrecedents_dbActivities] FOREIGN KEY ([PrecTimeRecCode]) REFERENCES [dbo].[dbActivities] ([actCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedents_dbActivities1] FOREIGN KEY ([precTimeRecInCode]) REFERENCES [dbo].[dbActivities] ([actCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedents_dbApplication] FOREIGN KEY ([PrecProgType]) REFERENCES [dbo].[dbApplication] ([appID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedents_dbBranch] FOREIGN KEY ([brID]) REFERENCES [dbo].[dbBranch] ([brID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedents_dbContactType] FOREIGN KEY ([PrecAddressee]) REFERENCES [dbo].[dbContactType] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedents_dbDirectory] FOREIGN KEY ([PrecDirID]) REFERENCES [dbo].[dbDirectory] ([dirID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedents_dbDocumentType] FOREIGN KEY ([PrecType]) REFERENCES [dbo].[dbDocumentType] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedents_dbLanguage] FOREIGN KEY ([PrecLanguage]) REFERENCES [dbo].[dbLanguage] ([langCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedents_dbStorageProvider] FOREIGN KEY ([PrecLocation]) REFERENCES [dbo].[dbStorageProvider] ([spID]) NOT FOR REPLICATION,
    CONSTRAINT [IX_dbPrecedents_Combi] UNIQUE NONCLUSTERED ([PrecTitle] ASC, [PrecType] ASC, [PrecLibrary] ASC, [PrecCategory] ASC, [PrecSubCategory] ASC, [PrecLanguage] ASC, [PrecMinorCategory] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbPrecedents_rowguid]
    ON [dbo].[dbPrecedents]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbPrecedents_type]
    ON [dbo].[dbPrecedents]([PrecType] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrDefaultPrecedentInfo] ON [dbo].[dbPrecedents] 
FOR INSERT  NOT FOR REPLICATION
AS

declare @id bigint
declare @type ucodelookup
declare @ext nvarchar(15)
declare @loc smallint
declare @app smallint

select @id = precid, @type = prectype, @ext = precextension, @loc = preclocation, @app = precprogtype from inserted

if @ext is null or @loc is null or @app is null or @ext = ''
begin
	declare @newext nvarchar(15)
	declare @newloc smallint
	select @newext = typeprecext, @newloc =  typedefaultstorage, @app = typedefaultapp from dbdocumenttype where typecode = @type
	update dbprecedents set precextension = coalesce(precextension, @newext), preclocation = coalesce(preclocation, @newloc), precprogtype = coalesce(precprogtype, @app)  where precid = @id
end

GO


CREATE TRIGGER [dbo].[tgrPrecedentCategoryRebuild] ON [dbo].[dbPrecedents] 
FOR INSERT, UPDATE, DELETE  NOT FOR REPLICATION
AS

if update(preclibrary) or update(preccategory) or update(precsubcategory)
begin
	if (select object_id(N'[tempdb]..[##omsPrecedentLibrary]')) is not null
	begin
		drop table ##omsPrecedentLibrary
	end
	
	if (select object_id(N'[tempdb]..[##omsPrecedentCategory]')) is not null
	begin
		drop table ##omsPrecedentCategory
	end

	if (select object_id(N'[tempdb]..[##omsPrecedentSubCategory]')) is not null
	begin
		drop table ##omsPrecedentSubCategory
	end

end

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPrecedents] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPrecedents] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPrecedents] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPrecedents] TO [OMSApplicationRole]
    AS [dbo];

