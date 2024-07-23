CREATE VIEW [dbo].[fdvwRaPIdAddresses] AS
select
		cd.fileID,
		'ClaimantAddress' = dbo.fdGetRaPIdAddress(
			cd.rpdClmtHouseName,
			cd.rpdClmtHouseNumber,
			cd.rpdClmtStreet1,
			cd.rpdClmtStreet2,
			cd.rpdClmtDistrict,
			cd.rpdClmtCity,
			cd.rpdClmtCounty,
			cd.rpdClmtPostCode,
			cd.rpdClmtCountry),
		'DefendantAddress' = 
			CASE WHEN dd.rpdDefStatus = 'P' Then dbo.fdGetRaPIdAddress(
			dd.rpdDefHouseName,
			dd.rpdDefHouseNumber,
			dd.rpdDefStreet1,
			dd.rpdDefStreet2,
			dd.rpdDefDistrict,
			dd.rpdDefCity,
			dd.rpdDefCounty,
			dd.rpdDefPostCode,
			dd.rpdDefCountry) 
			ELSE dd.rpdDefCompanyName + CHAR(13) + dbo.fdGetRaPIdAddress(
			dd.rpdDefWorkHouseName,
			dd.rpdDefWorkHouseNumber,
			dd.rpdDefWorkStreet1,
			dd.rpdDefWorkStreet2,
			dd.rpdDefWorkDistrict,
			dd.rpdDefWorkCity,
			dd.rpdDefWorkCounty,
			dd.rpdDefWorkPostCode,
			dd.rpdDefWorkCountry) END,			
		'DriverAddress' =  dbo.fdGetRaPIdAddress(
			ad.rpdAccDriverHouseName,
			ad.rpdAccDriverHouseNumber,
			ad.rpdAccDriverStreet1,
			ad.rpdAccDriverStreet2,
			ad.rpdAccDriverDistrict,
			ad.rpdAccDriverCity,
			ad.rpdAccDriverCounty,
			ad.rpdAccDriverPostCode,
			ad.rpdAccDriverCountry),
		'OwnerAddress' = dbo.fdGetRaPIdAddress(
			ad.rpdAccOwnerHouseName,
			ad.rpdAccOwnerHouseNumber,
			ad.rpdAccOwnerStreet1,
			ad.rpdAccOwnerStreet2,
			ad.rpdAccOwnerDistrict,
			ad.rpdAccOwnerCity,
			ad.rpdAccOwnerCounty,
			ad.rpdAccOwnerPostCode,
			ad.rpdAccOwnerCountry),
		'OwnerInsurerAddress' = dbo.fdGetRaPIdAddress(
			ad.rpdAccInsCompHouseName,
			ad.rpdAccInsCompHouseNumber,
			ad.rpdAccInsCompStreet1,
			ad.rpdAccInsCompStreet2,
			ad.rpdAccInsCompDistrict,
			ad.rpdAccInsCompCity,
			ad.rpdAccInsCompCounty,
			ad.rpdAccInsCompPostCode,
			ad.rpdAccInsCompCountry)			
from 
	dbo.fdRaPIdClaimantDetails as cd join
	dbo.fdRaPIdDefendantDetails as dd on cd.fileID = dd.fileID join
	dbo.fdRaPIdAccidentDetails as ad on cd.fileID = ad.fileID
