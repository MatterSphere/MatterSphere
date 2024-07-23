

CREATE PROCEDURE [dbo].[sprPackageData]
(
	@UI uUICultureInfo = '{default}',
	@CODE nvarchar(15)
)

AS

DECLARE 
	@doc1 varchar(4000), 
	@doc2 varchar(4000), 
	@doc3 varchar(4000), 
	@doc4 varchar(4000),  
	@doc5 nvarchar(4000), 
	@doc6 nvarchar(4000), 
	@doc7 nvarchar(4000), 
	@doc8 nvarchar(4000), 
	@doc9 nvarchar(4000), 
	@doc10 nvarchar(4000), 
	@idoc int, 
	@ptr varbinary(16), 
	@doc11 varchar(4000), 
	@doc12 varchar(4000), 
	@doc13 varchar(4000), 
	@doc14 varchar(4000), 
	@doc15 nvarchar(4000), 
	@doc16 nvarchar(4000), 
	@doc17 nvarchar(4000), 
	@doc18 nvarchar(4000), 
	@doc19 nvarchar(4000), 
	@doc20 nvarchar(4000)
 
SELECT @doc1 = replace(replace(coalesce (Substring ( pkgXML , (0*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '')  FROM dbPackages where pkgcode = @CODE
SELECT @doc2 = replace(replace(coalesce (Substring ( pkgXML , (1*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc3 = replace(replace(coalesce (Substring ( pkgXML , (2*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc4 = replace(replace(coalesce (Substring ( pkgXML , (3*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc5 = replace(replace(coalesce (Substring ( pkgXML , (4*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc6 = replace(replace(coalesce (Substring ( pkgXML , (5*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc7 = replace(replace(coalesce (Substring ( pkgXML , (6*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc8 = replace(replace(coalesce (Substring ( pkgXML , (7*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc9 = replace(replace(coalesce (Substring ( pkgXML , (8*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc10 = replace(replace(coalesce (Substring ( pkgXML , (9*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc11 = replace(replace(coalesce (Substring ( pkgXML , (10*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc12 = replace(replace(coalesce (Substring ( pkgXML , (11*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc13 = replace(replace(coalesce (Substring ( pkgXML , (12*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc14 = replace(replace(coalesce (Substring ( pkgXML , (13*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc15 = replace(replace(coalesce (Substring ( pkgXML , (14*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc16 = replace(replace(coalesce (Substring ( pkgXML , (15*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc17 = replace(replace(coalesce (Substring ( pkgXML , (16*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc18 = replace(replace(coalesce (Substring ( pkgXML , (17*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc19 = replace(replace(coalesce (Substring ( pkgXML , (18*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
SELECT @doc20 = replace(replace(coalesce (Substring ( pkgXML , (19*4000) + 1 , 4000) , ' '), '&#xD;', ''), '&#xA;', '') FROM dbPackages where pkgcode = @CODE
 
EXEC ( ' DECLARE @idoc int
EXEC sp_xml_preparedocument @idoc OUTPUT, ''' + @doc1 + @doc2 +  @doc3 +  @doc4 +  @doc5 +  @doc6 +
            @doc7 +  @doc8 + @doc9  + @doc10 + @doc11 + @doc12 +  @doc13 +  @doc14 +  @doc15 +  @doc16 +
            @doc17 +  @doc18 + @doc19 + @doc20 + '''
 
SELECT    *
FROM       OPENXML (@idoc,  ''/config/Items'' , 0 )
  WITH (Licenses nvarchar(100)    ''../@Licenses'',
    Code nvarchar(100)   ''../@Code'',
    Version int     ''../@Version'',
    [ID] int      ''@ID'',
    [Description] nvarchar(200) ''@Description'',
    Code nvarchar(100)   ''@Code'',
    Type nvarchar(100)   ''@Type'')
 WHERE [Description] <> ''Root''
 ORDER BY Type
 
EXEC sp_xml_removedocument @idoc')


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprPackageData] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprPackageData] TO [OMSAdminRole]
    AS [dbo];

