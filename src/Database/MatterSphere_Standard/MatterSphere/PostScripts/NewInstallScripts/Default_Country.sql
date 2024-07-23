Print 'Starting NewInstallScripts\Default_Country.sql'

-- add default Countries
IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 1 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 1 , 'AFGHANISTAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 2 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 2 , 'ALBANIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 3 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 3 , 'ALGERIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 4 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 4 , 'AMERICAN SAMOA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 5 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 5 , 'ANDORRA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 6 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 6 , 'ANGOLA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 7 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 7 , 'ANGUILLA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 8 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 8 , 'ANTARCTICA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 9 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 9 , 'ANTIGUA AND BAR' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 10 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 10 , 'ARGENTINA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 11 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 11 , 'ARMENIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 12 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 12 , 'ARUBA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 13 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 13 , 'AUSTRALIA' , '<ADDRESSLINES>
	<Line field="addLine1" question="txtAdd1"/>
	<Line field="addLine2" question="txtAdd2"/>
	<Line field="addLine3" question="txtAdd3"/>
	<Line field="addLine4" question="txtCity"/>
	<Line field="addLine5" question="txtCounty"/>
	<Line field="addPostCode" question="txtPostCode"/>
	<Line field="addCountry" question="cboCountry"/>
</ADDRESSLINES>' , 0 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 14 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 14 , 'AUSTRIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 15 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 15 , 'AZERBAIJAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 16 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 16 , 'BAHAMAS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 17 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 17 , 'BAHRAIN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 18 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 18 , 'BANGLADESH' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 19 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 19 , 'BARBADOS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 20 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 20 , 'BELARUS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 21 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 21 , 'BELGIUM' , '<ADDRESSLINES>
	<Line field="addLine1" question="txtAdd1"/>
	<Line field="addLine2" question="txtAdd2"/>
	<Line field="addLine3" question="txtAdd3"/>
	<Line field="addLine4" question="txtCity"/>
	<Line field="addLine5" question="txtProvince"/>
	<Line field="addPostCode" question="txtPostCode"/>
	<Line field="addCountry" question="cboCountry"/>
</ADDRESSLINES>' , 0 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 22 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 22 , 'BELIZE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 23 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 23 , 'BENIN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 24 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 24 , 'BERMUDA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 25 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 25 , 'BHUTAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 26 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 26 , 'BOLIVIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 27 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 27 , 'BOSNIA AND HERZ' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 28 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 28 , 'BOTSWANA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 29 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 29 , 'BOUVET ISLAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 30 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 30 , 'BRAZIL' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 31 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 31 , 'BRITISH INDIAN ' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 32 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 32 , 'BRUNEI' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 33 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 33 , 'BULGARIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 34 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 34 , 'BURKINA FASO' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 35 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 35 , 'BURUNDI' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 36 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 36 , 'CAMBODIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 37 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 37 , 'CAMEROON' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 38 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 38 , 'CANADA' , '<ADDRESSLINES>
	<Line field="addLine1" question="txtAdd1"/>
	<Line field="addLine2" question="txtAdd2"/>
	<Line field="addLine3" question="txtAdd3"/>
	<Line field="addLine4" question="txtCity"/>
	<Line field="addLine5" question="txtState"/>
	<Line field="addPostCode" question="txtZipCode"/>
	<Line field="addCountry" question="cboCountry"/>
