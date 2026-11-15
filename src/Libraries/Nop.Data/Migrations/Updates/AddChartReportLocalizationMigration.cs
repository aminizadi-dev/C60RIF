using FluentMigrator;
using Nop.Core.Domain.Localization;
using Nop.Data.Mapping;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

/// <summary>
/// Persian and English labels for the reports menu and printable charts
/// </summary>
[NopUpdateMigration("2026/11/07 12:00:00", "4.90", UpdateMigrationType.Localization)]
public class AddChartReportLocalizationMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        var resourceTable = NameCompatibilityManager.GetTableName(typeof(LocaleStringResource));
        var languageTable = NameCompatibilityManager.GetTableName(typeof(Language));

        IfDatabase("SqlServer").Execute.Sql($@"
DECLARE @resources TABLE (
    ResourceName nvarchar(200) NOT NULL,
    FaValue nvarchar(max) NOT NULL,
    EnValue nvarchar(max) NOT NULL
);

INSERT INTO @resources (ResourceName, FaValue, EnValue) VALUES
(N'Admin.Reports', N'گزارش‌ها', N'Reports'),
(N'Admin.Reports.Charts.Age', N'نمودار میانگین سنی مراجعان به کنگره 60', N'Average age of Congress 60 visitors'),
(N'Admin.Reports.Charts.Education', N'نمودار میزان تحصیلات مراجعان به کنگره 60', N'Education level of Congress 60 visitors'),
(N'Admin.Reports.Charts.LiberationByYear', N'نمودار آمار رهایی ثبت شده در طول سال', N'Registered liberations during the year'),
(N'Admin.Reports.Charts.LiberationByCity', N'آمار رهایی ثبت شده به تفکیک استان/شهر', N'Registered liberations by province/city'),
(N'Admin.Reports.Charts.AntiX', N'توزیع مصرف آنتی ایکس', N'AntiX consumption distribution'),
(N'Admin.Reports.Charts.MaritalStatus', N'وضعیت تاهل', N'Marital status'),
(N'Admin.Reports.Charts.Employment', N'وضعیت اشتغال', N'Employment status'),
(N'Admin.Reports.Charts.Print', N'چاپ', N'Print'),
(N'Admin.Reports.Charts.Apply', N'نمایش', N'Show'),
(N'Admin.Reports.Charts.DateFrom', N'از تاریخ', N'From date'),
(N'Admin.Reports.Charts.DateTo', N'تا تاریخ', N'To date'),
(N'Admin.Reports.Charts.City', N'شهر', N'City'),
(N'Admin.Reports.Charts.Agency', N'منطقه (نمایندگی)', N'Region (agency)'),
(N'Admin.Reports.Charts.Year', N'سال رهایی', N'Liberation year'),
(N'Admin.Reports.Charts.All', N'همه', N'All'),
(N'Admin.Reports.Charts.Total', N'تعداد کل', N'Total'),
(N'Admin.Reports.Charts.NoData', N'موردی با این فیلتر یافت نشد', N'No records match this filter'),
(N'Admin.Reports.Charts.Person', N'نفر', N'people'),
(N'Admin.Reports.Charts.Unknown', N'نامشخص', N'Unknown'),
(N'Admin.Reports.Charts.TravelStartHint', N'فیلتر تاریخ بر اساس تاریخ شروع سفر است.', N'The date range filters by travel start date.'),
(N'Admin.Reports.Charts.LiberationDateHint', N'فیلتر تاریخ بر اساس تاریخ رهایی است.', N'The date range filters by liberation date.'),
(N'Security.Permission.Reports.ReportsView', N'گزارش‌ها. مشاهده', N'Admin area. Reports. View');

INSERT INTO [{resourceTable}] ([LanguageId], [ResourceName], [ResourceValue])
SELECT l.[Id], r.ResourceName,
       CASE WHEN l.[LanguageCulture] LIKE N'fa%' OR l.[UniqueSeoCode] = N'fa' THEN r.FaValue ELSE r.EnValue END
FROM [{languageTable}] l
CROSS JOIN @resources r
WHERE NOT EXISTS (
    SELECT 1 FROM [{resourceTable}] existing
    WHERE existing.[LanguageId] = l.[Id] AND existing.[ResourceName] = r.ResourceName
);

UPDATE existing
SET existing.[ResourceValue] = r.FaValue
FROM [{resourceTable}] existing
INNER JOIN [{languageTable}] l ON l.[Id] = existing.[LanguageId]
INNER JOIN @resources r ON r.ResourceName = existing.[ResourceName]
WHERE l.[LanguageCulture] LIKE N'fa%' OR l.[UniqueSeoCode] = N'fa';
");
    }
}
