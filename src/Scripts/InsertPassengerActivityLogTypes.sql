-- =============================================
-- Seed ActivityLogType rows for Passenger CRUD logging
-- =============================================

IF NOT EXISTS (SELECT 1 FROM [ActivityLogType] WHERE [SystemKeyword] = 'AddNewPassenger')
    INSERT INTO [ActivityLogType] ([SystemKeyword], [Name], [Enabled])
    VALUES ('AddNewPassenger', N'افزودن فرم رهایی', 1);

IF NOT EXISTS (SELECT 1 FROM [ActivityLogType] WHERE [SystemKeyword] = 'EditPassenger')
    INSERT INTO [ActivityLogType] ([SystemKeyword], [Name], [Enabled])
    VALUES ('EditPassenger', N'ویرایش فرم رهایی', 1);

IF NOT EXISTS (SELECT 1 FROM [ActivityLogType] WHERE [SystemKeyword] = 'DeletePassenger')
    INSERT INTO [ActivityLogType] ([SystemKeyword], [Name], [Enabled])
    VALUES ('DeletePassenger', N'حذف فرم رهایی', 1);
GO
