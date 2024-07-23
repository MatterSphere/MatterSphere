CREATE TABLE [dbo].[dbCommandBarControl] (
    [ctrlID]         INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ctrlCommandBar] [dbo].[uCodeLookup] NOT NULL,
    [ctrlCode]       [dbo].[uCodeLookup] NOT NULL,
    [ctrlFilter]     NVARCHAR (255)      CONSTRAINT [DF_dbCommandBarControl_ctrlFilter] DEFAULT (N'*') NOT NULL,
    [ctrlOrder]      INT                 NOT NULL,
    [ctrlLevel]      TINYINT             CONSTRAINT [DF_dbCommandBarControl_ctrlLevel] DEFAULT ((0)) NOT NULL,
    [ctrlParent]     [dbo].[uCodeLookup] NULL,
    [ctrlType]       VARCHAR (30)        CONSTRAINT [DF_dbCommandBarControl_ctrlType] DEFAULT ('msoControlButton') NOT NULL,
    [ctrlBeginGroup] BIT                 CONSTRAINT [DF_dbCommandBar_ctrlBeginGroup] DEFAULT ((0)) NOT NULL,
    [ctrlIcon]       INT                 NULL,
    [ctrlHide]       BIT                 CONSTRAINT [DF_dbCommandBarControl_ctrlHide] DEFAULT ((0)) NOT NULL,
    [ctrlRunCommand] NVARCHAR (75)       NULL,
    [ctrlIncFav]     BIT                 CONSTRAINT [DF_dbCommandBarControl_ctrlIncFav] DEFAULT ((0)) NOT NULL,
    [ctrlRole]       NVARCHAR (150)      NULL,
    [ctrlCondition]  NVARCHAR (150)      NULL,
    [ctrlLicense]    NVARCHAR (150)      NULL,
    [rowguid]        UNIQUEIDENTIFIER    CONSTRAINT [DF_dbCommandBarControl_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbCommandBarControl] PRIMARY KEY CLUSTERED ([ctrlCommandBar] ASC, [ctrlCode] ASC, [ctrlFilter] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbCommandBarControl_dbCommandBar] FOREIGN KEY ([ctrlCommandBar]) REFERENCES [dbo].[dbCommandBar] ([cbCode]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbCommandBarControl_rowguid]
    ON [dbo].[dbCommandBarControl]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrIncCommandBarVersion] ON [dbo].[dbCommandBarControl]
FOR INSERT, UPDATE, DELETE  NOT FOR REPLICATION
AS
update dbcommandbar
set cbversion = cbversion + 1 
where cbcode in
	(select distinct ctrlcommandbar from inserted I)

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbCommandBarControl] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbCommandBarControl] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbCommandBarControl] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbCommandBarControl] TO [OMSApplicationRole]
    AS [dbo];

