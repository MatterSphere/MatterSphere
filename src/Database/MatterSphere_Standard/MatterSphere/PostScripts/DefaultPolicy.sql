print 'Start DefaultPolicy'

IF NOT EXISTS 
(
	SELECT 
		*
	FROM
		config.SystemPolicy
	WHERE
		IsDefault = 1
)
BEGIN 
	UPDATE 
		config.SystemPolicy
	SET 
		IsDefault = 1
	WHERE
		ID = '3cc3bd00-7d7e-4d4a-96c6-44e44e140c5e'	
END