-- =============================================
-- Passenger performance report localization (Persian)
-- =============================================
DECLARE @LanguageId INT = 3; -- TODO: Replace with your Persian language ID

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.PassengerPerformance' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.PassengerPerformance', N'گزارش عملکرد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.PassengerPerformance.Summary' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.PassengerPerformance.Summary', N'خلاصه کل سیستم');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.PassengerPerformance.ByUser' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.PassengerPerformance.ByUser', N'به تفکیک کاربر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.PassengerPerformance.SystemTotal' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.PassengerPerformance.SystemTotal', N'جمع کل سیستم');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.PassengerPerformance.Fields.User' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.PassengerPerformance.Fields.User', N'کاربر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.PassengerPerformance.Fields.Email' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.PassengerPerformance.Fields.Email', N'ایمیل');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.PassengerPerformance.Fields.CountToday' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.PassengerPerformance.Fields.CountToday', N'امروز');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.PassengerPerformance.Fields.CountThisWeek' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.PassengerPerformance.Fields.CountThisWeek', N'این هفته');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.PassengerPerformance.Fields.CountThisMonth' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.PassengerPerformance.Fields.CountThisMonth', N'این ماه');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.PassengerPerformance.Fields.CountThisYear' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.PassengerPerformance.Fields.CountThisYear', N'سال {0}');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.PassengerPerformance.Fields.CountAllTime' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.PassengerPerformance.Fields.CountAllTime', N'کل دوره');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.PassengerPerformance.Fields.CountUnattributed' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.PassengerPerformance.Fields.CountUnattributed', N'بدون ثبت‌کننده');
GO
