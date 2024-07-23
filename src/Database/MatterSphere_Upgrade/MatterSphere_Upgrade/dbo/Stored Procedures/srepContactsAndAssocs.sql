CREATE  PROCEDURE [dbo].[srepContactsAndAssocs]
	(@UI uUICultureInfo='{default}')

AS 

DECLARE @cdDescCLIENT NVARCHAR(1000) = dbo.getcodelookupdesc ( 'SUBASSOC' , 'CLIENT',  'en' )
	, @cdDescSOURCE NVARCHAR(1000) = dbo.getcodelookupdesc ( 'SUBASSOC' , 'SOURCE',  'en' )


SELECT 
                COALESCE(CL1.cdDesc, '~' + NULLIF(typecode, '') + '~') as cdDesc
                , @cdDescCLIENT + ' (always available)' as cdDesc_1
                , typecode as cdtype
FROM
                dbo.dbContactType 
LEFT JOIN dbo.GetCodeLookupDescription ( 'CONTTYPE', 'en' ) CL1 ON CL1.[cdCode] =  typecode
where typeActive = 1
                
UNION SELECT 
                COALESCE(CL1.cdDesc, '~' + NULLIF(typecode, '') + '~') as cdDesc
                , @cdDescSOURCE + ' (always available)'  as cdDesc_1
                , typecode as cdtype
FROM
                dbo.dbContactType 
LEFT JOIN dbo.GetCodeLookupDescription ( 'CONTTYPE', 'en' ) CL1 ON CL1.[cdCode] =  typecode
where typeActive = 1

UNION SELECT
                COALESCE(CL1.cdDesc, '~' + NULLIF(conttype, '') + '~') as cdDesc
                , COALESCE(CL2.cdDesc, '~' + NULLIF(assoctype, '') + '~') as cdDesc_1
                , LINK.conttype as cdtype
FROM 
                dbo.dbAssociatedTypes LINK

INNER JOIN 
                dbo.dbContactTYpe CONTTYPE ON CONTTYPE.typeCode = LINK.contType
INNER JOIN 
                dbo.dbAssociateType ASSOCTYPE ON ASSOCTYPE.typeCode = LINK.assocType
LEFT JOIN dbo.GetCodeLookupDescription ( 'CONTTYPE', 'en' ) CL1 ON CL1.[cdCode] =  conttype
LEFT JOIN dbo.GetCodeLookupDescription ( 'SUBASSOC', 'en' ) CL2 ON CL2.[cdCode] =  assoctype
WHERE
                CONTTYPE.typeActive = 1 AND 
                ASSOCTYPE.typeActive = 1
ORDER BY 
                cdDesc, cddesc_1
