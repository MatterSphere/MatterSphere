CREATE TABLE [dbo].[dbCountry] (
    [ctryID]            [dbo].[uCountry]    NOT NULL,
    [ctryCode]          [dbo].[uCodeLookup] NOT NULL,
    [ctryAddressFormat] [dbo].[uXML]        NULL,
    [ctryIgnore]        BIT                 CONSTRAINT [DF_dbCountry_ctryAddress] DEFAULT ((0)) NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER    CONSTRAINT [DF_dbCountry_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbCountry] PRIMARY KEY CLUSTERED ([ctryID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbCountry_rowguid]
    ON [dbo].[dbCountry]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbCountry] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbCountry] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbCountry] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbCountry] TO [OMSApplicationRole]
    AS [dbo];

