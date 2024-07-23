CREATE TABLE [dbo].[dbContactNewAddressRequest] (
    [contID]          BIGINT              NOT NULL,
    [contaddID]       BIGINT              NOT NULL,
    [contCode]        [dbo].[uCodeLookup] NOT NULL,
    [contMakeDefault] BIT                 NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContactNewAddressRequest_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbContactNewAddressRequest_Code] PRIMARY KEY CLUSTERED ([contID] ASC, [contaddID] ASC, [contCode] ASC) WITH (FILLFACTOR = 90)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactNewAddressRequest] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactNewAddressRequest] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactNewAddressRequest] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactNewAddressRequest] TO [OMSApplicationRole]
    AS [dbo];

