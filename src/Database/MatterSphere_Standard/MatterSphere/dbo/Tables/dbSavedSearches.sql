CREATE TABLE [dbo].[dbSavedSearches] (
    [ssID]            BIGINT             IDENTITY (1, 1) NOT NULL,
    [ssType]          NVARCHAR (15)      NOT NULL,
    [ssOMSObjectCode] NVARCHAR (15)      NOT NULL,
    [ssName]          NVARCHAR (100)     NOT NULL,
    [ssObject]        NVARCHAR (15)      NOT NULL,
    [ssObjectID]      BIGINT             NULL,
    [ssCriteriaXML]   [dbo].[uXML]       NOT NULL,
    [Created]         [dbo].[uCreated]   NOT NULL,
    [CreatedBy]       [dbo].[uCreatedBy] NOT NULL,
    [Updated]         [dbo].[uCreated]   NULL,
    [UpdatedBy]       [dbo].[uCreatedBy] NULL,
    [rowguid]         UNIQUEIDENTIFIER   CONSTRAINT [DF_dbSavedSearches_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [ssForcedSave]    BIT                CONSTRAINT [DF_dbSavedSearches_FSD] DEFAULT ((0)) NOT NULL,
    [ssGlobalSave]    BIT                CONSTRAINT [DF_dbSavedSearches_GSD] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_dbSavedSearches] PRIMARY KEY CLUSTERED ([ssID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbSavedSearches] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbSavedSearches] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbSavedSearches] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbSavedSearches] TO [OMSApplicationRole]
    AS [dbo];

