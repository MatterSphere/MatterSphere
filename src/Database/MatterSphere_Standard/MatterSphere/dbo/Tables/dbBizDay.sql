CREATE TABLE [dbo].[dbBizDay] (
    [bizDate]               SMALLDATETIME          NOT NULL,
    [bizCultureInfo]        [dbo].[uUICultureInfo] NOT NULL,
    [bizIsWeekDay]          AS                     (CONVERT([bit],case when datepart(weekday,[bizDate])=(7) OR datepart(weekday,[bizDate])=(1) then (0) else (1) end,(0))),
    [bizIsWorkDay]          BIT                    CONSTRAINT [DF_dbBizDay_bizIsWeekDay] DEFAULT ((1)) NOT NULL,
    [rowguid]               UNIQUEIDENTIFIER       CONSTRAINT [DF_dbBizDay_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [bizHolidayDescription] NVARCHAR (50)          NULL,
    CONSTRAINT [PK_dbBizDay] PRIMARY KEY CLUSTERED ([bizDate] ASC, [bizCultureInfo] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbBizDay_compound]
    ON [dbo].[dbBizDay]([bizDate] ASC, [bizCultureInfo] ASC, [bizIsWorkDay] ASC)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbBizDay_rowguid]
    ON [dbo].[dbBizDay]([rowguid] ASC)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbBizDay] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbBizDay] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbBizDay] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbBizDay] TO [OMSApplicationRole]
    AS [dbo];

