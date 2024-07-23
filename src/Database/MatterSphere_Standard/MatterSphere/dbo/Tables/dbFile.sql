CREATE TABLE [dbo].[dbFile] (
    [fileID]                BIGINT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [clID]                  BIGINT                 NOT NULL,
    [fileAccCode]           NVARCHAR (30)          NULL,
    [fileNo]                NVARCHAR (20)          NOT NULL,
    [fileguid]              UNIQUEIDENTIFIER       CONSTRAINT [DF_dbFile_fileguid] DEFAULT (newid()) NOT NULL,
    [fileDesc]              NVARCHAR (255)         NOT NULL,
    [fileResponsibleID]     INT                    NULL,
    [filePrincipleID]       INT                    NULL,
    [fileManagerID]         INT                    NULL,
    [fileDepartment]        [dbo].[uCodeLookup]    NOT NULL,
    [brID]                  INT                    NULL,
    [fileType]              [dbo].[uCodeLookup]    NOT NULL,
    [fileStatus]            [dbo].[uCodeLookup]    CONSTRAINT [DF_dbMatters_fileStatus] DEFAULT (N'LIVE') NOT NULL,
    [fileClosed]            [dbo].[uCreated]       NULL,
    [fileClosedby]          [dbo].[uCreatedBy]     NULL,
    [fileReviewDate]        DATETIME               NULL,
    [fileNeedExport]        BIT                    CONSTRAINT [DF_dbMatters_MatNeedExport] DEFAULT ((1)) NOT NULL,
    [fileNotes]             NVARCHAR (MAX)         NULL,
    [fileAllowExternal]     BIT                    CONSTRAINT [DF_dbFile_fileAllowExternal] DEFAULT ((0)) NOT NULL,
    [fileExternalNotes]     NVARCHAR (MAX)         NULL,
    [fileSMSEnabled]        BIT                    CONSTRAINT [DF_dbFile_MatSMSEnabled] DEFAULT ((0)) NOT NULL,
    [filePassword]          NVARCHAR (25)          NULL,
    [filePasswordHint]      NVARCHAR (50)          NULL,
    [fileRiskAssesment]     BIT                    CONSTRAINT [DF_dbFile_fileRiskAssesment] DEFAULT ((0)) NOT NULL,
    [fileUICultureInfo]     [dbo].[uUICultureInfo] NULL,
    [fileConflictNotes]     NVARCHAR (MAX)         NULL,
    [fileConflictFound]     INT                    NULL,
    [fileConflictCheck]     BIT                    NULL,
    [fileTeam]              INT                    NULL,
    [fileOffline]           BIT                    CONSTRAINT [DF_dbFile_fileOffline] DEFAULT ((0)) NOT NULL,
    [fileSource]            [dbo].[uCodeLookup]    NULL,
    [fileSourceContact]     BIGINT                 NULL,
    [fileSourceUser]        INT                    NULL,
    [filePrecLibrary]       [dbo].[uCodeLookup]    NULL,
    [fileFundCode]          [dbo].[uCodeLookup]    NOT NULL,
    [filecurISOCode]        CHAR (3)               NOT NULL,
    [fileFundRef]           NVARCHAR (50)          NULL,
    [fileWarningPerc]       INT                    CONSTRAINT [DF_dbFile_ftWarningPerc] DEFAULT ((70)) NOT NULL,
    [fileCreditLimit]       MONEY                  CONSTRAINT [DF_dbFile_ftCreditLimit] DEFAULT ((0)) NOT NULL,
    [fileOriginalLimit]     MONEY                  CONSTRAINT [DF_dbFile_ftOriginalLimit] DEFAULT ((0)) NOT NULL,
    [fileRatePerUnit]       MONEY                  CONSTRAINT [DF_dbFile_ftRatePerUnit] DEFAULT ((0)) NOT NULL,
    [fileBanding]           INT                    CONSTRAINT [DF_dbFile_ftBanding] DEFAULT ((3)) NOT NULL,
    [fileAgreementDate]     DATETIME               NULL,
    [fileQuoteSent]         BIT                    CONSTRAINT [DF_dbFile_ftQuoteSent] DEFAULT ((0)) NOT NULL,
    [fileEstimate]          MONEY                  CONSTRAINT [DF_dbFile_ftEstimate] DEFAULT ((0)) NOT NULL,
    [fileLastEstimate]      MONEY                  CONSTRAINT [DF_dbFile_ftLastEstimate] DEFAULT ((0)) NOT NULL,
    [fileWIP]               MONEY                  CONSTRAINT [DF_dbFile_ftWIP] DEFAULT ((0)) NOT NULL,
    [fileBTD]               MONEY                  CONSTRAINT [DF_dbFile_ftBTD] DEFAULT ((0)) NOT NULL,
    [fileCost]              MONEY                  CONSTRAINT [DF_dbFile_ftCost] DEFAULT ((0)) NOT NULL,
    [fileRestrictions]      NVARCHAR (100)         NULL,
    [fileScope]             NVARCHAR (100)         NULL,
    [fileComplexity]        NVARCHAR (1)           NULL,
    [fileLACategory]        SMALLINT               NULL,
    [fileFranCode]          [dbo].[uCodeLookup]    NULL,
    [fileFundingExtension]  BIT                    CONSTRAINT [DF_dbFile_ftFundingExtension] DEFAULT ((0)) NOT NULL,
    [fileFundingExtReason]  NVARCHAR (50)          NULL,
    [fileArchiveDate]       DATETIME               NULL,
    [fileArchiveRef]        NVARCHAR (50)          NULL,
    [fileDestroyDate]       DATETIME               NULL,
    [fileStorageProvider]   SMALLINT               NULL,
    [fileSPSSite]           NVARCHAR (255)         NULL,
    [fileSPSDocWS]          NVARCHAR (255)         NULL,
    [Created]               [dbo].[uCreated]       CONSTRAINT [DF_dbFile_Created] DEFAULT (getutcdate()) NULL,
    [CreatedBy]             [dbo].[uCreatedBy]     NULL,
    [Updated]               [dbo].[uCreated]       NULL,
    [Updatedby]             [dbo].[uCreatedBy]     NULL,
    [fileExtLinkID]         BIGINT                 NULL,
    [fileExtLinkTxtID]      NVARCHAR (36)          NULL,
    [fileSMSDocWS]          NVARCHAR (255)         NULL,
    [fileNickname]          NVARCHAR (100)         NULL,
    [phID]                  BIGINT                 NULL,
    [rowguid]               UNIQUEIDENTIFIER       CONSTRAINT [DF_dbFile_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [fileAlertLevel]        SMALLINT               NULL,
    [fileAlertMessage]      NVARCHAR (150)         NULL,
    [DefaultAssociateID]    BIGINT                 NULL,
    [fileTimeActivityGroup] [dbo].[uCodeLookup]    NULL,
    [fileRTF]               NVARCHAR (255)         NULL,
    [SecurityOptions]       BIGINT                 DEFAULT ((0)) NOT NULL,
    [fileLastEstimateDate]  DATETIME               NULL,
    [fileXml]               NVARCHAR (MAX)         CONSTRAINT [DF_dbFile_fileXml] DEFAULT ('<config/>') NOT NULL,
    CONSTRAINT [PK_dbFile_fileID] PRIMARY KEY CLUSTERED ([fileID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbFile_dbBranch] FOREIGN KEY ([brID]) REFERENCES [dbo].[dbBranch] ([brID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFile_dbClient] FOREIGN KEY ([clID]) REFERENCES [dbo].[dbClient] ([clID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFile_dbContact] FOREIGN KEY ([fileSourceContact]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFile_dbCurrency] FOREIGN KEY ([filecurISOCode]) REFERENCES [dbo].[dbCurrency] ([curISOCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFile_dbFeeEarner] FOREIGN KEY ([fileResponsibleID]) REFERENCES [dbo].[dbFeeEarner] ([feeusrID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFile_dbFeeEarner1] FOREIGN KEY ([filePrincipleID]) REFERENCES [dbo].[dbFeeEarner] ([feeusrID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFile_dbFilePhase] FOREIGN KEY ([phID]) REFERENCES [dbo].[dbFilePhase] ([phID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFile_dbFileType] FOREIGN KEY ([fileType]) REFERENCES [dbo].[dbFileType] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFile_dbFundType] FOREIGN KEY ([fileFundCode], [filecurISOCode]) REFERENCES [dbo].[dbFundType] ([ftCode], [ftcurISOCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFile_dbLanguage] FOREIGN KEY ([fileUICultureInfo]) REFERENCES [dbo].[dbLanguage] ([langCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFile_dbStorageProvider] FOREIGN KEY ([fileStorageProvider]) REFERENCES [dbo].[dbStorageProvider] ([spID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFile_dbUser] FOREIGN KEY ([fileSourceUser]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION,
	CONSTRAINT [FK_dbFile_dbLegalAidCategory] FOREIGN KEY ([fileLACategory]) REFERENCES [dbo].[dbLegalAidCategory] ([LegAidCategory]) ON UPDATE CASCADE NOT FOR REPLICATION,
    CONSTRAINT [IX_dbFile_guid] UNIQUE NONCLUSTERED ([fileguid] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFile_fileID]
    ON [dbo].[dbFile]([fileID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFile_rowguid]
    ON [dbo].[dbFile]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFile_clID_fileNo]
    ON [dbo].[dbFile]([clID] ASC, [fileNo] ASC)
	INCLUDE (filestatus);


GO

CREATE NONCLUSTERED INDEX [IX_dbFile_clID_Created]
	ON [dbo].[dbFile]([clID], [Created] DESC)
	INCLUDE ([fileStatus], [fileDesc], [fileType], [phID], [fileAlertLevel], [fileAlertMessage]);
GO

CREATE NONCLUSTERED INDEX [IX_dbFile_fileLACategory]
    ON [dbo].[dbFile]([fileLACategory] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];
GO

CREATE NONCLUSTERED INDEX [IX_dbFile_filePrincipleID_fileDepartment]
	ON [dbo].[dbFile]([filePrincipleID] ASC, [fileDepartment] ASC)
	INCLUDE ([fileID], [clID], [fileNo], [fileDesc], [fileType])
	ON [IndexGroup];
GO

CREATE TRIGGER [dbo].[tgrExportFileToAccounts] ON [dbo].[dbFile]
FOR UPDATE  NOT FOR REPLICATION
AS
if not update(fileneedexport)
begin
	update dbfile set fileNeedExport = 1 where fileID in (select fileid from inserted)
end

GO


CREATE TRIGGER [dbo].[tgrFileNumberGenerator] ON [dbo].[dbFile] 
FOR INSERT  NOT FOR REPLICATION
AS
declare @clid bigint
declare @fileid bigint
declare @filetype uCodeLookup
declare @number nvarchar(20)
declare @branch int
declare @usrid int
--set @fileid = scope_identity()
select @fileid = fileid, @clid = clid, @number = fileno, @filetype = filetype, @usrid = createdby , @branch = brID from inserted --where fileid = @fileid

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
	declare @newnum nvarchar(20)
	declare @lastnum nvarchar(20)
	declare @seed uCodeLookup
	declare @ret int
	declare @ctr int
	
	set @seed = IsNull((select typeseed from dbfiletype where typecode = @filetype), 'FI')

	-- *** NOTE ***
	--The file record has already been written so must filter the file id that has just been added.
	set @lastnum = IsNull((select top 1 fileno from config.dbFile where clid = @clid and fileid <> @fileid order by created desc), 'N/A')
		
	execute @ret = sprGetNextSeedNo @branch, @seed, @lastnum, @newnum output, @fileid
	if @ret = 0
	begin
		set @ctr = 0
		while exists(select fileid from config.dbFile where clid = @clid and fileno = @newnum)
		begin
			execute @ret = sprGetNextSeedNo @branch, @seed, @newnum, @newnum output
			if @ret <> 0 break
			set @ctr = @ctr + 1
			if @ctr > 1000 break
		end
		update config.dbFile set fileno = @newnum where fileid = @fileid
	end
	else
	begin
		declare @msg nvarchar(500)
		declare @severity tinyint
		declare @UI uUICultureInfo
		set @UI  = (select usruicultureinfo from dbuser where usrid = @usrid)
		execute @severity = sprRaiseError 'MSGFILESEEDNO', @UI, @msg out
		raiserror (@msg, @severity, 1, @seed)
		rollback transaction
	end
end

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFile] TO [OMSApplicationRole]
    AS [dbo];

