GO

/****** Object:  Table [dbo].[BM_UserReport]    Script Date: 12/29/2016 16:45:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BM_UserReport](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ShopId] int NULL,
	[ReportTitle] [nvarchar](50) NULL,
	[Addr] [nvarchar](50) NULL,
	[JsonContent] [nvarchar](max) NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_BM_UserReport] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


