# اسکریپت درج رشته‌های Localization برای Passenger به فارسی

این فایل شامل اسکریپت SQL برای درج تمام رشته‌های localization مربوط به Passenger به زبان فارسی است.

## روش استفاده

### مرحله 1: پیدا کردن شناسه زبان فارسی

ابتدا باید شناسه زبان فارسی را در دیتابیس پیدا کنید:

```sql
SELECT Id, Name, LanguageCulture FROM [Language] 
WHERE LanguageCulture LIKE '%fa%' 
   OR Name LIKE '%Persian%' 
   OR Name LIKE '%Farsi%';
```

### مرحله 2: تنظیم متغیر @LanguageId

در فایل `InsertPassengerLocalizationStrings_Persian.sql`، متغیر `@LanguageId` را با شناسه زبان فارسی که در مرحله قبل پیدا کردید جایگزین کنید.

**روش 1: استفاده از متغیر SQL**
```sql
DECLARE @LanguageId INT = 2; -- شناسه زبان فارسی خود را اینجا قرار دهید
-- سپس بقیه اسکریپت را اجرا کنید
```

**روش 2: جایگزینی مستقیم**
تمام `@LanguageId` را در فایل با شناسه زبان فارسی جایگزین کنید (مثلاً `2`)

### مرحله 3: اجرای اسکریپت

اسکریپت را در SQL Server Management Studio اجرا کنید.

## رشته‌های Localization شامل شده

### رشته‌های Admin
- `Admin.Passengers.Passengers` - مسافران
- `Admin.Passengers.Passengers.AddNew` - افزودن مسافر جدید
- `Admin.Passengers.Passengers.EditPassengerDetails` - ویرایش جزئیات مسافر
- `Admin.Passengers.Passengers.BackToList` - بازگشت به فهرست
- `Admin.Passengers.Passengers.Added` - مسافر با موفقیت افزوده شد
- `Admin.Passengers.Passengers.Updated` - مسافر با موفقیت به‌روزرسانی شد
- `Admin.Passengers.Passengers.Deleted` - مسافر با موفقیت حذف شد

### فیلدهای Passenger
- `Admin.Passengers.Fields.RecoveryNo` - شماره بازیابی
- `Admin.Passengers.Fields.RecoveryYear` - سال بازیابی
- `Admin.Passengers.Fields.RecoveryMonth` - ماه بازیابی
- `Admin.Passengers.Fields.PersonName` - نام شخص
- `Admin.Passengers.Fields.BranchName` - نام شعبه
- `Admin.Passengers.Fields.GuideNameAndLegionNo` - نام راهنما و شماره لژیون
- `Admin.Passengers.Fields.ClinicName` - نام کلینیک
- `Admin.Passengers.Fields.BirthDate` - تاریخ تولد
- `Admin.Passengers.Fields.Education` - سطح تحصیلات
- `Admin.Passengers.Fields.MaritalStatus` - وضعیت تاهل
- `Admin.Passengers.Fields.EmploymentStatus` - وضعیت اشتغال
- `Admin.Passengers.Fields.CardNo` - شماره کارت
- `Admin.Passengers.Fields.AntiX1` - AntiX1
- `Admin.Passengers.Fields.AntiX2` - AntiX2
- `Admin.Passengers.Fields.TravelStartDate` - تاریخ شروع سفر
- `Admin.Passengers.Fields.TravelEndDate` - تاریخ پایان سفر
- `Admin.Passengers.Fields.PictureId` - شناسه تصویر
- `Admin.Passengers.Fields.CreatedOn` - تاریخ ایجاد

### فیلدهای جستجو
- `Admin.Passengers.List.SearchRecoveryNo` - شماره بازیابی
- `Admin.Passengers.List.SearchRecoveryYear` - سال بازیابی
- `Admin.Passengers.List.SearchPersonName` - نام شخص

### Enum ها

#### EducationLevel (سطح تحصیلات)
- `Enums.EducationLevel.Unknown` - نامشخص
- `Enums.EducationLevel.Primary` - ابتدایی
- `Enums.EducationLevel.MiddleSchool` - راهنمایی
- `Enums.EducationLevel.HighSchool` - دبیرستان
- `Enums.EducationLevel.Diploma` - دیپلم
- `Enums.EducationLevel.Associate` - کاردانی
- `Enums.EducationLevel.Bachelor` - کارشناسی
- `Enums.EducationLevel.Master` - کارشناسی ارشد
- `Enums.EducationLevel.Doctorate` - دکترا

#### MaritalStatus (وضعیت تاهل)
- `Enums.MaritalStatus.Unknown` - نامشخص
- `Enums.MaritalStatus.Single` - مجرد
- `Enums.MaritalStatus.Married` - متاهل

#### EmploymentStatus (وضعیت اشتغال)
- `Enums.EmploymentStatus.Unknown` - نامشخص
- `Enums.EmploymentStatus.Employed` - شاغل
- `Enums.EmploymentStatus.Unemployed` - بیکار

### Activity Log
- `ActivityLog.AddNewPassenger` - مسافر جدید با شناسه {0} افزوده شد
- `ActivityLog.EditPassenger` - مسافر با شناسه {0} ویرایش شد
- `ActivityLog.DeletePassenger` - مسافر با شناسه {0} حذف شد

## نکات مهم

1. اسکریپت به صورت Idempotent است (می‌توانید چندین بار اجرا کنید بدون خطا)
2. اگر رشته از قبل وجود داشته باشد، درج نمی‌شود
3. تمام رشته‌ها با استفاده از `N''` برای پشتیبانی از کاراکترهای Unicode فارسی ذخیره می‌شوند
4. پس از اجرای اسکریپت، باید Cache را پاک کنید تا تغییرات اعمال شوند

## پاک کردن Cache

پس از اجرای اسکریپت، Cache را پاک کنید:
- از طریق Admin Panel: Configuration > Maintenance > Clear Cache
- یا از طریق دیتابیس: حذف فایل‌های Cache در پوشه App_Data