</ADDRESSLINES>' , 0 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 39 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 39 , 'CAPE VERDE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 40 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 40 , 'CAYMAN ISLANDS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 41 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 41 , 'CENTRAL AFRICAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 42 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 42 , 'CHAD' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 43 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 43 , 'CHILE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 44 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 44 , 'CHINA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 45 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 45 , 'CHRISTMAS ISLAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 46 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 46 , 'COCOS (KEELING)' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 47 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 47 , 'COLOMBIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 48 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 48 , 'COMOROS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 49 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 49 , 'CONGO' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 50 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 50 , 'COOK ISLANDS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 51 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 51 , 'COSTA RICA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 52 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 52 , 'CÔTE D''IVOIRE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 53 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 53 , 'CROATIA (HRVATS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 54 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 54 , 'CUBA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 55 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 55 , 'CYPRUS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 56 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 56 , 'CZECH REPUBLIC' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 57 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 57 , 'CONGO (DRC)' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 58 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 58 , 'DENMARK' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 59 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 59 , 'DJIBOUTI' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 60 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 60 , 'DOMINICA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 61 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 61 , 'DOMINICAN REPUB' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 62 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 62 , 'EAST TIMOR' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 63 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 63 , 'ECUADOR' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 64 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 64 , 'EGYPT' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 65 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 65 , 'EL SALVADOR' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 66 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 66 , 'EQUATORIAL GUIN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 67 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 67 , 'ERITREA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 68 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 68 , 'ESTONIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 69 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 69 , 'ETHIOPIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 70 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 70 , 'FALKLAND ISLAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 71 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 71 , 'FAROE ISLANDS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 72 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 72 , 'FIJI ISLANDS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 73 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 73 , 'FINLAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 74 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 74 , 'FRANCE' , '<ADDRESSLINES>
	<Line field="addLine1" question="txtAdd1"/>
	<Line field="addLine2" question="txtAdd2"/>
	<Line field="addLine3" question="txtAdd3"/>
	<Line field="addLine4" question="txtCity"/>
	<Line field="addLine5" question="txtProvince"/>
	<Line field="addPostCode" question="txtPostCode"/>
	<Line field="addCountry" question="cboCountry"/>
</ADDRESSLINES>' , 0 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 75 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 75 , 'FRENCH GUIANA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 76 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 76 , 'FRENCH POLYNESI' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 77 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 77 , 'FRENCH SOUTHERN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 78 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 78 , 'GABON' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 79 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 79 , 'GAMBIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 80 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 80 , 'GEORGIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 81 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 81 , 'GERMANY' , '<ADDRESSLINES>
	<Line field="addLine1" question="txtAdd1"/>
	<Line field="addLine2" question="txtAdd2"/>
	<Line field="addLine3" question="txtAdd3"/>
	<Line field="addLine4" question="txtCity"/>
	<Line field="addLine5" question="txtProvince"/>
	<Line field="addPostCode" question="txtPostCode"/>
	<Line field="addCountry" question="cboCountry"/>
