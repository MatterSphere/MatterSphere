CREATE TABLE [dbo].[dbAddress] (
    [addID]       BIGINT               IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [addLine1]    [dbo].[uAddressLine] NULL,
    [addLine2]    [dbo].[uAddressLine] NULL,
    [addLine3]    [dbo].[uAddressLine] NULL,
    [addLine4]    [dbo].[uAddressLine] NULL,
    [addLine5]    [dbo].[uAddressLine] NULL,
    [addPostcode] [dbo].[uPostcode]    NULL,
    [addCountry]  [dbo].[uCountry]     NULL,
    [addDXCode]   NVARCHAR (80)        NULL,
    [Created]     [dbo].[uCreated]     CONSTRAINT [DF_dbAddress_Created] DEFAULT (getutcdate()) NULL,
    [CreatedBy]   [dbo].[uCreatedBy]   NULL,
    [Updated]     [dbo].[uCreated]     CONSTRAINT [DF_dbAddress_Updated] DEFAULT (getutcdate()) NULL,
    [UpdatedBy]   [dbo].[uCreatedBy]   NULL,
    [addExtTxtID] NVARCHAR (20)        NULL,
    [rowguid]     UNIQUEIDENTIFIER     CONSTRAINT [DF_dbAddress_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbAddress] PRIMARY KEY CLUSTERED ([addID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbAddress_dbCountry] FOREIGN KEY ([addCountry]) REFERENCES [dbo].[dbCountry] ([ctryID]) NOT FOR REPLICATION
);




GO
CREATE NONCLUSTERED INDEX [IX_dbAddress_addExtTxtID]
    ON [dbo].[dbAddress]([addExtTxtID] ASC)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbAddress_addline1]
    ON [dbo].[dbAddress]([addLine1] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbAddress_addpostcode]
    ON [dbo].[dbAddress]([addPostcode] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbAddress_rowguid]
    ON [dbo].[dbAddress]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrUpdateToAddress] ON [dbo].[dbAddress]
FOR UPDATE NOT FOR REPLICATION

AS
SET NOCOUNT ON
BEGIN
	--Update Client clNeedExport where the address has changed the client default contact
	UPDATE 
		CL
	SET CL.clNeedExport = 1
	FROM 
		[dbo].[dbClient] CL 
	JOIN
		[dbo].[dbContact] C ON C.contID = CL.clDefaultContact
	JOIN
		[dbo].[dbContactAddresses] CA ON CA.contID = C.contID
	JOIN
		[Inserted] I ON I.addID = CA.contAddID

	--Update Client clNeedExport where the address has changed where client has assigned address
	UPDATE CL
	SET CL.clNeedExport = 1
	FROM 
		[dbo].[dbClient] CL
	JOIN
		[Inserted] I ON I.addID = CL.clDefaultAddress
END

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbAddress] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbAddress] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbAddress] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbAddress] TO [OMSApplicationRole]
    AS [dbo];

