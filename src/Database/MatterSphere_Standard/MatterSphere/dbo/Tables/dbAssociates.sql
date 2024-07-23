CREATE TABLE [dbo].[dbAssociates] (
    [assocID]                BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [fileID]                 BIGINT              NOT NULL,
    [contID]                 BIGINT              NOT NULL,
    [assocDefAddSetting]     BIT                 CONSTRAINT [DF_dbAssociates_assocUseClientAdd] DEFAULT ((0)) NOT NULL,
    [assocdefaultaddID]      BIGINT              NULL,
    [assocEmployeeID]        BIGINT              NULL,
    [assocOrder]             SMALLINT            CONSTRAINT [DF_dbAssociates_assocOrder] DEFAULT ((0)) NOT NULL,
    [assocType]              [dbo].[uCodeLookup] NOT NULL,
    [assocHeading]           NVARCHAR (255)      NULL,
    [assocSalut]             NVARCHAR (100)      NULL,
    [assocAddressee]         NVARCHAR (100)      NULL,
    [assocRef]               NVARCHAR (50)       NULL,
    [assocNotes]             NVARCHAR (200)      NULL,
    [assocUseDX]             BIT                 CONSTRAINT [DF_dbAssociates_assocUseDX] DEFAULT ((0)) NOT NULL,
    [assocActive]            BIT                 CONSTRAINT [DF_dbAssociates_assocActive] DEFAULT ((1)) NOT NULL,
    [assocDDI]               [dbo].[uTelephone]  NULL,
    [assocFax]               [dbo].[uTelephone]  NULL,
    [assocEmail]             [dbo].[uEmail]      NULL,
    [assocMobile]            [dbo].[uTelephone]  NULL,
    [Created]                [dbo].[uCreated]    CONSTRAINT [DF_dbAssociates_Created] DEFAULT (getutcdate()) NULL,
    [CreatedBy]              [dbo].[uCreatedBy]  NULL,
    [Updated]                [dbo].[uCreated]    NULL,
    [UpdatedBy]              [dbo].[uCreatedBy]  NULL,
    [rowguid]                UNIQUEIDENTIFIER    CONSTRAINT [DF_dbAssociates_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [assocPrefContactMethod] [dbo].[uCodeLookup] NULL,
    CONSTRAINT [PK_dbAssociates] PRIMARY KEY NONCLUSTERED ([assocID] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup],
    CONSTRAINT [FK_dbAssociates_dbAssociateType] FOREIGN KEY ([assocType]) REFERENCES [dbo].[dbAssociateType] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbAssociates_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbAssociates_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION
);




GO
CREATE CLUSTERED INDEX [IX_dbAssociates_fileID]
    ON [dbo].[dbAssociates]([fileID] ASC) WITH (FILLFACTOR = 90, PAD_INDEX = ON);


GO
CREATE NONCLUSTERED INDEX [IX_dbAssociates_ContID]
    ON [dbo].[dbAssociates]([contID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbAssociates_rowguid]
    ON [dbo].[dbAssociates]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


-- =============================================
-- Author:		FWBS Ltd
-- Create date: 
-- Description:	Keeps the newly added column DefaultAssociateID in the dbFile table synchronised with the associate table
-- =============================================
CREATE TRIGGER [dbo].[tgr_dbAssociate_dbFile_DefaultAssociate] 
   ON  [dbo].[dbAssociates] 
   AFTER INSERT,DELETE,UPDATE
	NOT FOR REPLICATION
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets FROM
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	with AssociatesRankedByAssocOrder AS	
	(
		SELECT 
			fileid, associd, rank() over (partition BY a.fileid ORDER BY a.assocOrder ASC) AS AssociateOrder
		FROM
			[dbo].dbassociates a	
			WHERE a.assocactive = 1
			AND
			( EXISTS (SELECT 1 FROM INSERTED WHERE fileid = a.fileid) OR EXISTS (SELECT 1 FROM DELETED WHERE fileid = a.fileid))
	)

	,DistinctRankedAssociates AS -- Get a distinct assoicate for each file (in some cases there IS more than 1 associate with assocOrder 0 for each file)
	(
		SELECT 
			fileid, associd, rank() over (partition BY a.fileid ORDER BY a.assocID ASC) AS AssociateOrder
		FROM
			AssociatesRankedByAssocOrder a
			WHERE a.associateOrder = 1	
		)
		, AlteredFiles AS
		(
		SELECT 
			fileid
		FROM
			[dbo].dbDocument
		GROUP BY
			fileid	
	)


UPDATE [dbo].dbfile SET DefaultAssociateID = a.associd FROM
  [dbo].dbfile f JOIN AlteredFiles af ON af.fileid = f.fileid JOIN DistinctRankedAssociates a ON f.fileid = a.fileid 
WHERE ( (DefaultAssociateID <> a.associd) OR (a.associd IS NULL AND DefaultAssociateID IS NOT NULL) OR ( a.associd IS NOT NULL AND DefaultAssociateID IS NULL) ) AND  a.associateOrder = 1
		
END


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbAssociates] TO [OMSApplicationRole]
    AS [dbo];

