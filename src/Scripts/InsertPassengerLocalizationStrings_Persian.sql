-- =============================================
-- Script to insert Passenger localization strings in Persian (Farsi)
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
DECLARE @LanguageId INT = 3; -- TODO: Replace 2 with your actual Persian language ID

-- =============================================
-- Admin Passenger Strings
-- =============================================

-- Main Passenger strings
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Passengers' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Passengers', N'مسافران');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Passengers.AddNew' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Passengers.AddNew', N'افزودن مسافر جدید');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Passengers.EditPassengerDetails' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Passengers.EditPassengerDetails', N'ویرایش جزئیات مسافر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Passengers.BackToList' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Passengers.BackToList', N'بازگشت به فهرست');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Passengers.Added' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Passengers.Added', N'مسافر با موفقیت افزوده شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Passengers.Updated' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Passengers.Updated', N'مسافر با موفقیت به‌روزرسانی شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Passengers.Deleted' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Passengers.Deleted', N'مسافر با موفقیت حذف شد');

-- Passenger Card Titles
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Passengers.RecoveryInformation' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Passengers.RecoveryInformation', N'اطلاعات بازیابی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Passengers.LocationInformation' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Passengers.LocationInformation', N'اطلاعات محل و راهنما');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Passengers.PersonalInformation' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Passengers.PersonalInformation', N'اطلاعات شخصی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Passengers.CardTravelInformation' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Passengers.CardTravelInformation', N'اطلاعات کارت و سفر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Passengers.Picture' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Passengers.Picture', N'تصویر فرم رهایی');

-- Passenger Fields
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.RecoveryNo' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.RecoveryNo', N'شماره بازیابی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.RecoveryYear' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.RecoveryYear', N'سال بازیابی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.RecoveryMonth' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.RecoveryMonth', N'ماه بازیابی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.PersonName' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.PersonName', N'نام شخص');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.BranchName' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.BranchName', N'نام شعبه');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.GuideNameAndLegionNo' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.GuideNameAndLegionNo', N'نام راهنما و شماره لژیون');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.Clinic' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.Clinic', N'کلینیک');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.BirthYear' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.BirthYear', N'سال تولد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.BirthYear.Range' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.BirthYear.Range', N'سال تولد باید بین 1300 تا 1400 باشد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.Education' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.Education', N'سطح تحصیلات');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.IsMarried' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.IsMarried', N'وضعیت تاهل');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.IsSingle' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.IsSingle', N'مجرد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.IsEmployed' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.IsEmployed', N'وضعیت اشتغال');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.IsUnemployed' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.IsUnemployed', N'بیکار');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.HasCompanion' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.HasCompanion', N'همسفر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.HasCompanion.Unknown' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.HasCompanion.Unknown', N'نامشخص');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.HasCompanion.No' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.HasCompanion.No', N'ندارد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.HasCompanion.Yes' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.HasCompanion.Yes', N'دارد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.CardNo' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.CardNo', N'شماره کارت');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.AntiX1' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.AntiX1', N'AntiX1');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.AntiX2' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.AntiX2', N'AntiX2');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.City' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.City', N'شهر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.Agency' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.Agency', N'نمایندگی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.TravelStartDate' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.TravelStartDate', N'تاریخ شروع سفر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.TravelEndDate' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.TravelEndDate', N'تاریخ پایان سفر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.PictureId' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.PictureId', N'شناسه تصویر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.CreatedOn' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.CreatedOn', N'تاریخ ایجاد');

-- Passenger Field Required Messages
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.PersonName.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.PersonName.Required', N'نام رهجو الزامی است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.RecoveryNo.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.RecoveryNo.Required', N'شماره رهایی الزامی است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.RecoveryNo.Duplicate' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.RecoveryNo.Duplicate', N'شماره رهایی قبلا ثبت شده است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.CardNo.DuplicateWarning' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.CardNo.DuplicateWarning', N'شماره کارت قبلا ثبت شده است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.GuideNameAndLegionNo.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.GuideNameAndLegionNo.Required', N'نام راهنما و شماره لژیون الزامی است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.CardNo.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.CardNo.Required', N'شماره کارت الزامی است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.TravelStartDate.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.TravelStartDate.Required', N'تاریخ شروع سفر الزامی است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.TravelEndDate.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.TravelEndDate.Required', N'تاریخ پایان سفر الزامی است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.AntiX1.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.AntiX1.Required', N'AntiX1 الزامی است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.Agency.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.Agency.Required', N'انتخاب نمایندگی الزامی است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.Fields.Clinic.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.Fields.Clinic.Required', N'انتخاب کلینیک الزامی است');