</ADDRESSLINES>' , 0 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 82 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 82 , 'GHANA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 83 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 83 , 'GIBRALTAR' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 84 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 84 , 'GREECE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 85 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 85 , 'GREENLAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 86 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 86 , 'GRENADA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 87 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 87 , 'GUADELOUPE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 88 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 88 , 'GUAM' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 89 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 89 , 'GUATEMALA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 90 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 90 , 'GUINEA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 91 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 91 , 'GUINEABISSAU' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 92 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 92 , 'GUYANA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 93 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 93 , 'HAITI' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 94 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 94 , 'HEARD ISLAND AN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 95 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 95 , 'HONDURAS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 96 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 96 , 'HONG KONG SAR' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 97 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 97 , 'HUNGARY' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 98 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 98 , 'ICELAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 99 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 99 , 'INDIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 100 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 100 , 'INDONESIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 101 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 101 , 'IRAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 102 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 102 , 'IRAQ' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 103 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 103 , 'IRELAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 104 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 104 , 'ISRAEL' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 105 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 105 , 'ITALY' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 106 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 106 , 'JAMAICA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 107 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 107 , 'JAPAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 108 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 108 , 'JORDAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 109 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 109 , 'KAZAKHSTAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 110 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 110 , 'KENYA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 111 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 111 , 'KIRIBATI' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 112 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 112 , 'KOREA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 113 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 113 , 'KUWAIT' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 114 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 114 , 'KYRGYZSTAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 115 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 115 , 'LAOS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 116 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 116 , 'LATVIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 117 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 117 , 'LEBANON' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 118 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 118 , 'LESOTHO' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 119 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 119 , 'LIBERIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 120 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 120 , 'LIBYA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 121 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 121 , 'LIECHTENSTEIN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 122 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 122 , 'LITHUANIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 123 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 123 , 'LUXEMBOURG' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 124 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 124 , 'MACAU SAR' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 125 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 125 , 'MACEDONIA, FORM' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 126 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 126 , 'MADAGASCAR' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 127 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 127 , 'MALAWI' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 128 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 128 , 'MALAYSIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 129 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 129 , 'MALDIVES' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 130 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 130 , 'MALI' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 131 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 131 , 'MALTA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 132 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 132 , 'MARSHALL ISLAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 133 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 133 , 'MARTINIQUE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 134 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 134 , 'MAURITANIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 135 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 135 , 'MAURITIUS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 136 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 136 , 'MAYOTTE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 137 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 137 , 'MEXICO' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 138 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 138 , 'MICRONESIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 139 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 139 , 'MOLDOVA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 140 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 140 , 'MONACO' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 141 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 141 , 'MONGOLIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 142 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 142 , 'MONTSERRAT' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 143 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 143 , 'MOROCCO' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 144 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 144 , 'MOZAMBIQUE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 145 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 145 , 'MYANMAR' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 146 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 146 , 'NAMIBIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 147 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 147 , 'NAURU' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 148 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 148 , 'NEPAL' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 149 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 149 , 'NETHERLANDS' , '<ADDRESSLINES>
	<Line field="addLine1" question="txtAdd1"/>
	<Line field="addLine2" question="txtAdd2"/>
	<Line field="addLine3" question="txtAdd3"/>
	<Line field="addLine4" question="txtCity"/>
	<Line field="addLine5" question="txtProvince"/>
	<Line field="addPostCode" question="txtPostCode"/>
	<Line field="addCountry" question="cboCountry"/>
</ADDRESSLINES>' , 0 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 150 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 150 , 'NETHERLANDS ANT' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 151 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 151 , 'NEW CALEDONIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 152 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 152 , 'NEW ZEALAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 153 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 153 , 'NICARAGUA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 154 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 154 , 'NIGER' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 155 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 155 , 'NIGERIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 156 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 156 , 'NIUE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 157 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 157 , 'NORFOLK ISLAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 158 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 158 , 'NORTH KOREA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 159 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 159 , 'NORTHERN MARIAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 160 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 160 , 'NORWAY' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 161 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 161 , 'OMAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 162 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 162 , 'PAKISTAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 163 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 163 , 'PALAU' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 164 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 164 , 'PANAMA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 165 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 165 , 'PAPUA NEW GUINE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 166 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 166 , 'PARAGUAY' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 167 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 167 , 'PERU' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 168 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 168 , 'PHILIPPINES' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 169 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 169 , 'PITCAIRN ISLAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 170 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 170 , 'POLAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 171 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 171 , 'PORTUGAL' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 172 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 172 , 'PUERTO RICO' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 173 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 173 , 'QATAR' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 174 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 174 , 'REUNION' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 175 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 175 , 'ROMANIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 176 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 176 , 'RUSSIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 177 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 177 , 'RWANDA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 178 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 178 , 'ST. KITTS AND N' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 179 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 179 , 'ST. LUCIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 180 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 180 , 'ST. VINCENT AND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 181 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 181 , 'SAMOA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 182 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 182 , 'SAN MARINO' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 183 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 183 , 'SÃO TOMÉ AND PR' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 184 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 184 , 'SAUDI ARABIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 185 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 185 , 'SCOTLAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 186 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 186 , 'SENEGAL' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 187 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 187 , 'SEYCHELLES' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 188 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 188 , 'SIERRA LEONE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 189 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 189 , 'SINGAPORE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 190 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 190 , 'SLOVAKIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 191 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 191 , 'SLOVENIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 192 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 192 , 'SOLOMON ISLANDS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 193 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 193 , 'SOMALIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 194 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 194 , 'SOUTH AFRICA' , '<ADDRESSLINES>
	<Line field="addLine1" question="txtAdd1"/>
	<Line field="addLine2" question="txtAdd2"/>
	<Line field="addLine3" question="txtAdd3"/>
	<Line field="addLine4" question="txtCity"/>
	<Line field="addLine5" question="txtProvince"/>
	<Line field="addPostCode" question="txtPostCode"/>
	<Line field="addCountry" question="cboCountry"/>
