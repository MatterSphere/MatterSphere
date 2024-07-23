CREATE TABLE [dbo].[dbNewAddressRequest] (
    [addID]         BIGINT               IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [addExistingId] BIGINT               CONSTRAINT [DF_dbNewAddressRequest_addExistingId] DEFAULT ((0)) NOT NULL,
    [addLine1]      [dbo].[uAddressLine] NULL,
    [addLine2]      [dbo].[uAddressLine] NULL,
    [addLine3]      [dbo].[uAddressLine] NULL,
    [addLine4]      [dbo].[uAddressLine] NULL,
    [addLine5]      [dbo].[uAddressLine] NULL,
    [addPostcode]   [dbo].[uPostcode]    NULL,
    [addCountry]    [dbo].[uCountry]     NULL,
    [addDXCode]     NVARCHAR (80)        NULL,
    [Created]       [dbo].[uCreated]     CONSTRAINT [DF_dbNewAddressRequest_Created] DEFAULT (getutcdate()) NULL,
    [CreatedBy]     [dbo].[uCreatedBy]   NULL,
    [Updated]       [dbo].[uCreated]     CONSTRAINT [DF_dbNewAddressRequest_Updated] DEFAULT (getutcdate()) NULL,
    [UpdatedBy]     [dbo].[uCreatedBy]   NULL,
    [rowguid]       UNIQUEIDENTIFIER     CONSTRAINT [DF_dbNewAddressRequest_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [addExtTxtID]   NVARCHAR (20)        NULL,
    CONSTRAINT [PK_dbNewAddressRequest] PRIMARY KEY CLUSTERED ([addID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbNewAddressRequest_dbCountry] FOREIGN KEY ([addCountry]) REFERENCES [dbo].[dbCountry] ([ctryID]) NOT FOR REPLICATION
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbNewAddressRequest] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbNewAddressRequest] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbNewAddressRequest] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbNewAddressRequest] TO [OMSApplicationRole]
    AS [dbo];

