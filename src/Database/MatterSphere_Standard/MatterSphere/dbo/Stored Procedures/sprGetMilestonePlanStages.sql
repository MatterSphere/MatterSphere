
CREATE PROCEDURE [dbo].[sprGetMilestonePlanStages](@plan uCodeLookup, @UI uUICUltureInfo = '{default}', @includeNull bit = 0)
as

DECLARE @description NVARCHAR(MAX) = dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', 'en')

select null as stage, @description as [description]  from dbo.dbMSConfig_OMS2K where @includeNull <> 1
union
select Convert(tinyint, 1) as [stage],  msstage1desc as [description] from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage1desc)) > 0
union
select Convert(tinyint, 2),  msstage2desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage2desc)) > 0
union
select Convert(tinyint, 3),  msstage3desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage3desc)) > 0
union
select Convert(tinyint, 4),  msstage4desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage4desc)) > 0
union
select Convert(tinyint, 5),  msstage5desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage5desc)) > 0
union
select Convert(tinyint, 6),  msstage6desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage6desc)) > 0
union
select Convert(tinyint, 7),  msstage7desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage7desc)) > 0
union
select Convert(tinyint, 8),  msstage8desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage8desc)) > 0
union
select Convert(tinyint, 9),  msstage9desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage9desc)) > 0
union
select Convert(tinyint, 10),  msstage10desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage10desc)) > 0
union
select Convert(tinyint, 11) ,  msstage11desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage11desc)) > 0
union
select Convert(tinyint, 12),  msstage12desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage12desc)) > 0
union
select Convert(tinyint, 13) ,  msstage13desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage13desc)) > 0
union
select Convert(tinyint, 14),  msstage14desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage14desc)) > 0
union
select Convert(tinyint, 15) ,  msstage15desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage15desc)) > 0
union
select Convert(tinyint, 16) ,  msstage16desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage16desc)) > 0
union
select Convert(tinyint, 17) ,  msstage17desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage17desc)) > 0
union
select Convert(tinyint, 18),  msstage18desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage18desc)) > 0
union
select Convert(tinyint, 19),  msstage19desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage19desc)) > 0
union
select Convert(tinyint, 20),  msstage20desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage20desc)) > 0
union
select Convert(tinyint, 21) ,  msstage21desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage21desc)) > 0
union
select Convert(tinyint, 22),  msstage22desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage22desc)) > 0
union
select Convert(tinyint, 23) ,  msstage23desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage23desc)) > 0
union
select Convert(tinyint, 24),  msstage24desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage24desc)) > 0
union
select Convert(tinyint, 25) ,  msstage25desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage25desc)) > 0
union
select Convert(tinyint, 26) ,  msstage26desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage26desc)) > 0
union
select Convert(tinyint, 27) ,  msstage27desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage27desc)) > 0
union
select Convert(tinyint, 28),  msstage28desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage28desc)) > 0
union
select Convert(tinyint, 29),  msstage29desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage29desc)) > 0
union
select Convert(tinyint, 30),  msstage30desc from dbo.dbMSConfig_OMS2K where mscode = @plan and len(rtrim(msstage30desc)) > 0
GO


