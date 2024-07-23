CREATE TABLE search.BlackList
(
	Extension NVARCHAR(15) NOT NULL
	, [Contains] NVARCHAR(1000) NOT NULL
	, EncodingType VARCHAR(30) NOT NULL
	, MaxSize BIGINT NULL
	, CONSTRAINT PK_BlackList PRIMARY KEY CLUSTERED (Extension, [Contains], EncodingType)
)
