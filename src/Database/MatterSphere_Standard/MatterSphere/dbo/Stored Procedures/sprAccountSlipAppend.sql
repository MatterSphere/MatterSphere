

CREATE PROCEDURE [dbo].[sprAccountSlipAppend] 
(
	@FILEID bigint,
	@POSTID bigint,
	@FINENTRYID bigint,
	@CREATEDBY bigint,
	@FINITEMDATE datetime,
	@FINPAYNAME nvarchar(50),
	@FINASSOCID bigint,
	@FINAUTHBYFEEID bigint,
	@FINDESC nvarchar(50),
	@FINTHEIRREF nvarchar(20),
	@FINVALUE money,
	@FINVAT money,
	@FINGROSS money,
	@FINPAID bit,
	@FINEXPECTEDPAYMENT datetime,
	@FINNEEDEXPORT bit = 0,
	@FINREQUESTEXPORTBY bigint = null
)
AS
if @FILEID is not null begin
insert into dbfinancialledger
	(
		fileID,
	 	postID,
		finentryid,
		createdby,
		finItemDate,
		finPayName,
		finAssocID,
		finAuthByFeeID,
		finDesc,
		finTheirRef,
		finValue,
		finVat,
		finGross,
		finPaid,
		finExpectedPayment,
		finNeedExport,
		finRequestExportBy,
		created)
values
	(
		@FILEID,
		@POSTID,
		@FINENTRYID,
		@CREATEDBY,
		@FINITEMDATE,
		@FINPAYNAME,
		@FINASSOCID,
		@FINAUTHBYFEEID,
		@FINDESC,
		@FINTHEIRREF,
		@FINVALUE,
		@FINVAT,
		@FINGROSS,
		@FINPAID,
		@FINEXPECTEDPAYMENT,
		@FINNEEDEXPORT,
		@FINREQUESTEXPORTBY,
		getutcdate())
END 
select scope_identity(), scope_identity() --will return null if there is a failure...

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAccountSlipAppend] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAccountSlipAppend] TO [OMSAdminRole]
    AS [dbo];

