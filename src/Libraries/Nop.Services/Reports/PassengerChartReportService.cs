using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Services.Localization;

namespace Nop.Services.Reports;

/// <summary>
/// Builds the passenger bar charts used by the reports section
/// </summary>
public partial class PassengerChartReportService : IPassengerChartReportService
{
    #region Fields

    protected readonly IAgencyService _agencyService;
    protected readonly ICityService _cityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPersonService _personService;
    protected readonly IRecoveryFormService _recoveryFormService;

    /// <summary>
    /// AntiX categories, matched by the same identifiers as the dashboard chart
    /// </summary>
    protected static readonly (string Label, HashSet<int> Ids)[] AntiXCategories =
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

    protected static readonly string[] PersianMonthNames =
    {
        "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
        "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
    };

    #endregion

    #region Ctor

    public PassengerChartReportService(
        IAgencyService agencyService,
        ICityService cityService,
        ILocalizationService localizationService,
        IPersonService personService,
        IRecoveryFormService recoveryFormService)
    {
        _agencyService = agencyService;
        _cityService = cityService;
        _localizationService = localizationService;
        _personService = personService;
        _recoveryFormService = recoveryFormService;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public virtual async Task<IList<ChartReportPoint>> GetAgeDistributionAsync(int cityId, int agencyId, DateTime? dateFrom, DateTime? dateTo)
    {
        NormalizeRange(ref dateFrom, ref dateTo);
        var forms = await LoadFormsAsync(cityId, agencyId);
        forms = forms.Where(form => IsOnOrAfter(form.TravelStartDateUtc, dateFrom) && IsOnOrBefore(form.TravelStartDateUtc, dateTo)).ToList();

        var currentPersianYear = new PersianDateTime(DateTime.Now).Year;
        var personIds = forms.Select(form => form.PersonId).Where(id => id > 0).Distinct().ToArray();
        var persons = await _personService.GetPersonsByIdsAsync(personIds);
        var ages = persons
            .Where(person => person.BirthYear.HasValue && person.BirthYear.Value >= 1300 && person.BirthYear.Value <= currentPersianYear)
            .Select(person => currentPersianYear - person.BirthYear.Value)
            .ToList();

        return new List<ChartReportPoint>
        {
            Point("18 تا 20 سال", ages.Count(age => age >= 18 && age < 20)),
            Point("20 تا 30 سال", ages.Count(age => age >= 20 && age < 30)),
            Point("30 تا 40 سال", ages.Count(age => age >= 30 && age < 40)),
            Point("40 تا 50 سال", ages.Count(age => age >= 40 && age < 50)),
            Point("50 تا 60 سال", ages.Count(age => age >= 50 && age < 60)),
            Point("بالاتر از 60 سال", ages.Count(age => age >= 60))
        };
    }

    /// <inheritdoc />
    public virtual async Task<IList<ChartReportPoint>> GetEducationDistributionAsync(int cityId, int agencyId, DateTime? dateFrom, DateTime? dateTo)
    {
        NormalizeRange(ref dateFrom, ref dateTo);
        var forms = await LoadFormsAsync(cityId, agencyId);
        forms = forms.Where(form => IsOnOrAfter(form.TravelStartDateUtc, dateFrom) && IsOnOrBefore(form.TravelStartDateUtc, dateTo)).ToList();

        var result = new List<ChartReportPoint>();
        foreach (EducationLevel level in Enum.GetValues(typeof(EducationLevel)))
        {
            var label = await _localizationService.GetLocalizedEnumAsync(level);
            result.Add(Point(label, forms.Count(form => form.Education == level)));
        }

        return result;
    }

    /// <inheritdoc />
    public virtual async Task<IList<ChartReportPoint>> GetLiberationByMonthAsync(int year, int cityId, int agencyId)
    {
        var liberations = await LoadLiberationsAsync(cityId, agencyId);
        var currentYear = new PersianDateTime(DateTime.Now).Year;
        var targetYear = year > 0
            ? year
            : (liberations.Any() ? liberations.Max(item => item.PersianDate.Year) : currentYear);

        var counts = new int[12];
        foreach (var item in liberations.Where(item => item.PersianDate.Year == targetYear))
        {
            var month = item.PersianDate.Month;
            if (month >= 1 && month <= 12)
                counts[month - 1]++;
        }

        return PersianMonthNames
            .Select((name, index) => Point(name, counts[index]))
            .ToList();
    }

    /// <inheritdoc />
    public virtual async Task<IList<int>> GetLiberationYearsAsync(int cityId, int agencyId)
    {
        var liberations = await LoadLiberationsAsync(cityId, agencyId);
        var years = liberations
            .Select(item => item.PersianDate.Year)
            .Distinct()
            .OrderByDescending(year => year)
            .ToList();

        var currentYear = new PersianDateTime(DateTime.Now).Year;
        if (!years.Contains(currentYear))
            years.Add(currentYear);

        years.Sort((left, right) => right.CompareTo(left));
        return years;
    }

    /// <inheritdoc />
    public virtual async Task<IList<ChartReportPoint>> GetLiberationByCityAsync(DateTime? dateFrom, DateTime? dateTo)
    {
        NormalizeRange(ref dateFrom, ref dateTo);
        var liberations = (await LoadLiberationsAsync())
            .Where(item => IsOnOrAfter(item.Form.TravelEndDateUtc, dateFrom) && IsOnOrBefore(item.Form.TravelEndDateUtc, dateTo))
            .ToList();

        var agencies = await _agencyService.GetAllAgenciesAsync(pageIndex: 0, pageSize: int.MaxValue);
        var agencyCity = agencies.ToDictionary(agency => agency.Id, agency => agency.CityId);
        var cities = await _cityService.GetAllCitiesAsync(pageIndex: 0, pageSize: int.MaxValue);

        var counts = new Dictionary<int, int>();
        var unknown = 0;
        foreach (var item in liberations)
        {
            if (item.Form.AgencyId > 0 && agencyCity.TryGetValue(item.Form.AgencyId, out var cityId) && cityId > 0)
                counts[cityId] = counts.TryGetValue(cityId, out var count) ? count + 1 : 1;
            else
                unknown++;
        }

        var result = cities
            .Select(city => Point(city.Name, counts.TryGetValue(city.Id, out var count) ? count : 0))
            .Where(point => point.Count > 0)
            .OrderBy(point => point.Label, StringComparer.CurrentCulture)
            .ToList();

        if (unknown > 0)
            result.Add(Point(await _localizationService.GetResourceAsync("Admin.Reports.Charts.Unknown"), unknown));

        return result;
    }

    /// <inheritdoc />
    public virtual async Task<IList<ChartReportPoint>> GetAntiXDistributionAsync()
    {
        var forms = await LoadFormsAsync();
        var values = forms
            .Where(form => form.AntiX1 > 0)
            .Select(form => form.AntiX1)
            .Concat(forms.Where(form => form.AntiX2.HasValue && form.AntiX2.Value > 0).Select(form => form.AntiX2.Value))
            .ToList();

        var knownIds = new HashSet<int>(AntiXCategories.SelectMany(category => category.Ids));
        var result = AntiXCategories
            .Select(category => Point(category.Label, values.Count(value => category.Ids.Contains(value))))
            .ToList();
        result.Add(Point("سایر", values.Count(value => !knownIds.Contains(value))));
        return result;
    }

    /// <inheritdoc />
    public virtual async Task<IList<ChartReportPoint>> GetMaritalStatusAsync()
    {
        var forms = await LoadFormsAsync();
        return new List<ChartReportPoint>
        {
            Point(await _localizationService.GetResourceAsync("Admin.Reports.Passengers.MaritalStatusStatistics.Married"), forms.Count(form => form.IsMarried == true)),
            Point(await _localizationService.GetResourceAsync("Admin.Reports.Passengers.MaritalStatusStatistics.Single"), forms.Count(form => form.IsMarried == false)),
            Point(await _localizationService.GetResourceAsync("Admin.Passengers.Fields.HasCompanion.Unknown"), forms.Count(form => form.IsMarried == null))
        };
    }

    /// <inheritdoc />
    public virtual async Task<IList<ChartReportPoint>> GetEmploymentStatusAsync()
    {
        var forms = await LoadFormsAsync();
        return new List<ChartReportPoint>
        {
            Point(await _localizationService.GetResourceAsync("Admin.Reports.Passengers.EmploymentStatusStatistics.Employed"), forms.Count(form => form.IsEmployed == true)),
            Point(await _localizationService.GetResourceAsync("Admin.Reports.Passengers.EmploymentStatusStatistics.Unemployed"), forms.Count(form => form.IsEmployed == false)),
            Point(await _localizationService.GetResourceAsync("Admin.Passengers.Fields.HasCompanion.Unknown"), forms.Count(form => form.IsEmployed == null))
        };
    }

    #endregion

    #region Utilities

    protected virtual async Task<IList<RecoveryForm>> LoadFormsAsync(int cityId = 0, int agencyId = 0)
    {
        var forms = await _recoveryFormService.GetAllRecoveryFormsAsync(
            cityId: cityId,
            agencyId: agencyId,
            pageIndex: 0,
            pageSize: int.MaxValue);

        return forms.ToList();
    }

    protected virtual async Task<IList<(RecoveryForm Form, PersianDateTime PersianDate)>> LoadLiberationsAsync(int cityId = 0, int agencyId = 0)
    {
        var forms = await LoadFormsAsync(cityId, agencyId);
        return forms
            .Where(form => form.TravelEndDateUtc.HasValue && form.TravelEndDateUtc.Value > DateTime.MinValue)
            .Select(form => (form, new PersianDateTime(form.TravelEndDateUtc.Value)))
            .ToList();
    }

    protected static ChartReportPoint Point(string label, int count)
    {
        return new ChartReportPoint
        {
            Label = label,
            Count = count
        };
    }

    /// <summary>
    /// When no bound is set, every record matches. A bound excludes records that have no date.
    /// </summary>
    protected static bool IsOnOrAfter(DateTime? value, DateTime? from)
    {
        if (!from.HasValue)
            return true;

        return value.HasValue && value.Value > DateTime.MinValue && value.Value.Date >= from.Value.Date;
    }

    protected static bool IsOnOrBefore(DateTime? value, DateTime? to)
    {
        if (!to.HasValue)
            return true;

        return value.HasValue && value.Value > DateTime.MinValue && value.Value.Date <= to.Value.Date;
    }

    protected static void NormalizeRange(ref DateTime? from, ref DateTime? to)
    {
        if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
            (from, to) = (to, from);
    }

    #endregion
}
