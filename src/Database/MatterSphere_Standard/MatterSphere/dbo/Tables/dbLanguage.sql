CREATE TABLE [dbo].[dbLanguage] (
    [langCode]      [dbo].[uUICultureInfo] NOT NULL,
    [langDesc]      NVARCHAR (50)          NULL,
    [langSupported] BIT                    CONSTRAINT [DF_dbLanguage_langSupported] DEFAULT ((0)) NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER       CONSTRAINT [DF_dbLanguage_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbLanguageCode] PRIMARY KEY CLUSTERED ([langCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbLanguage_rowguid]
    ON [dbo].[dbLanguage]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbLanguage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbLanguage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbLanguage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbLanguage] TO [OMSApplicationRole]
    AS [dbo];

