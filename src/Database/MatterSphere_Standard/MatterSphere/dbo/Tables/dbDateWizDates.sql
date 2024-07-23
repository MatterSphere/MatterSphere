CREATE TABLE [dbo].[dbDateWizDates] (
    [typeCode]          [dbo].[uCodeLookup] NOT NULL,
    [dateCode]          [dbo].[uCodeLookup] NOT NULL,
    [dateOrder]         INT                 CONSTRAINT [DF_dbDateWizDates_dateOrder] DEFAULT ((0)) NOT NULL,
    [dateEditable]      BIT                 CONSTRAINT [DF_dbDateWizDates_dateEditable] DEFAULT ((0)) NOT NULL,
    [dateCalcFrom]      [dbo].[uCodeLookup] NULL,
    [dateUnits]         FLOAT (53)          CONSTRAINT [DF_dbDateWizDates_dateCalcDays] DEFAULT ((0)) NOT NULL,
    [dateMeasure]       CHAR (1)            CONSTRAINT [DF_dbDateWizDates_dateCalcUnit] DEFAULT ('D') NOT NULL,
    [dateKey]           BIT                 CONSTRAINT [DF_dbDateWizDates_dateKey] DEFAULT ((0)) NOT NULL,
    [dateAsTask]        BIT                 CONSTRAINT [DF_dbDateWizDates_dateAsTask] DEFAULT ((0)) NOT NULL,
    [dateAsAppointment] BIT                 CONSTRAINT [DF_dbDateWizDates_dateAsAppointment] DEFAULT ((0)) NOT NULL,
    [datectrlid]        INT                 CONSTRAINT [DF_dbDateWizDates_datectrlid] DEFAULT ((10)) NOT NULL,
    [dateFormat]        VARCHAR (50)        CONSTRAINT [DF_dbDateWizDates_dateFormat] DEFAULT ('D') NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER    CONSTRAINT [DF_dbDateWizDates_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDateWizDates] PRIMARY KEY CLUSTERED ([typeCode] ASC, [dateCode] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbDateWizDates_dbDateWizTypes] FOREIGN KEY ([typeCode]) REFERENCES [dbo].[dbDateWizTypes] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDateWizDates_dbEnquiryControl] FOREIGN KEY ([datectrlid]) REFERENCES [dbo].[dbEnquiryControl] ([ctrlID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDateWizDates_rowguid]
    ON [dbo].[dbDateWizDates]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDateWizDates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDateWizDates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDateWizDates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDateWizDates] TO [OMSApplicationRole]
    AS [dbo];