-- Passenger List Search Fields
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchRecoveryNo' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchRecoveryNo', N'شماره بازیابی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchCardNo' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchCardNo', N'شماره کارت');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchPersonName' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchPersonName', N'نام شخص');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchGuideNameAndLegionNo' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchGuideNameAndLegionNo', N'نام راهنما و شماره لژیون');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchCity' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchCity', N'شهر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchAgency' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchAgency', N'نمایندگی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchClinic' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchClinic', N'کلینیک');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchAntiX' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchAntiX', N'AntiX');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchTravelStartDate' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchTravelStartDate', N'تاریخ شروع سفر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchTravelEndDate' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchTravelEndDate', N'تاریخ پایان سفر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchRecoveryYear' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchRecoveryYear', N'سال رهایی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchRecoveryMonth' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchRecoveryMonth', N'ماه رهایی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.NoPassengers' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.NoPassengers', N'هیچ رهجویی انتخاب نشده است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Passengers.List.SearchPersonName' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Passengers.List.SearchPersonName', N'نام شخص');

-- Passenger Dashboard Charts
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.Passengers.MaritalStatusStatistics' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.Passengers.MaritalStatusStatistics', N'وضعیت تاهل');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.Passengers.MaritalStatusStatistics.Married' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.Passengers.MaritalStatusStatistics.Married', N'متاهل');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.Passengers.MaritalStatusStatistics.Single' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.Passengers.MaritalStatusStatistics.Single', N'مجرد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.Passengers.EmploymentStatusStatistics' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.Passengers.EmploymentStatusStatistics', N'وضعیت اشتغال');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.Passengers.EmploymentStatusStatistics.Employed' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.Passengers.EmploymentStatusStatistics.Employed', N'شاغل');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.Passengers.EmploymentStatusStatistics.Unemployed' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.Passengers.EmploymentStatusStatistics.Unemployed', N'بیکار');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.Passengers.AverageTravelLengthByAgency' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.Passengers.AverageTravelLengthByAgency', N'میانگین طول سفر به تفکیک نمایندگی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.Passengers.AverageTravelLengthByAgency.Label' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.Passengers.AverageTravelLengthByAgency.Label', N'میانگین طول سفر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.Passengers.AverageTravelLengthByAgency.Days' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.Passengers.AverageTravelLengthByAgency.Days', N'روز');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.Passengers.AgeDistribution' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.Passengers.AgeDistribution', N'توزیع سنی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.Reports.Passengers.EducationDistribution' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.Reports.Passengers.EducationDistribution', N'میزان تحصیلات');

-- Menu Item (if needed - usually handled by Admin.Customers)
-- Note: The menu item "Passengers list" uses the same permission as Customers, 
-- so it might use Admin.Customers resource or you may need to add Admin.Passengers.Menu if separate

-- =============================================
-- Enum Localization Strings
-- =============================================

-- EducationLevel Enum
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Enums.EducationLevel.Unknown' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'enums.nop.core.domain.EducationLevel.Unknown', N'نامشخص');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Enums.EducationLevel.Primary' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'enums.nop.core.domain.EducationLevel.Primary', N'ابتدایی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Enums.EducationLevel.MiddleSchool' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'enums.nop.core.domain.EducationLevel.MiddleSchool', N'راهنمایی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Enums.EducationLevel.HighSchool' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'enums.nop.core.domain.EducationLevel.HighSchool', N'دبیرستان');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Enums.EducationLevel.Diploma' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'enums.nop.core.domain.EducationLevel.Diploma', N'دیپلم');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Enums.EducationLevel.Associate' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'enums.nop.core.domain.EducationLevel.Associate', N'کاردانی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Enums.EducationLevel.Bachelor' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'enums.nop.core.domain.EducationLevel.Bachelor', N'کارشناسی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Enums.EducationLevel.Master' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'enums.nop.core.domain.EducationLevel.Master', N'کارشناسی ارشد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Enums.EducationLevel.Doctorate' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'enums.nop.core.domain.EducationLevel.Doctorate', N'دکترا');

-- =============================================
-- Activity Log Strings
-- =============================================

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.AddNewPassenger' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.AddNewPassenger', N'مسافر جدید با شناسه {0} افزوده شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.EditPassenger' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.EditPassenger', N'مسافر با شناسه {0} ویرایش شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.DeletePassenger' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.DeletePassenger', N'مسافر با شناسه {0} حذف شد');

GO

PRINT 'Passenger localization strings inserted successfully for Persian language.';
PRINT 'Please verify that @LanguageId is set to your Persian language ID.';
PRINT 'To find your Persian language ID, run: SELECT Id, Name, LanguageCulture FROM [Language] WHERE LanguageCulture LIKE ''%fa%'' OR Name LIKE ''%Persian%'' OR Name LIKE ''%Farsi%'';';

