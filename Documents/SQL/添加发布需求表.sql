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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'Id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'CreateTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'ModificationTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'CreateUserName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�޸�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'ModificationUserName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'SortCode'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'ParentId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'Title'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'SubTitle'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ְλ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'EnumPositionType'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ص�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'WorkPlace'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ҫ���Ա�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'WorkLimitSex'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ҫ������(���)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'WorkLimitAgeLow'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ҫ������(���)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'WorkLimitAgeHigh'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ҫ��ѧ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'EnumWorkLimitDegree'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����(���)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'SalaryLow'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����(���)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'SalaryHigh'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ܷ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'TotalServiceMoney'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'PublishDate'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ƹ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'TotalHire'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ְλ��ǩ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'Tag'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'PK_App_Customer_CustomerName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'Description'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƽ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement', @level2type=N'COLUMN',@level2name=N'SwitchBtnRecommend'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App_Requirement'

