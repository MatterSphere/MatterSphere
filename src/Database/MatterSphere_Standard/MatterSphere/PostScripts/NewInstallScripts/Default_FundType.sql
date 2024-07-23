
Print 'NewInstallScripts\Default_FundType.sql'

-- Add default Fund types
IF EXISTS ( SELECT 'NOCHG' , 'GBP' EXCEPT SELECT ftCode , ftCurISOCode FROM dbo.dbFundType )
BEGIN
	INSERT dbo.dbFundType ( ftCode , ftCurISOCode , ftCreditLimit , ftCLCode , ftRefCode , ftBand , ftWarningPerc , ftAgreementCode , ftLegalAidCharged , ftAccCode , ftRatePerUnit , ftActive )
	VALUES ( 'NOCHG' , 'GBP' , 0.00 , 'NOCHG' , 'NOCHG' , 3 , 100 , 'NOCHG' , 0 , 0 , 0.00 , 1 )
END
GO


IF EXISTS ( SELECT 'TEMPLATE' , 'GBP' EXCEPT SELECT ftCode , ftCurISOCode FROM dbo.dbFundType )
BEGIN
	INSERT dbo.dbFundType ( ftCode , ftCurISOCode , ftCreditLimit , ftCLCode , ftRefCode , ftBand , ftWarningPerc , ftAgreementCode , ftLegalAidCharged , ftAccCode , ftRatePerUnit , ftActive )
	VALUES ( 'TEMPLATE' , 'GBP' , 1000.00 , 'NOCHG' , 'NOCHG' , 3 , 100 , 'NOCHG' , 0 , 0 , 0.00 , 1 )
END
GO	