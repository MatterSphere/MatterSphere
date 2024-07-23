CREATE TYPE dbo.EntityType AS TABLE
(
	EntityField VARCHAR(25)
	, EntityValue BIGINT
	, EntityPrimary BIT NOT NULL
)
