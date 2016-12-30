GO

/****** Object:  Table [dbo].[BM_MailReadLog]    Script Date: 12/28/2016 09:31:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BM_MailReadLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[MailId] [int] NULL,
	[IsRead] [int] NULL,
	[ClientIp] [nvarchar](50) NULL,
	[cookie] [nvarchar](50) NULL,
	[ReadTime] [datetime] NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_BM_MailReadLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BM_MailReadLog] ADD  CONSTRAINT [DF_BM_MailReadLog_UserId]  DEFAULT ((0)) FOR [UserId]
GO

ALTER TABLE [dbo].[BM_MailReadLog] ADD  CONSTRAINT [DF_BM_MailReadLog_MailId]  DEFAULT ((0)) FOR [MailId]
GO

ALTER TABLE [dbo].[BM_MailReadLog] ADD  CONSTRAINT [DF_BM_MailReadLog_IsRead]  DEFAULT ((0)) FOR [IsRead]
GO

ALTER TABLE [dbo].[BM_MailReadLog] ADD  CONSTRAINT [DF_BM_MailReadLog_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO


