UPDATE [config].[ObjectPolicyConfig] SET SecurityLevel = 512 WHERE [Byte] IN ( 4 , 5 )
GO

UPDATE [config].[ObjectPolicyConfig] SET SecurityLevel = 256 WHERE [Byte] IN ( 6 , 7 )
GO

UPDATE [config].[ObjectPolicyConfig] SET SecurityLevel = 128 WHERE [Byte] IN ( 8, 9 )
GO

UPDATE [config].[ObjectPolicyConfig] SET SecurityLevel = 1024 WHERE [Byte] = 10
GO

UPDATE [config].[ObjectPolicyConfig] SET Permission = 'VIEWCL' WHERE [Byte] = 5 AND [BitValue] = 128
GO

UPDATE [config].[ObjectPolicyConfig] SET Permission = 'VIEWFL' WHERE [Byte] = 6 AND [BitValue] = 32
GO

UPDATE [config].[ObjectPolicyConfig] SET Permission = 'VIEWDOC' WHERE [Byte] = 9 AND [BitValue] = 128
GO

UPDATE [config].[ObjectPolicyConfig] SET Permission = 'VIEWCONT' WHERE [Byte] = 10 AND [BitValue] = 128
GO
