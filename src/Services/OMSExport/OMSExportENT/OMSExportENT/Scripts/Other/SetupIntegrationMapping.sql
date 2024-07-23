
declare @IntSys nvarchar(50)
set @IntSys = '67933068-6C1F-423C-A86E-2B8C656E1F58'

if (not exists(select ID from fddbIntegrationSystem where ID = @IntSys))
	insert into fddbIntegrationSystem ( ID, [Name]) values (@IntSys,'Enterprise')

declare @IntEnt nvarchar(50)

set @IntEnt = '9EC7680D-7E93-4627-9DE4-4CF056B34299'
if (not exists(select ID from fddbIntegrationEntity where ID = @IntEnt))
	insert into fddbIntegrationEntity ( ID, [Name]) values (@IntEnt,'Client')
set @IntEnt = '12DF5A9E-EED8-4e36-A543-5C84A817DA3D'
if (not exists(select ID from fddbIntegrationEntity where ID = @IntEnt))
	insert into fddbIntegrationEntity ( ID, [Name]) values (@IntEnt,'File')
set @IntEnt = 'DAE5C022-B238-4284-8F63-51D94668AEFB'
if (not exists(select ID from fddbIntegrationEntity where ID = @IntEnt))
	insert into fddbIntegrationEntity ( ID, [Name]) values (@IntEnt,'User')
set @IntEnt = 'C33002B6-8412-4AA6-B7F0-8005B3E7D18E'
if (not exists(select ID from fddbIntegrationEntity where ID = @IntEnt))
	insert into fddbIntegrationEntity ( ID, [Name]) values (@IntEnt,'Fee Earner')

