CREATE TABLE [dbo].App_Requirement  ( [Id] [varchar](50) NOT NULL,[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_App_Requirement_CreateTime]  DEFAULT (getdate()),[ModificationTime] [datetime] NOT NULL CONSTRAINT [DF_App_Requirement_ModificationTime]  DEFAULT (getdate()),[CreateUserName] [nvarchar](max) NULL,[ModificationUserName] [nvarchar](max) NULL,[SortCode] [int] NOT  NULL default 0,[ParentId] [varchar](50)  NULL,
Title nvarchar(max) NULL,
SubTitle nvarchar(max) NULL,
EnumPositionType nvarchar(max) NULL,
WorkPlace nvarchar(max) NULL,
WorkLimitSex nvarchar(max) NULL,
WorkLimitAgeLow int NULL default 0,
WorkLimitAgeHigh int NULL default 0,
EnumWorkLimitDegree nvarchar(max) NULL,
SalaryLow decimal(16,0) NULL,
SalaryHigh decimal(16,0) NULL,
TotalServiceMoney decimal(16,0) NULL,
PublishDate nvarchar(max) NULL,
TotalHire int NULL default 0,
Tag nvarchar(max) NULL,
PK_App_Customer_CustomerName nvarchar(max) NULL,
Description nvarchar(max) NULL,
SwitchBtnRecommend nvarchar(max) NULL,
 CONSTRAINT [PK_App_Requirement] PRIMARY KEY CLUSTERED 
( [Id] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
) ON [PRIMARY]
SET ANSI_PADDING OFF
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'Id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'CreateTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'ModificationTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'CreateUserName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'ModificationUserName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'SortCode'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联数据Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'ParentId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'Title'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'副标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'SubTitle'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'EnumPositionType'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作地点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'WorkPlace'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作要求性别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'WorkLimitSex'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作要求年龄(最低)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'WorkLimitAgeLow'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作要求年龄(最高)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'WorkLimitAgeHigh'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作要求学历' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'EnumWorkLimitDegree'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'报酬(最低)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'SalaryLow'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'报酬(最高)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'SalaryHigh'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'总服务费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'TotalServiceMoney'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'PublishDate'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'招聘人数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'TotalHire'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位标签' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'Tag'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'PK_App_Customer_CustomerName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'Description'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'SwitchBtnRecommend'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'需求' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement'

