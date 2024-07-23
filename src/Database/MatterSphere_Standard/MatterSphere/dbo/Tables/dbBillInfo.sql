CREATE TABLE [dbo].[dbBillInfo] (
    [billID]        BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [billNo]        NVARCHAR (12)       NULL,
    [fileID]        BIGINT              NOT NULL,
    [assocID]       BIGINT              NOT NULL,
    [billCategory]  [dbo].[uCodeLookup] NOT NULL,
    [billDate]      DATETIME            CONSTRAINT [DF_dbBillInfo_BillDate] DEFAULT (getutcdate()) NOT NULL,
    [billOnAccount] MONEY               CONSTRAINT [DF_dbBillInfo_billOnAccount] DEFAULT ((0)) NOT NULL,
    [billexVat]     MONEY               CONSTRAINT [DF_dbBillInfo_billexVat] DEFAULT ((0)) NOT NULL,
    [billVAT]       MONEY               CONSTRAINT [DF_dbBillInfo_billVAT] DEFAULT ((0)) NOT NULL,
    [billTotal]     MONEY               CONSTRAINT [DF_dbBillInfo_billTotal] DEFAULT ((0)) NOT NULL,
    [billProCosts]  MONEY               CONSTRAINT [DF_dbBillInfo_billProCosts] DEFAULT ((0)) NOT NULL,
    [billPaidDisb]  MONEY               CONSTRAINT [DF_dbBillInfo_billPaidDisb] DEFAULT ((0)) NOT NULL,
    [billNYPDisb]   MONEY               CONSTRAINT [DF_dbBillInfo_billNYPDisb] DEFAULT ((0)) NOT NULL,
    [CreatedBy]     [dbo].[uCreatedBy]  NULL,
    [Created]       [dbo].[uCreated]    CONSTRAINT [DF_dbBillInfo_Created] DEFAULT (getutcdate()) NULL,
    [rowguid]       UNIQUEIDENTIFIER    CONSTRAINT [DF_dbBillInfo_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbBillInfo] PRIMARY KEY CLUSTERED ([billID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbBillInfo_rowguid]
    ON [dbo].[dbBillInfo]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrBillingInfoNumberGenerator] ON [dbo].[dbBillInfo] 
FOR INSERT NOT FOR REPLICATION
AS
declare @billid bigint
declare @number nvarchar(12)
declare @branch int
declare @usrid int

SELECT @billid = billid , @number = B.billno,  @usrid = B.createdby , @branch = F.brID  from inserted B left join dbfile F on  F.fileid = B.fileid 

-- If the branchID is specifically set i.e. the value is not 0 or -1 set the branchID to equal that value
-- =========================================================
IF (SELECT TOP 1 regBranchConfig FROM dbRegInfo)  > 0
	SET @branch = ( SELECT TOP 1 regBranchConfig FROM dbRegInfo R JOIN dbBranch B ON B.brID = R.regBranchConfig )

-- If using site specfic database (value 0)
-- ======================
IF (SELECT TOP 1 regBranchConfig FROM dbRegInfo) = 0 OR @branch IS NULL
	set @branch = (select top 1 brid from dbreginfo)

if @number is null or @number = ''
begin
	declare @newnum nvarchar(12)
	declare @seed uCodeLookup
	declare @ret int
	
	set @seed = 'BILL'
	execute @ret = sprGetNextSeedNo @branch, @seed, null, @newnum output, @billid
	if @ret = 0
		update dbbillinfo set billno = @newnum where billid = @billid
   	else
	begin
		declare @msg nvarchar(500)
		declare @severity tinyint
		declare @UI uUICultureInfo
		set @UI  = (select usruicultureinfo from dbuser where usrid = @usrid)
		execute @severity = sprRaiseError 'MSGBILLSEEDNO', @UI, @msg out
		raiserror (@msg, @severity, 1, @seed)
		rollback transaction
	end
end

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbBillInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbBillInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbBillInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbBillInfo] TO [OMSApplicationRole]
    AS [dbo];

