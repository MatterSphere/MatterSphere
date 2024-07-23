CREATE TABLE [dbo].[dbFields] (
    [fldID]       INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [fldType]     NVARCHAR (50)       NOT NULL,
    [fldCode]     [dbo].[uCodeLookup] NOT NULL,
    [fldGroup]    NVARCHAR (50)       NULL,
    [fldExtended] NVARCHAR (50)       NULL,
    [fldName]     NVARCHAR (50)       NOT NULL,
    [fldAlias]    NVARCHAR (50)       NULL,
    [fldOld]      NVARCHAR (50)       NULL,
    [fldExclude]  BIT                 CONSTRAINT [DF_dbFields_fldExclude] DEFAULT ((0)) NOT NULL,
    [fldHide]     BIT                 CONSTRAINT [DF_dbFields_fldHide] DEFAULT ((0)) NOT NULL,
    [fldCommon]   BIT                 CONSTRAINT [DF_dbFields_fldCommon] DEFAULT ((0)) NOT NULL,
    [fldFormat]   NVARCHAR (50)       NULL,
    [fldLookup]   [dbo].[uCodeLookup] NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFields_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbFields] PRIMARY KEY CLUSTERED ([fldID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFields_rowguid]
    ON [dbo].[dbFields]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrdbFieldsUpdated]
   ON  [dbo].[dbFields] 
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	exec sprTableMonitorUpdate 'dbFields'    
END



GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFields] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFields] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFields] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFields] TO [OMSApplicationRole]
    AS [dbo];

