GO

/****** Object:  Table [dbo].[BM_WorkReport]    Script Date: 12/29/2016 16:44:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BM_WorkReport](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WorkTitle] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_BM_WorkReport] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BM_WorkReport] ADD  CONSTRAINT [DF_BM_WorkReport_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO


