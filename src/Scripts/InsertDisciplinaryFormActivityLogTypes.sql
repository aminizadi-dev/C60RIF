-- =============================================
-- Seed ActivityLogType rows for DisciplinaryForm CRUD logging
-- =============================================

IF NOT EXISTS (SELECT 1 FROM [ActivityLogType] WHERE [SystemKeyword] = 'AddNewDisciplinaryForm')
    INSERT INTO [ActivityLogType] ([SystemKeyword], [Name], [Enabled])
    VALUES ('AddNewDisciplinaryForm', N'افزودن فرم انضباطی', 1);

IF NOT EXISTS (SELECT 1 FROM [ActivityLogType] WHERE [SystemKeyword] = 'EditDisciplinaryForm')
    INSERT INTO [ActivityLogType] ([SystemKeyword], [Name], [Enabled])
    VALUES ('EditDisciplinaryForm', N'ویرایش فرم انضباطی', 1);

IF NOT EXISTS (SELECT 1 FROM [ActivityLogType] WHERE [SystemKeyword] = 'DeleteDisciplinaryForm')
    INSERT INTO [ActivityLogType] ([SystemKeyword], [Name], [Enabled])
    VALUES ('DeleteDisciplinaryForm', N'حذف فرم انضباطی', 1);
GO
