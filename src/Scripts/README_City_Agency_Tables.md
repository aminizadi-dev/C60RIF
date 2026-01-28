# اسکریپت‌های ایجاد و به‌روزرسانی جداول City، Agency و Passenger

این فایل شامل اسکریپت‌های SQL برای ایجاد جداول City و Agency و اضافه کردن ستون AgencyId به جدول Passenger است.

## فایل‌های موجود

### 1. `CreateCityTable.sql`
اسکریپت برای ایجاد جدول City

### 2. `CreateAgencyTable.sql`
اسکریپت برای ایجاد جدول Agency (نیاز به City دارد)

### 3. `AddAgencyIdToPassengerTable.sql`
اسکریپت برای اضافه کردن ستون AgencyId به جدول Passenger (نیاز به City و Agency دارد)

### 4. `UpdateAllTables_City_Agency_Passenger.sql` (توصیه می‌شود)
اسکریپت کامل که تمام جداول را به ترتیب صحیح ایجاد/به‌روزرسانی می‌کند

## روش استفاده

### روش 1: استفاده از اسکریپت کامل (توصیه می‌شود)

1. SQL Server Management Studio (SSMS) را باز کنید
2. به دیتابیس NopCommerce متصل شوید
3. فایل `UpdateAllTables_City_Agency_Passenger.sql` را باز کنید
4. اسکریپت را اجرا کنید (F5 یا Execute)

این اسکریپت به صورت خودکار:
- جدول City را ایجاد می‌کند
- جدول Agency را ایجاد می‌کند
- یک City و Agency پیش‌فرض ایجاد می‌کند
- ستون AgencyId را به Passenger اضافه می‌کند
- تمام Passenger های موجود را به Agency پیش‌فرض اختصاص می‌دهد
- Foreign Key و Index ها را ایجاد می‌کند

### روش 2: اجرای جداگانه اسکریپت‌ها

اگر می‌خواهید اسکریپت‌ها را جداگانه اجرا کنید، باید به ترتیب زیر عمل کنید:

1. ابتدا `CreateCityTable.sql` را اجرا کنید
2. سپس `CreateAgencyTable.sql` را اجرا کنید
3. در نهایت `AddAgencyIdToPassengerTable.sql` را اجرا کنید

## ساختار جداول

### جدول City
- **Id**: شناسه یکتا (Primary Key, Identity)
- **Name**: نام شهر (nvarchar(500), NOT NULL)
- **Published**: وضعیت انتشار (bit, NOT NULL)
- **DisplayOrder**: ترتیب نمایش (int, NOT NULL)

### جدول Agency
- **Id**: شناسه یکتا (Primary Key, Identity)
- **CityId**: شناسه شهر (Foreign Key به City, NOT NULL)
- **Name**: نام نمایندگی (nvarchar(500), NOT NULL)
- **Published**: وضعیت انتشار (bit, NOT NULL)
- **DisplayOrder**: ترتیب نمایش (int, NOT NULL)

### تغییرات جدول Passenger
- **AgencyId**: شناسه نمایندگی (Foreign Key به Agency, NOT NULL) - **جدید**

## روابط (Relationships)

```
City (1) ──< (Many) Agency (1) ──< (Many) Passenger
```

- هر شهر می‌تواند چندین نمایندگی داشته باشد
- هر نمایندگی می‌تواند چندین Passenger داشته باشد
- Foreign Keys با Cascade Delete تنظیم شده‌اند

## Indexes

برای بهبود عملکرد، Index های زیر ایجاد شده‌اند:

### City
- `IX_City_Name`: برای جستجو بر اساس نام
- `IX_City_DisplayOrder`: برای مرتب‌سازی

### Agency
- `IX_Agency_CityId`: برای جستجو بر اساس شهر
- `IX_Agency_Name`: برای جستجو بر اساس نام
- `IX_Agency_DisplayOrder`: برای مرتب‌سازی

### Passenger
- `IX_Passenger_AgencyId`: برای جستجو بر اساس نمایندگی

## نکات مهم

1. **داده‌های موجود**: اگر جدول Passenger از قبل داده دارد، تمام Passenger های موجود به نمایندگی پیش‌فرض اختصاص داده می‌شوند.

2. **City و Agency پیش‌فرض**: اگر هیچ City یا Agency وجود نداشته باشد، یک City و Agency پیش‌فرض با نام‌های "شهر پیش‌فرض" و "نمایندگی پیش‌فرض" ایجاد می‌شود.

3. **Foreign Key Constraints**: Foreign Keys با Cascade Delete تنظیم شده‌اند، یعنی:
   - اگر یک City حذف شود، تمام Agency های مربوطه حذف می‌شوند
   - اگر یک Agency حذف شود، تمام Passenger های مربوطه حذف می‌شوند

4. **AgencyId Required**: ستون AgencyId در Passenger به صورت NOT NULL است و نمی‌تواند خالی باشد.

## عیب‌یابی

### خطای "Invalid column name 'AgencyId'"
اگر این خطا را دریافت می‌کنید، به این معنی است که ستون AgencyId هنوز به جدول Passenger اضافه نشده است. اسکریپت `AddAgencyIdToPassengerTable.sql` یا `UpdateAllTables_City_Agency_Passenger.sql` را اجرا کنید.

### خطای Foreign Key
اگر خطای Foreign Key دریافت می‌کنید، مطمئن شوید که:
1. جدول City قبل از Agency ایجاد شده باشد
2. جدول Agency قبل از اضافه کردن AgencyId به Passenger ایجاد شده باشد

## بعد از اجرای اسکریپت

بعد از اجرای موفقیت‌آمیز اسکریپت‌ها:
1. پروژه را Rebuild کنید
2. برنامه را Restart کنید
3. Migration ها به صورت خودکار اجرا می‌شوند (اما چون جداول از قبل وجود دارند، خطایی رخ نمی‌دهد)

