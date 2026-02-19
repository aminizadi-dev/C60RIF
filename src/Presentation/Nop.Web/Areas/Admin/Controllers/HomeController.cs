using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Common;
using Nop.Web.Areas.Admin.Models.Home;
using Nop.Web.Framework.Models.DataTables;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class HomeController : BaseAdminController
{
    #region Fields

    protected readonly AdminAreaSettings _adminAreaSettings;
    protected readonly ICommonModelFactory _commonModelFactory;
    protected readonly IHomeModelFactory _homeModelFactory;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly ISettingService _settingService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IWorkContext _workContext;
    protected readonly NopHttpClient _nopHttpClient;
    protected readonly IPassengerService _passengerService;
    protected readonly ICityService _cityService;
    protected readonly IAgencyService _agencyService;
    protected readonly IAntiXService _antiXService;

    #endregion

    #region Ctor

    public HomeController(AdminAreaSettings adminAreaSettings,
        ICommonModelFactory commonModelFactory,
        IHomeModelFactory homeModelFactory,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IPermissionService permissionService,
        ISettingService settingService,
        IGenericAttributeService genericAttributeService,
        IWorkContext workContext,
        NopHttpClient nopHttpClient,
        IPassengerService passengerService,
        ICityService cityService,
        IAgencyService agencyService,
        IAntiXService antiXService)
    {
        _adminAreaSettings = adminAreaSettings;
        _commonModelFactory = commonModelFactory;
        _homeModelFactory = homeModelFactory;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _settingService = settingService;
        _workContext = workContext;
        _genericAttributeService = genericAttributeService;
        _nopHttpClient = nopHttpClient;
        _passengerService = passengerService;
        _cityService = cityService;
        _agencyService = agencyService;
        _antiXService = antiXService;
    }

    #endregion

    #region Methods

    public virtual async Task<IActionResult> Index()
    {
        //display a warning to a store owner if there are some error
        var customer = await _workContext.GetCurrentCustomerAsync();
        var hideCard = await _genericAttributeService.GetAttributeAsync<bool>(customer, NopCustomerDefaults.HideConfigurationStepsAttribute);
        var closeCard = await _genericAttributeService.GetAttributeAsync<bool>(customer, NopCustomerDefaults.CloseConfigurationStepsAttribute);

        if ((hideCard || closeCard) && await _permissionService.AuthorizeAsync(StandardPermission.System.MANAGE_MAINTENANCE))
        {
            var warnings = await _commonModelFactory.PrepareSystemWarningModelsAsync();
            if (warnings.Any(warning => warning.Level == SystemWarningLevel.Fail || warning.Level == SystemWarningLevel.Warning))
            {
                var locale = await _localizationService.GetResourceAsync("Admin.System.Warnings.Errors");
                _notificationService.WarningNotification(string.Format(locale, Url.Action("Warnings", "Common")), false); //do not encode URLs
            }
        }

        //progress of localization 
        var currentLanguage = await _workContext.GetWorkingLanguageAsync();
        var progress = await _genericAttributeService.GetAttributeAsync<string>(currentLanguage, NopCommonDefaults.LanguagePackProgressAttribute);
        if (!string.IsNullOrEmpty(progress))
        {
            var locale = await _localizationService.GetResourceAsync("Admin.Configuration.LanguagePackProgressMessage");
            _notificationService.SuccessNotification(string.Format(locale, progress, NopLinksDefaults.OfficialSite.Translations), false);
            await _genericAttributeService.SaveAttributeAsync(currentLanguage, NopCommonDefaults.LanguagePackProgressAttribute, string.Empty);
        }

        //prepare model
        var model = await _homeModelFactory.PrepareDashboardModelAsync(new DashboardModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> NopCommerceNewsHideAdv()
    {
        _adminAreaSettings.HideAdvertisementsOnAdminArea = !_adminAreaSettings.HideAdvertisementsOnAdminArea;
        await _settingService.SaveSettingAsync(_adminAreaSettings);

        return Content("Setting changed");
    }

    public virtual async Task<IActionResult> GetPopularSearchTerm()
    {
        var model = new DataTablesModel();
        model = await _homeModelFactory.PreparePopularSearchTermReportModelAsync(model);
        return PartialView("Table", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> AcceptLicenseTerms()
    {
        if (!(await _settingService.TryGetLicenseAsync(true)).Exists)
            return Json(new { Result = false });

        try
        {
            await _nopHttpClient.CheckLicenseTermsAsync(true);
        }
        catch { }

        return Json(new { Result = true });
    }

    /// <summary>
    /// Load passenger treatment statistics by TravelEndDateUtc
    /// </summary>
    /// <param name="period">Period: "year" for yearly, "month" for monthly</param>
    /// <param name="year">Persian year for monthly statistics</param>
    /// <returns>JSON result with date labels and counts</returns>
    [CheckPermission(StandardPermission.Customers.CUSTOMERS_VIEW)]
    public virtual async Task<IActionResult> LoadPassengerTreatmentStatistics(string period, int? year = null)
    {
        var result = new List<object>();

        // Get all passengers with TravelEndDateUtc
        var allPassengers = await _passengerService.GetAllPassengersAsync(
            recoveryNo: 0,
            personName: null,
            cityId: 0,
            agencyId: 0,
            clinicId: 0,
            antiXId: 0,
            guideNameAndLegionNo: null,
            cardNo: null,
            travelStartDateUtc: null,
            travelEndDateUtc: null,
            pageIndex: 0,
            pageSize: int.MaxValue,
            getOnlyTotalCount: false);

        // Filter passengers with valid TravelEndDateUtc and convert to Persian dates
        var passengersWithDate = allPassengers
            .Where(p => p.TravelEndDateUtc.HasValue && p.TravelEndDateUtc.Value > DateTime.MinValue)
            .Select(p => new
            {
                Passenger = p,
                PersianDate = new PersianDateTime(p.TravelEndDateUtc.Value)
            })
            .ToList();

        switch (period)
        {
            case "year":
                // Yearly statistics - group by Persian year
                var yearGroups = passengersWithDate
                    .GroupBy(p => p.PersianDate.Year)
                    .OrderBy(g => g.Key)
                    .ToList();

                foreach (var yearGroup in yearGroups)
                {
                    result.Add(new
                    {
                        date = yearGroup.Key.ToString(),
                        value = yearGroup.Count()
                    });
                }
                break;

            case "month":
            default:
                // Monthly statistics - group by Persian year and month
                // Get selected Persian year or use the most recent year in data
                var currentPersianYear = new PersianDateTime(DateTime.Now).Year;
                var targetYear = year.HasValue && year.Value > 0
                    ? year.Value
                    : (passengersWithDate.Any()
                        ? passengersWithDate.Max(p => p.PersianDate.Year)
                        : currentPersianYear);

                // Initialize all 12 months with 0
                var monthData = new Dictionary<int, int>();
                for (int month = 1; month <= 12; month++)
                {
                    monthData[month] = 0;
                }

                // Count passengers for each month in the target year
                var monthGroups = passengersWithDate
                    .Where(p => p.PersianDate.Year == targetYear)
                    .GroupBy(p => p.PersianDate.Month)
                    .ToList();

                foreach (var monthGroup in monthGroups)
                {
                    monthData[monthGroup.Key] = monthGroup.Count();
                }

                // Persian month names
                var monthNames = new[]
                {
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
                    "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
                };

                // Add results for all 12 months
                for (int month = 1; month <= 12; month++)
                {
                    result.Add(new
                    {
                        date = monthNames[month - 1],
                        value = monthData[month]
                    });
                }
                break;
        }

        return Json(result);
    }

    /// <summary>
    /// Load available Persian years for passenger treatment statistics
    /// </summary>
    /// <returns>JSON result with available years</returns>
    [CheckPermission(StandardPermission.Customers.CUSTOMERS_VIEW)]
    public virtual async Task<IActionResult> LoadPassengerTreatmentYears()
    {
        // Get all passengers with TravelEndDateUtc
        var allPassengers = await _passengerService.GetAllPassengersAsync(
            recoveryNo: 0,
            personName: null,
            cityId: 0,
            agencyId: 0,
            clinicId: 0,
            antiXId: 0,
            guideNameAndLegionNo: null,
            cardNo: null,
            travelStartDateUtc: null,
            travelEndDateUtc: null,
            pageIndex: 0,
            pageSize: int.MaxValue,
            getOnlyTotalCount: false);

        var years = allPassengers
            .Where(p => p.TravelEndDateUtc.HasValue && p.TravelEndDateUtc.Value > DateTime.MinValue)
            .Select(p => new PersianDateTime(p.TravelEndDateUtc.Value).Year)
            .Distinct()
            .OrderByDescending(y => y)
            .ToList();

        var currentPersianYear = new PersianDateTime(DateTime.Now).Year;
        var selectedYear = years.FirstOrDefault();
        if (selectedYear == 0)
            selectedYear = currentPersianYear;

        var result = years.Any()
            ? years.Select(y => new { year = y, selected = y == selectedYear })
            : new[] { new { year = currentPersianYear, selected = true } };

        return Json(result);
    }

    /// <summary>
    /// Load cities for passenger distribution by agency chart
    /// </summary>
    /// <returns>JSON result with city list</returns>
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> LoadPassengerDistributionCities()
    {
        var cities = await _cityService.GetAllCitiesAsync(pageIndex: 0, pageSize: int.MaxValue);
        var result = cities.Select(city => new { id = city.Id, name = city.Name }).ToList();

        return Json(result);
    }

    /// <summary>
    /// Load passenger distribution by agency for a city
    /// </summary>
    /// <param name="cityId">City identifier</param>
    /// <returns>JSON result with agency labels and counts</returns>
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> LoadPassengerDistributionByAgency(int cityId)
    {
        if (cityId <= 0)
            return Json(Array.Empty<object>());

        var agencies = await _agencyService.GetAgenciesByCityIdAsync(cityId, showHidden: true);

        var passengers = await _passengerService.GetAllPassengersAsync(
            recoveryNo: 0,
            personName: null,
            cityId: cityId,
            agencyId: 0,
            clinicId: 0,
            antiXId: 0,
            guideNameAndLegionNo: null,
            cardNo: null,
            travelStartDateUtc: null,
            travelEndDateUtc: null,
            pageIndex: 0,
            pageSize: int.MaxValue,
            getOnlyTotalCount: false);

        var counts = passengers
            .Where(p => p.AgencyId > 0)
            .GroupBy(p => p.AgencyId)
            .ToDictionary(g => g.Key, g => g.Count());

        var result = agencies
            .OrderBy(a => a.Name)
            .Select(a => new
            {
                agencyName = a.Name,
                count = counts.TryGetValue(a.Id, out var count) ? count : 0
            })
            .ToList();

        return Json(result);
    }

    /// <summary>
    /// Load AntiX consumption distribution for dashboard bar chart.
    /// Counts both AntiX1 and AntiX2 (when AntiX2 has a value).
    /// Categories are matched by AntiX name from the database.
    /// </summary>
    /// <returns>JSON result with labels and counts</returns>
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> LoadAntiXConsumptionStatistics()
    {
        var passengers = await _passengerService.GetAllPassengersAsync(
            recoveryNo: 0,
            personName: null,
            cityId: 0,
            agencyId: 0,
            clinicId: 0,
            antiXId: 0,
            guideNameAndLegionNo: null,
            cardNo: null,
            travelStartDateUtc: null,
            travelEndDateUtc: null,
            pageIndex: 0,
            pageSize: int.MaxValue,
            getOnlyTotalCount: false);

        // Collect all AntiX values (AntiX1 + AntiX2 when present)
        var allAntiXValues = passengers
            .Where(p => p.AntiX1 > 0)
            .Select(p => p.AntiX1)
            .Concat(passengers
                .Where(p => p.AntiX2.HasValue && p.AntiX2.Value > 0)
                .Select(p => p.AntiX2.Value))
            .ToList();

        // Categories mapped by AntiX IDs from database:
        // 2=تریاک, 3=تریاک خوراکی, 4=تریاک کشیدنی, 5=شربت تریاک / OT
        // 6=شیره, 7=شیره خوراکی, 8=شیره کشیدنی
        // 11=شیشه, 12=هروئین, 16=متادون (قرص / شربت), 20=قرص B2
        var categories = new (string label, HashSet<int> ids)[]
        {
            ("تریاک کشیدنی", new HashSet<int> { 2, 4 }),
            ("شیره خوراکی", new HashSet<int> { 7 }),
            ("شیره کشیدنی", new HashSet<int> { 6, 8 }),
            ("تریاک خوراکی", new HashSet<int> { 3 }),
            ("شربت تریاک / OT", new HashSet<int> { 5 }),
            ("متادون", new HashSet<int> { 16 }),
            ("هروئین", new HashSet<int> { 12 }),
            ("شیشه", new HashSet<int> { 11 }),
            ("قرص B2", new HashSet<int> { 20 })
        };

        var knownIds = new HashSet<int>(categories.SelectMany(c => c.ids));

        // Count per category
        var result = new List<object>();
        foreach (var (label, ids) in categories)
        {
            result.Add(new { label, count = allAntiXValues.Count(v => ids.Contains(v)) });
        }
        result.Add(new { label = "سایر", count = allAntiXValues.Count(v => !knownIds.Contains(v)) });

        return Json(result);
    }

    /// <summary>
    /// Load marital status distribution for dashboard bar chart
    /// </summary>
    /// <returns>JSON result with labels and counts</returns>
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> LoadPassengerMaritalStatusStatistics()
    {
        var passengers = await _passengerService.GetAllPassengersAsync(
            recoveryNo: 0,
            personName: null,
            cityId: 0,
            agencyId: 0,
            clinicId: 0,
            antiXId: 0,
            guideNameAndLegionNo: null,
            cardNo: null,
            travelStartDateUtc: null,
            travelEndDateUtc: null,
            pageIndex: 0,
            pageSize: int.MaxValue,
            getOnlyTotalCount: false);

        var marriedCount = passengers.Count(p => p.IsMarried);
        var singleCount = passengers.Count(p => !p.IsMarried);

        var result = new[]
        {
            new { label = await _localizationService.GetResourceAsync("Admin.Reports.Passengers.MaritalStatusStatistics.Married"), count = marriedCount },
            new { label = await _localizationService.GetResourceAsync("Admin.Reports.Passengers.MaritalStatusStatistics.Single"), count = singleCount }
        };

        return Json(result);
    }

    /// <summary>
    /// Load employment status distribution for dashboard bar chart
    /// </summary>
    /// <returns>JSON result with labels and counts</returns>
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> LoadPassengerEmploymentStatusStatistics()
    {
        var passengers = await _passengerService.GetAllPassengersAsync(
            recoveryNo: 0,
            personName: null,
            cityId: 0,
            agencyId: 0,
            clinicId: 0,
            antiXId: 0,
            guideNameAndLegionNo: null,
            cardNo: null,
            travelStartDateUtc: null,
            travelEndDateUtc: null,
            pageIndex: 0,
            pageSize: int.MaxValue,
            getOnlyTotalCount: false);

        var employedCount = passengers.Count(p => p.IsEmployed);
        var unemployedCount = passengers.Count(p => !p.IsEmployed);

        var result = new[]
        {
            new { label = await _localizationService.GetResourceAsync("Admin.Reports.Passengers.EmploymentStatusStatistics.Employed"), count = employedCount },
            new { label = await _localizationService.GetResourceAsync("Admin.Reports.Passengers.EmploymentStatusStatistics.Unemployed"), count = unemployedCount }
        };

        return Json(result);
    }

    /// <summary>
    /// Load average travel length by agency for a city
    /// </summary>
    /// <param name="cityId">City identifier</param>
    /// <returns>JSON result with agency labels and averages</returns>
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> LoadPassengerAverageTravelLengthByAgency(int cityId)
    {
        if (cityId <= 0)
            return Json(Array.Empty<object>());

        var agencies = await _agencyService.GetAgenciesByCityIdAsync(cityId, showHidden: true);

        var passengers = await _passengerService.GetAllPassengersAsync(
            recoveryNo: 0,
            personName: null,
            cityId: cityId,
            agencyId: 0,
            clinicId: 0,
            antiXId: 0,
            guideNameAndLegionNo: null,
            cardNo: null,
            travelStartDateUtc: null,
            travelEndDateUtc: null,
            pageIndex: 0,
            pageSize: int.MaxValue,
            getOnlyTotalCount: false);

        var travelGroups = passengers
            .Where(p => p.AgencyId > 0 && p.TravelStartDateUtc.HasValue && p.TravelEndDateUtc.HasValue)
            .Select(p => new
            {
                p.AgencyId,
                Days = (p.TravelEndDateUtc.Value - p.TravelStartDateUtc.Value).TotalDays
            })
            .Where(p => p.Days >= 0)
            .GroupBy(p => p.AgencyId)
            .ToDictionary(
                g => g.Key,
                g => new
                {
                    Count = g.Count(),
                    TotalDays = g.Sum(x => x.Days)
                });

        var result = agencies
            .OrderBy(a => a.Name)
            .Select(a =>
            {
                if (!travelGroups.TryGetValue(a.Id, out var stats) || stats.Count == 0)
                {
                    return new { agencyName = a.Name, averageDays = 0d, averageMonths = 0d, averageLabel = "0 ماه 0 روز" };
                }

                var averageDays = stats.TotalDays / stats.Count;
                var totalRoundedDays = (int)Math.Round(averageDays, MidpointRounding.AwayFromZero);
                var months = totalRoundedDays / 30;
                var days = totalRoundedDays % 30;
                var averageMonths = Math.Round(averageDays / 30.0, 1);
                var averageLabel = $"{months} ماه {days} روز";

                return new { agencyName = a.Name, averageDays = Math.Round(averageDays, 1), averageMonths, averageLabel };
            })
            .ToList();

        return Json(result);
    }

    /// <summary>
    /// Load age distribution statistics for dashboard bar chart
    /// Age is calculated from BirthYear (Persian year) relative to current Persian year
    /// </summary>
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> LoadPassengerAgeDistribution()
    {
        var passengers = await _passengerService.GetAllPassengersAsync(
            recoveryNo: 0,
            personName: null,
            cityId: 0,
            agencyId: 0,
            clinicId: 0,
            antiXId: 0,
            guideNameAndLegionNo: null,
            cardNo: null,
            travelStartDateUtc: null,
            travelEndDateUtc: null,
            pageIndex: 0,
            pageSize: int.MaxValue,
            getOnlyTotalCount: false);

        var pc = new System.Globalization.PersianCalendar();
        var currentPersianYear = pc.GetYear(DateTime.Now);

        var withAge = passengers
            .Where(p => p.BirthYear.HasValue && p.BirthYear.Value >= 1300 && p.BirthYear.Value <= currentPersianYear)
            .Select(p => currentPersianYear - p.BirthYear.Value)
            .ToList();

        var result = new[]
        {
            new { label = "18 تا 20 سال", count = withAge.Count(a => a >= 18 && a < 20) },
            new { label = "20 تا 30 سال", count = withAge.Count(a => a >= 20 && a < 30) },
            new { label = "30 تا 40 سال", count = withAge.Count(a => a >= 30 && a < 40) },
            new { label = "40 تا 50 سال", count = withAge.Count(a => a >= 40 && a < 50) },
            new { label = "50 تا 60 سال", count = withAge.Count(a => a >= 50 && a < 60) },
            new { label = "بالاتر از 60 سال", count = withAge.Count(a => a >= 60) }
        };

        return Json(result);
    }

    /// <summary>
    /// Load education level distribution statistics for dashboard bar chart
    /// </summary>
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> LoadPassengerEducationDistribution()
    {
        var passengers = await _passengerService.GetAllPassengersAsync(
            recoveryNo: 0,
            personName: null,
            cityId: 0,
            agencyId: 0,
            clinicId: 0,
            antiXId: 0,
            guideNameAndLegionNo: null,
            cardNo: null,
            travelStartDateUtc: null,
            travelEndDateUtc: null,
            pageIndex: 0,
            pageSize: int.MaxValue,
            getOnlyTotalCount: false);

        var result = new List<object>();
        foreach (EducationLevel level in Enum.GetValues(typeof(EducationLevel)))
        {
            var count = passengers.Count(p => p.Education == level);
            var localizedName = await _localizationService.GetLocalizedEnumAsync(level);
            result.Add(new { label = localizedName, count });
        }

        return Json(result);
    }

    #endregion
}