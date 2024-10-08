if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSCheckLengths]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSCheckLengths]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSGetLedger]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSGetLedger]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








CREATE  procedure dbo.sprCMSCheckLengths
as

select
	*
from
	dbclienttype
where
	len(typecode)>5

select 
	* 
from 
	dbactivities
where
	len(actacccode) >5

select 
	* 
from 
	dbcodelookup 
where 
	cdtype = 'FILESTATUS' and
	len(cdcode)>5

select 
	* 
from 
	dbcodelookup 
where 
	cdtype = 'SOURCE' and
	len(cdcode)>5 and
	cdCode <>'CONTACT'

select 
	*
from
	dbDepartment 
where 
	deptacccode is null or
	len(deptacccode)>5

select 
	* 
from 
	dbFileType
where
	fileacccode is null or
	len(fileacccode)>5







GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO










CREATE     PROCEDURE [dbo].[sprCMSGetLedger]
(
	@CLNO as nvarchar(20),
	@FILENO as nvarchar(5)
)

AS

SET NOCOUNT ON

SELECT 
	cl.client_name, 
	cl.client_code,
	mat.matter_name,
	matter_code, 
	disb.tran_date as dates,
	disb.Last_modified as moddate,
	case when len(DISB.disb_ref) < 0 then 'Payment of Anticipated Disbursement o' + convert(nvarchar(30),DISB.tobill_amt * -1)
		when DISB.source_tran_typ = 'AD' then TXT.txt1 + ' o' + convert(nvarchar(30),DISB.billed_amt)
	else coalesce(txt.TXT1, 'No Narrative Entered') 
	end as narrative,
	DISB.source_tran_typ as TransType,
	0 as billdebit,
	0 as billcredit,
	disb.base_amt as offdebit,
	0 as offcredit,
	0 as clidebit,
	0 as clicredit,
	null as trx_desc
	
from 
	[SalesDemo].DBO.hbm_client CL
inner join
	[SalesDemo].DBO.hbm_matter MAT on MAT.client_uno = cl.client_uno
inner join 
	[SalesDemo].DBO.cdt_disb DISB on DISB.matter_uno = MAT.matter_uno
inner join
	[SalesDemo].DBO.cdt_text TXT on TXT.text_id = DISB.nar_text_id

where 
	cl.client_code = @CLNO and 
	mat.matter_code = @FILENO


UNION SELECT 
	cl.client_name, 
	cl.client_code,
	mat.matter_name,
	matter_code,  
	cltran.tran_date as dates,
	cltran.last_modified as moddate,
	case when CLTRAN.tran_type like '%X%' then 'Automatic System Posting'
		when CLTRAN.REMARKS is null or CLTRAN.REMARKS = '' then 'No Narrative Entered'
		else CLTRAN.REMARKS
	end as narrative,
	CLTRAN.tran_type as TransType,
	0 as billdebit,
	0 as billcredit,
	0 as offdebit,
	0 as offcredit,
	case when CLTRAN.SIGN = -1 and AMOUNT < 0 then 0
		when cltran.sign = -1 and AMOUNT >= 0 then AMOUNT 
		when AMOUNT < 0 then AMOUNT + (2 * -AMOUNT)
		else 0
	end as clidebit,
	case when CLTRAN.SIGN = 1 and AMOUNT >= 0 then AMOUNT 
	 	when CLTRAN.SIGN = 1 and AMOUNT < 0  then 0
		when CLTRAN.SIGN = -1 and AMOUNT >= 0 and CLTRAN.tran_type like '%x%' then 0
		when CLTRAN.SIGN = -1 and AMOUNT < 0 and CLTRAN.tran_type like '%x%' then AMOUNT + (2 * -AMOUNT)
	 	when AMOUNT >= 0  then AMOUNT
		else 0
	end as clicredit,
	TRXTYPE.trx_desc 
	

from 
	[SalesDemo].DBO.hbm_client CL
inner join
	[SalesDemo].DBO.hbm_matter MAT on MAT.client_uno = cl.client_uno
inner join 
	[SalesDemo].DBO.trm_trust TRMTRUST on TRMTRUST.matter_uno = mat.matter_uno
inner join 
	[SalesDemo].DBO.act_tran_trust CLTRAN on CLTRAN.trust_uno = TRMTRUST.trust_uno
left outer join 
	[SalesDemo].DBO.trl_trx_type TRXTYPE on TRXTYPE.trx_type_code = CLTRAN.trx_type_code

where 
	cl.client_code = @CLNO and 
	mat.matter_code = @FILENO


