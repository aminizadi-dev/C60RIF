# اسکریپت ایجاد جدول Passenger

این فایل شامل اسکریپت SQL برای ایجاد جدول Passenger در SQL Server است.

## روش استفاده

### روش 1: استفاده از Migration (توصیه می‌شود)
اگر از NopCommerce استفاده می‌کنید، Migration به صورت خودکار اجرا می‌شود:
- برای نصب اولیه: جدول Passenger در SchemaMigration اضافه شده است
- برای به‌روزرسانی: Migration `AddPassengerTableMigration` به صورت خودکار اجرا می‌شود

### روش 2: اجرای دستی اسکریپت SQL
اگر می‌خواهید به صورت دستی جدول را ایجاد کنید:

1. SQL Server Management Studio (SSMS) را باز کنید
2. به دیتابیس NopCommerce متصل شوید
3. فایل `CreatePassengerTable.sql` را باز کنید
4. اسکریپت را اجرا کنید (F5 یا Execute)

## ساختار جدول

جدول Passenger شامل فیلدهای زیر است:

- **Id**: شناسه یکتا (Primary Key, Identity)
- **RecoveryNo**: شماره بازیابی
- **PersonName**: نام شخص
- **GuideNameAndLegionNo**: نام راهنما و شماره لژیون
- **ClinicId**: شناسه کلینیک (Foreign Key, الزامی)
- **BirthDateUtc**: تاریخ تولد
- **Education**: سطح تحصیلات (enum)
- **MaritalStatus**: وضعیت تاهل (enum)
- **EmploymentStatus**: وضعیت اشتغال (enum)
- **CardNo**: شماره کارت
- **AntiX1**: شناسه AntiX اول (عدد)
- **AntiX2**: شناسه AntiX دوم (عدد، اختیاری)
- **TravelStartDateUtc**: تاریخ شروع سفر
- **TravelEndDateUtc**: تاریخ پایان سفر
- **PictureId**: شناسه تصویر
- **AgencyId**: شناسه نمایندگی
- **CreatedOnUtc**: تاریخ ایجاد

## Indexes

برای بهبود عملکرد، Index های زیر ایجاد شده‌اند:

1. **IX_Passenger_RecoveryNo**: برای جستجو بر اساس شماره بازیابی
2. **IX_Passenger_PersonName**: برای جستجو بر اساس نام شخص
3. **IX_Passenger_AgencyId**: برای جستجو بر اساس نمایندگی
4. **IX_Passenger_ClinicId**: برای جستجو بر اساس کلینیک
5. **IX_Passenger_AntiX1**: برای جستجو بر اساس AntiX1
6. **IX_Passenger_AntiX2**: برای جستجو بر اساس AntiX2
7. **IX_Passenger_CreatedOnUtc**: برای مرتب‌سازی بر اساس تاریخ ایجاد

## نکات مهم

- اسکریپت به صورت Idempotent است (می‌توانید چندین بار اجرا کنید بدون خطا)
- اگر جدول از قبل وجود داشته باشد، پیام "Passenger table already exists" نمایش داده می‌شود
- تمام فیلدهای DateTime به صورت UTC ذخیره می‌شوند

