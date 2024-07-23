CREATE TABLE [dbo].[dbClient] (
    [clID]                 BIGINT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [clNo]                 NVARCHAR (12)          NULL,
    [cltypeCode]           [dbo].[uCodeLookup]    NOT NULL,
    [clGroup]              [dbo].[uCodeLookup]    NULL,
    [clextID]              INT                    NULL,
    [clexttxtID]           NVARCHAR (36)          NULL,
    [clguid]               UNIQUEIDENTIFIER       CONSTRAINT [DF_dbClient_clguid] DEFAULT (newid()) NULL,
    [clName]               NVARCHAR (128)         NOT NULL,
    [clAccCode]            NVARCHAR (30)          NULL,
    [clSearch1]            NVARCHAR (50)          NULL,
    [clSearch2]            NVARCHAR (50)          NULL,
    [clSearch3]            NVARCHAR (50)          NULL,
    [clSearch4]            NVARCHAR (50)          NULL,
    [clSearch5]            NVARCHAR (50)          NULL,
    [clMarket]             BIT                    CONSTRAINT [DF_DBClient_clMarket] DEFAULT ((0)) NOT NULL,
    [clSource]             [dbo].[uCodeLookup]    NULL,
    [brID]                 INT                    NULL,
    [feeusrID]             INT                    NULL,
    [clNotes]              NVARCHAR (MAX)         NULL,
    [clAddInfo]            NVARCHAR (100)         NULL,
    [clPreClient]          BIT                    CONSTRAINT [DF_DBClient_clPreClient] DEFAULT ((0)) NOT NULL,
    [clPreClientConvDate]  DATETIME               NULL,
    [clNeedExport]         BIT                    CONSTRAINT [DF_DBClient_clNeedExport] DEFAULT ((1)) NOT NULL,
    [clStop]               BIT                    CONSTRAINT [DF_DBClient_clStop] DEFAULT ((0)) NOT NULL,
    [clStopReason]         NVARCHAR (150)         NULL,
    [clActiveComplaints]   SMALLINT               CONSTRAINT [DF_dbClient_clActiveComplaints] DEFAULT ((0)) NOT NULL,
    [clActiveUndertakings] SMALLINT               CONSTRAINT [DF_dbClient_clActiveUndertakings] DEFAULT ((0)) NOT NULL,
    [clRegisterCount]      SMALLINT               CONSTRAINT [DF_dbClient_clRegisterCount] DEFAULT ((0)) NOT NULL,
    [clDefaultContact]     BIGINT                 NOT NULL,
    [clDefaultAddress]     BIGINT                 NULL,
    [clAutoCreated]        DATETIME               NULL,
    [clAutoSource]         [dbo].[uCodeLookup]    NULL,
    [clAutoType]           [dbo].[uCodeLookup]    NULL,
    [clPassword]           NVARCHAR (25)          NULL,
    [clPasswordHint]       NVARCHAR (50)          NULL,
    [clUICultureInfo]      [dbo].[uUICultureInfo] NULL,
    [clSourceContact]      BIGINT                 NULL,
    [clSourceUser]         INT                    NULL,
    [Created]              [dbo].[uCreated]       CONSTRAINT [DF_dbClient_Created] DEFAULT (getutcdate()) NULL,
    [CreatedBy]            [dbo].[uCreatedBy]     NULL,
    [Updated]              [dbo].[uCreated]       CONSTRAINT [DF_dbClient_Updated] DEFAULT (getutcdate()) NULL,
    [UpdatedBy]            [dbo].[uCreatedBy]     NULL,
    [clStorageProvider]    SMALLINT               NULL,
    [clNickname]           NVARCHAR (100)         NULL,
    [rowguid]              UNIQUEIDENTIFIER       CONSTRAINT [DF_dbClient_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [clStatus]             [dbo].[uCodeLookup]    CONSTRAINT [DF_dbClient_clStatus] DEFAULT (N'OPEN') NULL,
    [SecurityOptions]      BIGINT                 DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DBClient] PRIMARY KEY CLUSTERED ([clID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbClient_dbAddress] FOREIGN KEY ([clDefaultAddress]) REFERENCES [dbo].[dbAddress] ([addID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbClient_dbClientType] FOREIGN KEY ([cltypeCode]) REFERENCES [dbo].[dbClientType] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbClient_dbContact] FOREIGN KEY ([clDefaultContact]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbClient_dbStorageProvider] FOREIGN KEY ([clStorageProvider]) REFERENCES [dbo].[dbStorageProvider] ([spID]) NOT FOR REPLICATION,
    CONSTRAINT [IX_dbClient_guid] UNIQUE NONCLUSTERED ([clguid] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbClient_clNo]
    ON [dbo].[dbClient]([clNo] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbClient_rowguid]
    ON [dbo].[dbClient]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbClient_Search1]
    ON [dbo].[dbClient]([clSearch1] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbClient_Search2]
    ON [dbo].[dbClient]([clSearch2] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbClient_Search3]
    ON [dbo].[dbClient]([clSearch3] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbClient_Search4]
    ON [dbo].[dbClient]([clSearch4] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbClient_Search5]
    ON [dbo].[dbClient]([clSearch5] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO

CREATE NONCLUSTERED INDEX [IX_dbClient_cltypeCode] ON dbo.dbClient
(
	cltypeCode ASC
) WITH (FILLFACTOR = 90)
    ON [IndexGroup];
GO


CREATE TRIGGER [dbo].[tgrClientNumberGenerator] ON [dbo].[dbClient]
FOR INSERT  NOT FOR REPLICATION
AS
declare @clid bigint
declare @cltype uCodeLookup
declare @number nvarchar(12)
declare @branch int
declare @usrid int
select @clid = clid , @number = clno, @cltype = cltypecode, @usrid = createdby , @branch = brID from inserted 


-- If the branchID is specifically set i.e. the value is not 0 or -1 set the branchID to equal that value
-- =========================================================
IF (SELECT TOP 1 regBranchConfig FROM dbRegInfo)  > 0
	SET @branch = ( SELECT TOP 1 regBranchConfig FROM dbRegInfo R JOIN dbBranch B ON B.brID = R.regBranchConfig )

-- If using site specfic database (value 0)
-- ======================
IF (SELECT TOP 1 regBranchConfig FROM dbRegInfo) = 0 OR @branch IS NULL
	set @branch = (select top 1 brid from dbreginfo)


if @number is null or @number = ''
begin
 declare @newnum nvarchar(12)
 declare @seed uCodeLookup
 declare @ret int
 
 set @seed = IsNull((select typeseed from dbclienttype where typecode = @cltype), 'CL')
 execute @ret = sprGetNextSeedNo @branch, @seed, null, @newnum output, @clid
 if @ret = 0
 BEGIN
	WHILE (EXISTS (SELECT 1 FROM config.dbClient WHERE clNo = @newnum))
	BEGIN
		execute @ret = sprGetNextSeedNo @branch, @seed, null, @newnum output, @clid
	END
	update config.dbClient set clno = @newnum where clid = @clid
 END
 else
 begin
  declare @msg nvarchar(500)
  declare @severity tinyint
  declare @UI uUICultureInfo
  set @UI  = (select usruicultureinfo from dbuser where usrid = @usrid)
  execute @severity = sprRaiseError 'MSGCLSEEDNO', @UI, @msg out
  raiserror (@msg, @severity, 1, @seed)
  rollback transaction
 end
end

GO


CREATE TRIGGER [dbo].[tgrExportClientToAccounts] ON [dbo].[dbClient]
FOR UPDATE  NOT FOR REPLICATION
AS
if not update(clneedexport)
begin
	update dbclient set clNeedExport = 1 where CLID in (select clid from inserted)
end

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbClient] TO [OMSApplicationRole]
    AS [dbo];

