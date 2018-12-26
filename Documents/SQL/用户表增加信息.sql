
alter table APP_Customer add Age int not NULL;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'年龄' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'APP_Customer', @level2type=N'COLUMN',@level2name=N'Age'

alter table APP_Customer add Height nvarchar(max) NULL;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身高' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'APP_Customer', @level2type=N'COLUMN',@level2name=N'Height'

alter table APP_Customer add Weight nvarchar(max) NULL;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'体重' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'APP_Customer', @level2type=N'COLUMN',@level2name=N'Weight'

alter table APP_Customer add Nation nvarchar(max) NULL;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'民族' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'APP_Customer', @level2type=N'COLUMN',@level2name=N'Nation'

alter table APP_Customer add Introduction nvarchar(max) NULL;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'个人简介' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'APP_Customer', @level2type=N'COLUMN',@level2name=N'Introduction'

alter table APP_Customer add WordPath nvarchar(max) NULL;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Word简历' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'APP_Customer', @level2type=N'COLUMN',@level2name=N'WordPath'

alter table APP_Customer add VideoPath nvarchar(max) NULL;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'视频路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'APP_Customer', @level2type=N'COLUMN',@level2name=N'VideoPath'
