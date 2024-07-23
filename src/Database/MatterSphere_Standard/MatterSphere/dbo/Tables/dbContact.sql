CREATE TABLE [dbo].[dbContact] (
    [contID]                BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [contIsClient]          BIT                 CONSTRAINT [DF_dbContact_contIsClient] DEFAULT ((0)) NOT NULL,
    [contGuid]              UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContact_contGuid] DEFAULT (newid()) NOT NULL,
    [contGroup]             [dbo].[uCodeLookup] NULL,
    [contTypeCode]          [dbo].[uCodeLookup] NOT NULL,
    [contName]              NVARCHAR (128)      NOT NULL,
    [contDefaultAddress]    BIGINT              NOT NULL,
    [contSalut]             NVARCHAR (50)       NULL,
    [contAddressee]         NVARCHAR (50)       NULL,
    [contAddFilter]         [dbo].[uCodeLookup] NULL,
    [contAddInfo]           NVARCHAR (80)       NULL,
    [contNotes]             NVARCHAR (4000)     NULL,
    [contApproved]          BIT                 CONSTRAINT [DF_dbContact_contApproved] DEFAULT ((1)) NOT NULL,
    [contApprRevokedOn]     DATETIME            NULL,
    [contXMASCard]          BIT                 CONSTRAINT [DF_dbContact_contXMASCard] DEFAULT ((0)) NOT NULL,
    [contWebsite]           [dbo].[uFilePath]   NULL,
    [contDefSecSetting]     VARCHAR (12)        NULL,
    [contGrade]             TINYINT             CONSTRAINT [DF_dbContact_contGrade] DEFAULT ((0)) NOT NULL,
    [contSource]            [dbo].[uCodeLookup] NULL,
    [contLARef]             NVARCHAR (50)       NULL,
    [contIdent]             [dbo].[uCodeLookup] NULL,
    [contIdent2]            [dbo].[uCodeLookup] NULL,
    [contPrecLibrary]       [dbo].[uCodeLookup] NULL,
    [Created]               [dbo].[uCreated]    NULL,
    [CreatedBy]             [dbo].[uCreatedBy]  NULL,
    [Updated]               [dbo].[uCreated]    NULL,
    [UpdatedBy]             [dbo].[uCreatedBy]  NULL,
    [contExtTxtID]          NVARCHAR (36)       NULL,
    [rowguid]               UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContact_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [contShortName]         NVARCHAR (80)       NULL,
    [contMarketable]        BIT                 NULL,
    [SecurityOptions]       BIGINT              DEFAULT ((0)) NOT NULL,
    [contPrefContactMethod] [dbo].[uCodeLookup] NULL,
    [UserID]                INT                 NULL,
    CONSTRAINT [PK_dbContact] PRIMARY KEY CLUSTERED ([contID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbContact_dbAddress] FOREIGN KEY ([contDefaultAddress]) REFERENCES [dbo].[dbAddress] ([addID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbContact_dbContactType] FOREIGN KEY ([contTypeCode]) REFERENCES [dbo].[dbContactType] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbContact_dbUser] FOREIGN KEY ([UserID]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION,
    CONSTRAINT [IX_dbContact_guid] UNIQUE NONCLUSTERED ([contGuid] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE NONCLUSTERED INDEX [IX_dbContact_contExtTxtID]
    ON [dbo].[dbContact]([contExtTxtID] ASC)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContact_rowguid]
    ON [dbo].[dbContact]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrUpdateToContDefAddress] ON [dbo].[dbContact]
FOR UPDATE NOT FOR REPLICATION
AS
SET NOCOUNT ON
BEGIN
	UPDATE 
		CL
	SET 
		CL.clNeedExport = 1
	FROM 
		[dbo].[dbContact] C
	JOIN 
		[dbo].[dbClient] CL ON C.contID = CL.clDefaultContact
	JOIN	
		[Inserted] I ON I.contDefaultAddress = C.contDefaultAddress
	JOIN
		[Deleted] D ON D.contID = I.contID
	WHERE 
		I.contdefaultAddress <> D.contDefaultAddress AND I.contID = D.contID
END

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContact] TO [OMSApplicationRole]
    AS [dbo];

