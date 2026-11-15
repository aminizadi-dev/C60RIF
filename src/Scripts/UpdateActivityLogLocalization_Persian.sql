-- =============================================
-- Activity log page localization updates (Persian)
-- - Customer name column title
-- - Add-passenger message uses RecoveryNo and CardNo
-- =============================================
DECLARE @LanguageId INT = 3; -- Persian language ID

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Customers.ActivityLog.Fields.CustomerName' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Customers.ActivityLog.Fields.CustomerName', N'نام کاربر');
ELSE
    UPDATE [LocaleStringResource] SET [ResourceValue] = N'نام کاربر'
    WHERE [ResourceName] = 'Admin.Customers.ActivityLog.Fields.CustomerName' AND [LanguageId] = @LanguageId;

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Customers.ActivityLog.Fields.CustomerName.Hint' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Customers.ActivityLog.Fields.CustomerName.Hint', N'نام و نام خانوادگی کاربر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Customers.ActivityLog.Fields.Comment' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Customers.ActivityLog.Fields.Comment', N'پیام');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.AddNewPassenger' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.AddNewPassenger', N'رهجو جدید با شماره رهایی {0} و شماره کارت {1} افزوده شد');
ELSE
    UPDATE [LocaleStringResource] SET [ResourceValue] = N'رهجو جدید با شماره رهایی {0} و شماره کارت {1} افزوده شد'
    WHERE [ResourceName] = 'ActivityLog.AddNewPassenger' AND [LanguageId] = @LanguageId;
