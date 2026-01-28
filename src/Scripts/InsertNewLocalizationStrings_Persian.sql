-- =============================================
-- Script to insert new localization strings in Persian (Farsi)
-- City, Agency, AntiX, ActivityLog
-- =============================================
-- Note: Replace @LanguageId with your Persian language ID from the Language table
-- You can find it by running: SELECT Id, Name FROM [Language] WHERE Name LIKE '%Persian%' OR Name LIKE '%Farsi%' OR LanguageCulture LIKE '%fa%'

-- =============================================
-- STEP 1: Find your Persian Language ID
-- =============================================
-- Uncomment and run this query first to find your Persian language ID:
-- SELECT Id, Name, LanguageCulture FROM [Language] WHERE LanguageCulture LIKE '%fa%' OR Name LIKE '%Persian%' OR Name LIKE '%Farsi%';

-- =============================================
-- STEP 2: Set the Language ID variable
-- =============================================
-- Replace the number below with your Persian language ID from Step 1
DECLARE @LanguageId INT = 3; -- TODO: Replace with your actual Persian language ID

-- =============================================
-- Admin City Strings
-- =============================================
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities', N'شهرها');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Added' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Added', N'شهر با موفقیت افزوده شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.AddNew' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.AddNew', N'افزودن شهر جدید');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.BackToList' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.BackToList', N'بازگشت به فهرست شهرها');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Deleted' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Deleted', N'شهر با موفقیت حذف شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.EditCityDetails' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.EditCityDetails', N'ویرایش جزئیات شهر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Info' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Info', N'اطلاعات شهر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.PublishSelected' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.PublishSelected', N'انتشار انتخاب‌شده‌ها');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.UnpublishSelected' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.UnpublishSelected', N'عدم انتشار انتخاب‌شده‌ها');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Updated' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Updated', N'شهر با موفقیت به‌روزرسانی شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Fields.Name' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Fields.Name', N'نام');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Fields.Name.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Fields.Name.Required', N'نام شهر الزامی است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Fields.Published' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Fields.Published', N'منتشر شده');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Fields.DisplayOrder' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Fields.DisplayOrder', N'ترتیب نمایش');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Fields.NumberOfAgencies' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Fields.NumberOfAgencies', N'تعداد نمایندگی‌ها');

-- =============================================
-- Admin Agency Strings
-- =============================================
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Agencies' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Agencies', N'نمایندگی‌ها');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Agencies.AddNew' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Agencies.AddNew', N'افزودن نمایندگی جدید');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Agencies.Edit' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Agencies.Edit', N'ویرایش نمایندگی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Agencies.SaveBeforeEdit' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Agencies.SaveBeforeEdit', N'برای مدیریت نمایندگی‌ها ابتدا شهر را ذخیره کنید');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Agencies.Fields.Name' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Agencies.Fields.Name', N'نام');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Agencies.Fields.Name.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Agencies.Fields.Name.Required', N'نام نمایندگی الزامی است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Agencies.Fields.Published' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Agencies.Fields.Published', N'منتشر شده');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.Cities.Agencies.Fields.DisplayOrder' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.Cities.Agencies.Fields.DisplayOrder', N'ترتیب نمایش');

-- =============================================
-- Admin AntiX Strings
-- =============================================
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX', N'آنتی‌ایکس');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX.Added' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX.Added', N'آیتم آنتی‌ایکس با موفقیت افزوده شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX.AddNew' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX.AddNew', N'افزودن آنتی‌ایکس جدید');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX.BackToList' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX.BackToList', N'بازگشت به فهرست آنتی‌ایکس');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX.Deleted' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX.Deleted', N'آیتم آنتی‌ایکس با موفقیت حذف شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX.EditAntiXDetails' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX.EditAntiXDetails', N'ویرایش آنتی‌ایکس');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX.PublishSelected' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX.PublishSelected', N'انتشار انتخاب‌شده‌ها');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX.UnpublishSelected' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX.UnpublishSelected', N'عدم انتشار انتخاب‌شده‌ها');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX.Updated' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX.Updated', N'آیتم آنتی‌ایکس با موفقیت به‌روزرسانی شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX.Fields.Name' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX.Fields.Name', N'نام');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX.Fields.Name.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX.Fields.Name.Required', N'نام آنتی‌ایکس الزامی است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX.Fields.Published' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX.Fields.Published', N'منتشر شده');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Configuration.AntiX.Fields.DisplayOrder' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Configuration.AntiX.Fields.DisplayOrder', N'ترتیب نمایش');

-- =============================================
-- Activity Log Strings
-- =============================================
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.AddNewCity' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.AddNewCity', N'شهر جدید با شناسه {0} افزوده شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.EditCity' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.EditCity', N'شهر با شناسه {0} ویرایش شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.DeleteCity' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.DeleteCity', N'شهر با شناسه {0} حذف شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.AddNewAgency' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.AddNewAgency', N'نمایندگی جدید با شناسه {0} افزوده شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.EditAgency' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.EditAgency', N'نمایندگی با شناسه {0} ویرایش شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.DeleteAgency' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.DeleteAgency', N'نمایندگی با شناسه {0} حذف شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.AddNewAntiX' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.AddNewAntiX', N'آیتم آنتی‌ایکس جدید با شناسه {0} افزوده شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.EditAntiX' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.EditAntiX', N'آیتم آنتی‌ایکس با شناسه {0} ویرایش شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.DeleteAntiX' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.DeleteAntiX', N'آیتم آنتی‌ایکس با شناسه {0} حذف شد');

GO

PRINT 'New localization strings inserted successfully for Persian language.';
PRINT 'Please verify that @LanguageId is set to your Persian language ID.';
PRINT 'To find your Persian language ID, run: SELECT Id, Name, LanguageCulture FROM [Language] WHERE LanguageCulture LIKE ''%fa%'' OR Name LIKE ''%Persian%'' OR Name LIKE ''%Farsi%'';';

