﻿CREATE TYPE search.ESRelationship AS TABLE
(
	Id UNIQUEIDENTIFIER NOT NULL
	, RelPK BIGINT NOT NULL
, PRIMARY KEY CLUSTERED (RelPK, Id)
)