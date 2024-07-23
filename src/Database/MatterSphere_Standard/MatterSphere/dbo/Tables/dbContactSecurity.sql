CREATE TABLE [dbo].[dbContactSecurity] (
    [contID]     BIGINT           NOT NULL,
    [ImageID1]   BIGINT           NULL,
    [ImageID2]   BIGINT           NULL,
    [ImageID3]   BIGINT           NULL,
    [ImageID4]   BIGINT           NULL,
    [Question]   NVARCHAR (50)    NULL,
    [PassPhrase] NVARCHAR (50)    NULL,
    [rowguid]    UNIQUEIDENTIFIER CONSTRAINT [DF_dbContactSecurity_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbContactSecurity] PRIMARY KEY CLUSTERED ([contID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContactSecurity_rowguid]
    ON [dbo].[dbContactSecurity]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactSecurity] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactSecurity] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactSecurity] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactSecurity] TO [OMSApplicationRole]
    AS [dbo];

