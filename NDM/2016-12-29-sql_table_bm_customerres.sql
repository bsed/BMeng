GO

/****** Object:  Table [dbo].[BM_CustomerRes]    Script Date: 12/29/2016 16:14:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BM_CustomerRes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Addr] [nvarchar](200) NULL,
	[Remark] [nvarchar](500) NULL,
	[DataImg] [nvarchar](300) NULL,
	[SubmitName] [nvarchar](50) NULL,
	[Type] [int] NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_BM_CustomerRes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 Í¼Æ¬×ÊÁÏ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BM_CustomerRes', @level2type=N'COLUMN',@level2name=N'Type'
GO

ALTER TABLE [dbo].[BM_CustomerRes] ADD  CONSTRAINT [DF_BM_CustomerRes_UserId]  DEFAULT ((0)) FOR [UserId]
GO

ALTER TABLE [dbo].[BM_CustomerRes] ADD  CONSTRAINT [DF_BM_CustomerRes_Type]  DEFAULT ((0)) FOR [Type]
GO

ALTER TABLE [dbo].[BM_CustomerRes] ADD  CONSTRAINT [DF_BM_CustomerRes_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO


