UPDATE [dbo].[dbCodeLookup]
   SET [cdDesc] = 'Override Search'
 WHERE [cdType] = 'ENQQUESTION' and
	   [cdCode] = '-586089734' and
	   lower(rtrim([cdDesc])) = 'override tickford search'
GO