-- =============================================
-- Script to insert DisciplinaryForm localization strings in Persian (Farsi)
-- =============================================
-- Find Persian language ID:
-- SELECT Id, Name, LanguageCulture FROM [Language] WHERE LanguageCulture LIKE '%fa%' OR Name LIKE '%Persian%' OR Name LIKE '%Farsi%';

DECLARE @LanguageId INT = 3; -- TODO: Replace with your Persian language ID

-- =============================================
-- Admin DisciplinaryForm strings
-- =============================================

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.DisciplinaryForms' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.DisciplinaryForms', N'فرم‌های انضباطی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.AddNew' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.AddNew', N'افزودن فرم انضباطی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Edit' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Edit', N'ویرایش فرم انضباطی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.BackToList' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.BackToList', N'بازگشت به فهرست');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Added' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Added', N'فرم انضباطی با موفقیت ثبت شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Updated' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Updated', N'فرم انضباطی با موفقیت به‌روزرسانی شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Deleted' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Deleted', N'فرم انضباطی با موفقیت حذف شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.LookupPassenger' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.LookupPassenger', N'جستجو');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.PassengerNotFound' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.PassengerNotFound', N'مسافری با این شماره کارت یافت نشد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.PassengerAlreadyHasForm' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.PassengerAlreadyHasForm', N'برای این مسافر قبلاً فرم انضباطی ثبت شده است');

-- List search
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.List.SearchPersonName' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.List.SearchPersonName', N'نام');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.List.SearchFamilyName' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.List.SearchFamilyName', N'نام خانوادگی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.List.SearchCardNo' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.List.SearchCardNo', N'شماره کارت');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.List.SearchAgencyName' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.List.SearchAgencyName', N'نمایندگی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.List.SearchCreatedFrom' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.List.SearchCreatedFrom', N'تاریخ ثبت از');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.List.SearchCreatedTo' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.List.SearchCreatedTo', N'تاریخ ثبت تا');

-- Fields
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.PersonName' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.PersonName', N'نام');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.PersonName.Required' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.PersonName.Required', N'نام الزامی است');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.FamilyName' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.FamilyName', N'نام خانوادگی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.CardNo' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.CardNo', N'شماره کارت');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.PassengerId' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.PassengerId', N'شناسه مسافر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.Age' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.Age', N'سن');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.IsMarried' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.IsMarried', N'وضعیت تأهل');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.IsEmployed' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.IsEmployed', N'وضعیت شغلی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.EducationLevel' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.EducationLevel', N'میزان تحصیلات');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.AgencyName' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.AgencyName', N'نمایندگی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.Agency' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.Agency', N'نمایندگی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.LegionNo' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.LegionNo', N'شماره لژیون');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.PreviousSubstanceUseDetails' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.PreviousSubstanceUseDetails', N'نوع و میزان ماده مصرف قبلی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.CurrentSubstanceUseDetails' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.CurrentSubstanceUseDetails', N'نوع و میزان ماده مصرف فعلی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.CreatedOn' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.CreatedOn', N'تاریخ ثبت');

