

CREATE PROCEDURE [dbo].[sprFinancialHistory] 
(
@FILEID BIGINT = null,
@UI uUICultureInfo='{default}'
)
AS

declare @OfficeCredit money
declare @OfficeDebit money
declare @ClientCredit money
declare @ClientDebit money

set @OfficeCredit = (select isnull(sum(FL.fingross),0) from dbfinancialledger FL join dbpostingtype PT on FL.PostID = PT.postID where PT.postOfficeCredit = 1 and FL.fileID = @FILEID)
set @OfficeDebit = (select isnull(sum(FL.fingross),0) from dbfinancialledger FL join dbpostingtype PT on FL.PostID = PT.postID where PT.postOfficeDebit = 1 and FL.fileID = @FILEID)
set @ClientCredit = (select isnull(sum(FL.fingross),0) from dbfinancialledger FL join dbpostingentrytype PT on FL.postID = PT.postID where PT.postClientCredit = 1 and FL.fileID = @FILEID)
set @ClientDebit = (select isnull(sum(FL.fingross),0) from dbfinancialledger FL join dbpostingentrytype PT on FL.postID = PT.postID where PT.postClientDebit = 1 and FL.fileID = @FILEID)

select 
dbo.GetCodeLookupDesc('RESOURCE', 'OFFICE', @UI),
(@OfficeCredit) [Credit],
(@OfficeDebit) [Debit],
(@OfficeCredit - @OfficeDebit) [Summary]
union all
select
dbo.GetCodeLookupDesc('RESOURCE', 'CLIENT', @UI),
(@ClientCredit) [Credit],
(@ClientDebit) [Debit],
(@ClientCredit - @ClientDebit) [Summary]

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFinancialHistory] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFinancialHistory] TO [OMSAdminRole]
    AS [dbo];

