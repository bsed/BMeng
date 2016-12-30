GO

/****** Object:  Table [dbo].[BM_Mail]    Script Date: 12/28/2016 09:29:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BM_Mail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AuthorId] [int] NULL,
	[AuthorName] [nvarchar](50) NULL,
	[SendType] [int] NULL,
	[Title] [nvarchar](50) NULL,
	[BodyContent] [nvarchar](max) NULL,
	[CoverUrl] [nvarchar](300) NULL,
	[SendTime] [datetime] NULL,
	[IsRead] [int] NULL,
	[ReplyUserId] [int] NULL,
	[ReplyPid] [int] NULL,
	[ReplyTime] [datetime] NULL,
 CONSTRAINT [PK_BM_Mail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送类型 0盟主发给盟友，1盟友发给盟主，2发给霸盟' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BM_Mail', @level2type=N'COLUMN',@level2name=N'SendType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息发送时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BM_Mail', @level2type=N'COLUMN',@level2name=N'SendTime'
GO

ALTER TABLE [dbo].[BM_Mail] ADD  CONSTRAINT [DF_BM_Mail_AuthorId]  DEFAULT ((0)) FOR [AuthorId]
GO

ALTER TABLE [dbo].[BM_Mail] ADD  CONSTRAINT [DF_BM_Mail_SendType]  DEFAULT ((0)) FOR [SendType]
GO

ALTER TABLE [dbo].[BM_Mail] ADD  CONSTRAINT [DF_BM_Mail_SendTime]  DEFAULT (getdate()) FOR [SendTime]
GO

ALTER TABLE [dbo].[BM_Mail] ADD  CONSTRAINT [DF_BM_Mail_IsRead]  DEFAULT ((0)) FOR [IsRead]
GO

ALTER TABLE [dbo].[BM_Mail] ADD  CONSTRAINT [DF_BM_Mail_ReplyPid]  DEFAULT ((0)) FOR [ReplyPid]
GO

ALTER TABLE [dbo].[BM_Mail] ADD  CONSTRAINT [DF_BM_Mail_ReplyTime]  DEFAULT (getdate()) FOR [ReplyTime]
GO


