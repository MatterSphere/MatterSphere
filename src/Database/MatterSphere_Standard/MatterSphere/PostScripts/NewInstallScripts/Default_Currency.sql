Print 'Starting NewInstallScripts\Default_Currency.sql'

-- Add default Currency
IF NOT EXISTS ( SELECT curISOCode FROM dbo.dbCurrency WHERE curISOCode = 'CD' )
BEGIN
	INSERT dbo.dbCurrency ( curISOCode , curName , curSign , curSignDesc , curRate , curDecimalPlaces , curDecimalSeperator , curGroupSeparator , curNegativePattern , curPositivePattern , curActive )
	VALUES ( 'CD' , 'Canadian Dollars' , 'C$' , 'Canadian Dollars' , 1 , 2 , '.' , ',' , 1 , 0 , 1 )
END
GO

IF NOT EXISTS ( SELECT curISOCode FROM dbo.dbCurrency WHERE curISOCode = 'EUR' )
BEGIN
	INSERT dbo.dbCurrency ( curISOCode , curName , curSign , curSignDesc , curRate , curDecimalPlaces , curDecimalSeperator , curGroupSeparator , curNegativePattern , curPositivePattern , curActive )
	VALUES ( 'EUR' , 'Euro' , '€' , 'Euro' , 1.5789 , 2	 , '.' , ',' , 1 , 0 , 1 )
END
GO	

IF NOT EXISTS ( SELECT curISOCode FROM dbo.dbCurrency WHERE curISOCode = 'GBP' )
BEGIN
	INSERT dbo.dbCurrency ( curISOCode , curName , curSign , curSignDesc , curRate , curDecimalPlaces , curDecimalSeperator , curGroupSeparator , curNegativePattern , curPositivePattern , curActive )
	VALUES ( 'GBP' , 'Great Britain Pound' , '£' , 'Pound Sterling' , 1 , 2 , '.' , ',' , 1 , 0 , 1 )
END
GO

IF NOT EXISTS ( SELECT curISOCode FROM dbo.dbCurrency WHERE curISOCode = 'USD' )
BEGIN
	INSERT dbo.dbCurrency ( curISOCode , curName , curSign , curSignDesc , curRate , curDecimalPlaces , curDecimalSeperator , curGroupSeparator , curNegativePattern , curPositivePattern , curActive )
	VALUES ( 'USD' , 'United States Dollars' , 'US$' , 'US Dollars' , 1 , 2 , '.' , ',' , 1 , 0 , 1 )
END
GO	



