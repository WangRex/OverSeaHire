alter table APP_Customer add EnumCustomerType nvarchar(max) NULL;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户身份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'APP_Customer', @level2type=N'COLUMN',@level2name=N'EnumCustomerType'