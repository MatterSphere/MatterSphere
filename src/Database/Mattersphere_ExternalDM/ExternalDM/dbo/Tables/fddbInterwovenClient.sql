﻿CREATE TABLE [dbo].[fddbInterwovenClient]
(
[ClientId] [bigint] NOT NULL,
[IntServer] [nvarchar](50) NULL,
[IntDatabase] [nvarchar](50) NULL,
CONSTRAINT [PK_fddbInterwovenClient] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO