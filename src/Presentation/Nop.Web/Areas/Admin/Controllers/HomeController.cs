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
        IPassengerService passengerService)
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
    /// <returns>JSON result with date labels and counts</returns>
    [CheckPermission(StandardPermission.Customers.CUSTOMERS_VIEW)]
    public virtual async Task<IActionResult> LoadPassengerTreatmentStatistics(string period)
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
                // Get current Persian year or use the most recent year in data
                var currentPersianYear = new PersianDateTime(DateTime.Now).Year;
                var targetYear = passengersWithDate.Any() 
                    ? passengersWithDate.Max(p => p.PersianDate.Year) 
                    : currentPersianYear;

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

    #endregion
}