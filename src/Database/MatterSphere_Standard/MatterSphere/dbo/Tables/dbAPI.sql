CREATE TABLE [dbo].[dbAPI] (
    [apiGUID]            UNIQUEIDENTIFIER CONSTRAINT [DF_dbAPI_prGUID] DEFAULT (newid()) NOT NULL,
    [apiCode]            NVARCHAR (100)   NOT NULL,
    [apiDesc]            NVARCHAR (50)    NOT NULL,
    [apiAuthor]          NVARCHAR (100)   NOT NULL,
    [apiUIType]          TINYINT          CONSTRAINT [DF_dbAPI_prWeb] DEFAULT ((0)) NOT NULL,
    [apiDesigner]        BIT              CONSTRAINT [DF_dbAPI_prDesigner] DEFAULT ((0)) NOT NULL,
    [apiRegistered]      BIT              CONSTRAINT [DF_dbAPI_apiValid] DEFAULT ((0)) NOT NULL,
    [apiService]         BIT              CONSTRAINT [DF_dbAPI_apiService] DEFAULT ((0)) NOT NULL,
    [apiCompanyID]       BIGINT           NULL,
    [apiPublicKeyToken]  NVARCHAR (20)    NULL,
    [apiValidFrom]       DATETIME         CONSTRAINT [DF_dbAPI_apiValidFrom] DEFAULT (getutcdate()) NOT NULL,
    [apiExpires]         DATETIME         NULL,
    [apiDefaultPriority] INT              CONSTRAINT [DF_dbAPI_apiDefaultPriority] DEFAULT ((-1)) NOT NULL,
    [apiMaximumPriority] INT              CONSTRAINT [DF_dbAPI_apiMaximumPriority] DEFAULT ((-1)) NOT NULL,
    [apiConsumerType]    INT              CONSTRAINT [DF_dbAPI_apiConsumerType] DEFAULT ((1)) NOT NULL,
    [rowguid]            UNIQUEIDENTIFIER CONSTRAINT [DF_dbAPI_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbAPI] PRIMARY KEY CLUSTERED ([apiGUID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [IX_dbAPI_apiCode] UNIQUE NONCLUSTERED ([apiCode] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbAPI_rowguid]
    ON [dbo].[dbAPI]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


-- =============================================
-- Author:		Daniel Meech
-- Create date: 19/08/2011
-- Description:	Keep bit values and cosnumer type in sync
-- =============================================
CREATE TRIGGER [dbo].[tgrConsumerTypeSync]
ON [dbo].[dbAPI] 
AFTER INSERT,UPDATE NOT FOR REPLICATION
AS 
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

if (update(apidesigner) or update(apiservice))
begin 
update A
set A.apiconsumertype = ((case coalesce(I.apidesigner, 0) when 1 then coalesce(I.apiconsumertype, 0) | 2 else coalesce(I.apiconsumertype, 0) - (coalesce(I.apiconsumertype, 0) & 2) end) 
| (case coalesce(I.apiservice, 0) when 1 then coalesce(I.apiconsumertype, 0) | 4 else coalesce(I.apiconsumertype, 0) - (coalesce(I.apiconsumertype, 0) & 4) end)) 
from dbapi A
inner join inserted I on I.apiguid = A.apiguid
end 
if (update(apiconsumertype) and not (update(apiservice) and update(apidesigner)))
begin 
update A
set A.apidesigner = (case when coalesce(I.apiconsumertype, 0) & 2 = 2 then 1 else 0 end),
A.apiservice = (case when coalesce(I.apiconsumertype, 0) & 4 = 4 then 1 else 0 end)
from dbapi A
inner join inserted I on I.apiguid = A.apiguid
where not I.apiconsumertype is null
end 
END

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbAPI] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbAPI] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbAPI] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbAPI] TO [OMSApplicationRole]
    AS [dbo];