-- Sections
IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.PersonalInfo' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.PersonalInfo', N'اطلاعات هویتی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.ReferralReasons' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.ReferralReasons', N'علت ارجاع به لژیون انضباطی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.LeftFirstTrip' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.LeftFirstTrip', N'خروج از پروژه درمان در سفر اول');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.Relapse' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.Relapse', N'برگشت به مصرف مواد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.Indiscipline' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.Indiscipline', N'بی‌انضباطی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.AbsenceDuration' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.AbsenceDuration', N'غیبت');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.AbsenceLessThanSixMonths' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.AbsenceLessThanSixMonths', N'کمتر از ۶ ماه');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.AbsenceMoreThanSixMonths' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.AbsenceMoreThanSixMonths', N'بیشتر از ۶ ماه');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.RetravelLessThanOneCcOt' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.RetravelLessThanOneCcOt', N'سفر مجدد - مصرف کمتر از ۱ سی‌سی اوتی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.RelapseDuration' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.RelapseDuration', N'زمان برگشت');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.RelapseLessThanOneYear' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.RelapseLessThanOneYear', N'کمتر از یکسال پس از رهایی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.RelapseMoreThanOneYear' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.RelapseMoreThanOneYear', N'بیشتر از یکسال پس از رهایی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.WritingCd' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.WritingCd', N'نوشتن سی‌دی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.Other' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.Other', N'سایر موارد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.ReferralReasonOtherDetails' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.ReferralReasonOtherDetails', N'توضیح سایر موارد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.TravelStatus' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.TravelStatus', N'وضعیت سفر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.IsCurrentSubstanceUseBelowHalfGram' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.IsCurrentSubstanceUseBelowHalfGram', N'مصرف فعلی کمتر از نیم گرم');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.HasSubstanceUseInAnotherBranch' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.HasSubstanceUseInAnotherBranch', N'مصرف در شعبه دیگر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.CigaretteAndWeight' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.CigaretteAndWeight', N'وضعیت سیگار و اضافه وزن');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.CigaretteUsage' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.CigaretteUsage', N'وضعیت سیگار');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.HasCigaretteUse' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.HasCigaretteUse', N'آیا مصرف سیگار دارید؟');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.CigaretteTreatmentStatus' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.CigaretteTreatmentStatus', N'سابقه لژیون ویلیام وایت');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.CigaretteNoAction' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.CigaretteNoAction', N'برای درمان سیگار اقدام نکرده‌ام');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.CigaretteWilliamWhiteUnsuccessful' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.CigaretteWilliamWhiteUnsuccessful', N'سابقه حضور در لژیون ویلیام وایت ولی موفق به درمان نشده‌ام');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.CigaretteHasRelease' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.CigaretteHasRelease', N'دارای رهایی سیگار');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.WeightStatus' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.WeightStatus', N'وضعیت اضافه وزن');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.HeightCm' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.HeightCm', N'قد (سانتی‌متر)');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.WeightKg' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.WeightKg', N'وزن (کیلوگرم)');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.HasOverOrUnderWeight' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.HasOverOrUnderWeight', N'اضافه یا کمبود وزن دارید؟');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.HadHealthyDietInPastYear' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.HadHealthyDietInPastYear', N'عضو لژیون تغذیه سالم بوده‌اید؟');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.RelapsedDueToWeightIssue' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.RelapsedDueToWeightIssue', N'روش کاهش/افزایش وزن باعث سفر ناموفق شده؟');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.HadHealthyWeightBeforeRecovery' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.HadHealthyWeightBeforeRecovery', N'قبل از رهایی وزن سالم داشتید؟');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.EducationalResources' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.EducationalResources', N'استفاده از منابع آموزشی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.JahanbiniBooklet' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.JahanbiniBooklet', N'جزوه جهان‌بینی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.SixtyDegreeBook' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.SixtyDegreeBook', N'کتاب ۶۰ درجه');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.EshghBook' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.EshghBook', N'کتاب عشق');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.TwelveArticles' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.TwelveArticles', N'۱۴ مقاله');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.WroteOneCdPerWeek' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.WroteOneCdPerWeek', N'نوشتن هفتگی یک سی‌دی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.CompletedThirtyCdExam' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.CompletedThirtyCdExam', N'تکمیل فرم ۳۰ سی‌دی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.CompletedFortyCdExam' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.CompletedFortyCdExam', N'تکمیل فرم ۴۰ سی‌دی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.ParksAndSports' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.ParksAndSports', N'حضور در پارک و انجام ورزش');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.AttendedParksDuringFirstSixMonths' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.AttendedParksDuringFirstSixMonths', N'حضور در پارک‌های ورزشی در ۶ ماه اول سفر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.ParticipatedInAtLeastOneSport' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.ParticipatedInAtLeastOneSport', N'فعالیت در یکی از رشته‌های ورزشی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.ParticipatedInSportsActivitiesOrCompetitions' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.ParticipatedInSportsActivitiesOrCompetitions', N'حضور در مسابقات و فعالیت‌های ورزشی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.OrderAndDiscipline' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.OrderAndDiscipline', N'نظم و انضباط - نحوه حضور و فعالیت');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.HasRegularWorkshopAttendance' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.HasRegularWorkshopAttendance', N'حضور منظم در کارگاه‌های آموزشی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.HasRegularLegionAttendance' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.HasRegularLegionAttendance', N'حضور منظم در لژیون');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.HadWeeklyParticipationAndTravelDeclarationInFirstTrip' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.HadWeeklyParticipationAndTravelDeclarationInFirstTrip', N'مشارکت و اعلام سفر هفتگی در سفر اول');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.TookSecondTripExams' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.TookSecondTripExams', N'شرکت در آزمون‌های سفر دوم');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.ServiceRoles' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.ServiceRoles', N'جایگاه‌های خدمتی سفر دوم');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.ServiceRolePublications' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.ServiceRolePublications', N'نشریات');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.ServiceRoleOt' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.ServiceRoleOt', N'اوتی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.ServiceRoleSite' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.ServiceRoleSite', N'سایت');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.ServiceRoleGuard' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.ServiceRoleGuard', N'نگهبان');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.ServiceRoleSecretary' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.ServiceRoleSecretary', N'دبیر');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.ServiceRoleGuide' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.ServiceRoleGuide', N'راهنما');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.ServiceRoleNewcomer' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.ServiceRoleNewcomer', N'تازه‌واردین');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.ServiceRoleMarzban' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.ServiceRoleMarzban', N'مرزبان');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Sections.FamilyRelapseFactors' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Sections.FamilyRelapseFactors', N'عامل برگشت به اعتیاد از دیدگاه فرد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.FamilyRelapseCigaretteUse' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.FamilyRelapseCigaretteUse', N'مصرف سیگار');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.FamilyRelapseSickness' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.FamilyRelapseSickness', N'بیماری');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.FamilyRelapseFinancialAndMarital' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.FamilyRelapseFinancialAndMarital', N'مسائل جنسی و زناشویی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.FamilyRelapseFamilyIssues' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.FamilyRelapseFamilyIssues', N'مشکلات خانوادگی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.FamilyRelapseEconomicIssues' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.FamilyRelapseEconomicIssues', N'مشکلات اقتصادی');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.FamilyRelapseOverweight' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.FamilyRelapseOverweight', N'اضافه وزن');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.FamilyRelapseFactorOtherDetails' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.FamilyRelapseFactorOtherDetails', N'سایر موارد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'Admin.DisciplinaryForms.Fields.MedicalConditionAndMedicationNotes' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'Admin.DisciplinaryForms.Fields.MedicalConditionAndMedicationNotes', N'بیماری خاص یا مصرف دارو');

-- =============================================
-- Activity log strings
-- =============================================

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.AddNewDisciplinaryForm' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.AddNewDisciplinaryForm', N'فرم انضباطی جدید با شناسه {0} ثبت شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.EditDisciplinaryForm' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.EditDisciplinaryForm', N'فرم انضباطی با شناسه {0} ویرایش شد');

IF NOT EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE [ResourceName] = 'ActivityLog.DeleteDisciplinaryForm' AND [LanguageId] = @LanguageId)
    INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (@LanguageId, 'ActivityLog.DeleteDisciplinaryForm', N'فرم انضباطی با شناسه {0} حذف شد');

GO

PRINT 'DisciplinaryForm localization strings inserted successfully for Persian language.';
PRINT 'Please verify that @LanguageId is set to your Persian language ID.';
PRINT 'To find your Persian language ID, run: SELECT Id, Name, LanguageCulture FROM [Language] WHERE LanguageCulture LIKE ''%fa%'' OR Name LIKE ''%Persian%'' OR Name LIKE ''%Farsi%'';';
