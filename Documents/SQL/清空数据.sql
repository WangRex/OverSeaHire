USE [AppsDBOverseasHire]
GO

DELETE FROM [dbo].App_Customer;

DELETE FROM [dbo].App_ApplyJobRecord;

DELETE FROM [dbo].App_ApplyJob;

DELETE FROM [dbo].App_Requirement;

DELETE FROM [dbo].SysLog;

DELETE FROM [dbo].SysMessage;

DELETE FROM [dbo].SysUser where username not in (
'ohadmin'
,'admin'
);
GO