UNION SELECT
	cl.client_name, 
	cl.client_code,
	mat.matter_name,
	matter_code, 
	BB.bill_date as dates,	
	BB.Last_modified as moddate,
	coalesce(BT.txt1,'No Bill Narrative Entered') as narrative,
	BBA.tran_type as TransType,
		case when BBA.SIGN = 1 and tc_total_amt < 0 then 0
		when BBA.sign = 1 and tc_total_amt >= 0 then tc_total_amt
		when tc_total_amt < 0 then tc_total_amt
		else 0
	end as billdebit,	
		case when BBA.SIGN = -1 and tc_total_amt >= 0 then tc_total_amt
	 	when BBA.SIGN = -1 and tc_total_amt < 0 then 0
	 	when tc_total_amt < 0 then tc_total_amt
		else 0
	end as billcredit,
		case when BBA.SIGN = -1 and tc_total_amt >= 0 and BBA.tran_type like '%x%' then (BBA.hard_amt + BBA.soft_amt)
	 	when BBA.SIGN = -1 and tc_total_amt < 0 and BBA.tran_type like '%x%' then 0
	 	when tc_total_amt < 0 and BBA.tran_type like '%x%' then (BBA.hard_amt + BBA.soft_amt)
		else 0
	end as offdebit,
		case when BBA.SIGN = 1 and tc_total_amt < 0 then 0
		when BBA.sign = 1 and tc_total_amt >= 0 and BBA.tran_type <> 'RAX' then (BBA.hard_amt + BBA.soft_amt)
		when tc_total_amt < 0 then (BBA.hard_amt + BBA.soft_amt)
		else 0
	end as offcredit,
	0 as clidebit,
	0 as clicredit,
	null as trx_desc
	
from 
	[SalesDemo].DBO.hbm_client CL
inner join
	[SalesDemo].DBO.hbm_matter MAT on MAT.client_uno = cl.client_uno
inner join
	[SalesDemo].DBO.blt_bill_amt BBA on BBA.matter_uno = MAT.matter_uno
left join 
	[SalesDemo].DBO.blt_bill BB on BB.tran_uno = BBA.bill_tran_uno 
left join
	[SalesDemo].DBO.blt_text BT on BT.text_id = BB.text_id

where 
	cl.client_code = @CLNO and 
	mat.matter_code = @FILENO


UNION SELECT 
	cl.client_name, 
	cl.client_code,
	mat.matter_name,
	matter_code, 
	ATCM.tran_date as dates,
	ATCM.last_modified as moddate,
	case when ATCM.Remarks is not null and ATCM.Remarks <> ''  then ATCM.Remarks 
		when ATCM.Remarks = '' and ATXT.txt1 is not null then ATXT.txt1
		else 'Automatic System Posting'
	end as narrative,
	ATCM.tran_type as TransType,
	case when ATCM.tran_type = 'ra' and ATCM.Amount < 0 and ATCM.Data_type <> 'D' then ATCM.Amount + (2 * -AMOUNT)
		when ATCM.tran_type like '%CR%' and ATCM.Data_type <> 'd' then ATCM.Amount
		when ATCM.Amount < 0 and ATCM.tran_type = 'cbt' and ATCM.Data_type <> 'D' then ATCM.Amount + (2 * -AMOUNT)
	 	else 0
	end  as billdebit,
	case when ATCM.tran_type = 'rax' and ATCM.Data_type <> 'D' then ATCM.Amount + (2 * -AMOUNT)
		when ATCM.tran_type = 'ra' and ATCM.Amount >= 0 and ATCM.Data_type <> 'D' then ATCM.Amount
		when ATCM.tran_type like '%cr%' and ATCM.Amount >= 0 and ATCM.Data_type <> 'D' then ATCM.Amount
	 	when ATCM.Amount >= 0 and ATCM.tran_type = 'cbt' and ATCM.Data_type <> 'D' then ATCM.Amount
	 	else 0
	end as billcredit,
	case when ATCM.tran_type like '%CR%' and ATCM.Data_type = 'D' then ATCM.Amount
		when ATCM.tran_type = 'ra' and ATCM.Amount < 0 and ATCM.Data_type = 'D' then ATCM.Amount + (2 * -AMOUNT)
		when ATCM.Amount < 0 and ATCM.tran_type = 'cbt' and ATCM.Data_type = 'D' then ATCM.Amount + (2 * -AMOUNT)
	 	else 0
	end   as offdebit,
	case when ATCM.tran_type = 'ra' and ATCM.Amount >= 0 and ATCM.Data_type = 'D' then ATCM.Amount
		when ATCM.tran_type = 'rax' and ATCM.Data_type = 'D' then ATCM.Amount + (2 * -AMOUNT)
		when ATCM.tran_type like '%cr%' and ATCM.Amount >= 0 and ATCM.Data_type = 'D' then ATCM.Amount
	 	when ATCM.Amount >= 0 and ATCM.tran_type = 'cbt' and ATCM.Data_type = 'D' then ATCM.Amount
	 	else 0
	end as offcredit,
	0 as clidebit,
	0 as clicredit,
	null as trx_desc
	

from 
	[SalesDemo].DBO.hbm_client CL
inner join
	[SalesDemo].DBO.hbm_matter MAT on MAT.client_uno = cl.client_uno
inner join 
	[SalesDemo].DBO.act_tran_cr_ma ATCM on ATCM.matter_uno = MAT.matter_uno
inner join 
	[SalesDemo].DBO.act_tran ACT on ACT.tran_uno = ATCM.Tran_uno
inner join 
	[SalesDemo].DBO.act_text ATXT on ATXT.text_ID = ACT.trans_text_ID

where 
	cl.client_code = @CLNO and 
	mat.matter_code = @FILENO

order by 
	dates, transtype, moddate



SET NOCOUNT OFF







GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