</ADDRESSLINES>' , 0 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 195 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 195 , 'SOUTH GEORGIA A' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 196 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 196 , 'SPAIN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 197 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 197 , 'SRI LANKA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 198 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 198 , 'ST. HELENA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 199 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 199 , 'ST. PIERRE AND ' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 200 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 200 , 'SUDAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 201 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 201 , 'SURINAME' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 202 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 202 , 'SVALBARD AND JA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 203 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 203 , 'SWAZILAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 204 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 204 , 'SWEDEN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 205 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 205 , 'SWITZERLAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 206 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 206 , 'SYRIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 207 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 207 , 'TAIWAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 208 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 208 , 'TAJIKISTAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 209 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 209 , 'TANZANIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 210 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 210 , 'THAILAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 211 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 211 , 'TOGO' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 212 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 212 , 'TOKELAU' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 213 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 213 , 'TONGA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 214 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 214 , 'TRINIDAD AND TO' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 215 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 215 , 'TUNISIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 216 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 216 , 'TURKEY' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 217 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 217 , 'TURKMENISTAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 218 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 218 , 'ISTURKS AND CAI' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 219 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 219 , 'TUVALU' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 220 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 220 , 'UGANDA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 221 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 221 , 'UKRAINE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 222 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 222 , 'UNITED ARAB EMI' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 223 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 223 , 'ENGLAND' , '<ADDRESSLINES>
	<Line field="addLine1" question="txtAdd1"/>
	<Line field="addLine2" question="txtAdd2"/>
	<Line field="addLine3" question="txtTown"/>
	<Line field="addLine4" question="txtCity"/>
	<Line field="addLine5" question="txtCounty"/>
	<Line field="addPostcode" question="txtPostCode"/>
	<Line field="addCountry" question="cboCountry"/>
</ADDRESSLINES>' , 0 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 224 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 224 , 'UNITED STATES' , '<ADDRESSLINES>
	<Line field="addLine1" question="txtAdd1"/>
	<Line field="addLine2" question="txtAdd2"/>
	<Line field="addLine3" question="txtAdd3"/>
	<Line field="addLine4" question="txtCity"/>
	<Line field="addLine5" question="txtState"/>
	<Line field="addPostCode" question="txtZipCode"/>
	<Line field="addCountry" question="cboCountry"/>
</ADDRESSLINES>' , 0 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 225 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 225 , 'UNITED STATES M' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 226 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 226 , 'URUGUAY' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 227 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 227 , 'UZBEKISTAN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 228 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 228 , 'VANUATU' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 229 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 229 , 'VATICAN ' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 230 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 230 , 'VENEZUELA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 231 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 231 , 'VIET NAM' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 232 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 232 , 'VI_British' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 233 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 233 , 'VIRGIN ISLANDS' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 234 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 234 , 'WALES' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 235 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 235 , 'WALLIS AND FUTU' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 236 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 236 , 'YEMEN' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 237 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 237 , 'YUGOSLAVIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 238 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 238 , 'ZAMBIA' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 239 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 239 , 'ZIMBABWE' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 240 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 240 , 'NORTHERNIRELAND' , NULL , 1 )
END
GO

IF NOT EXISTS ( SELECT ctryID FROM dbo.dbCountry WHERE ctryID = 241 )
BEGIN
	INSERT dbo.dbCountry ( ctryID , ctryCode , ctryAddressFormat , ctryIgnore )
	VALUES ( 241 , 'NOT SPEC' , NULL , 0 )
END
GO
