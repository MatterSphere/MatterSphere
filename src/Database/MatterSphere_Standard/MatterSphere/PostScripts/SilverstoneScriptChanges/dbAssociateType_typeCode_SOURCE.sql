Print 'dbAssociateType_typeCode_SOURCE.sql'


IF NOT EXISTS ( SELECT typecode FROM dbAssociateType WHERE typecode = 'SOURCE')
      INSERT dbAssociateType (typecode, typeversion, typexml) 
      VALUES ('SOURCE', 1, '<Config><Dialog><Form lookup="ASSOCCAPTION" /><Tabs><Tab lookup="ASSOCDETAILS" glyph="23" source="SCRASSEDIT" tabtype="Enquiry" /></Tabs><Panels /></Dialog><Settings /><defaultTemplates /></Config>')
GO




