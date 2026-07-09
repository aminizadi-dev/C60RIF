using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the recovery form model factory implementation
/// </summary>
public partial class RecoveryFormModelFactory : IRecoveryFormModelFactory
{
    #region Fields

    protected readonly IAgencyService _agencyService;
    protected readonly IAntiXService _antiXService;
    protected readonly IClinicService _clinicService;
    protected readonly ICityService _cityService;
    protected readonly IDateTimeHelper _dateTimeHelper;
    protected readonly ILocalizationService _localizationService;
    protected readonly IRecoveryFormService _recoveryFormService;
    protected readonly IPersonService _personService;
    protected readonly IPictureService _pictureService;

    #endregion

    #region Ctor

    public RecoveryFormModelFactory(
        IAgencyService agencyService,
        IAntiXService antiXService,
        IClinicService clinicService,
        ICityService cityService,
        IDateTimeHelper dateTimeHelper,
        ILocalizationService localizationService,
        IRecoveryFormService recoveryFormService,
        IPersonService personService,
        IPictureService pictureService)
    {
        _agencyService = agencyService;
        _antiXService = antiXService;
        _clinicService = clinicService;
        _cityService = cityService;
        _dateTimeHelper = dateTimeHelper;
        _localizationService = localizationService;
        _recoveryFormService = recoveryFormService;
        _personService = personService;
        _pictureService = pictureService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare recovery form search model
    /// </summary>
    /// <param name="searchModel">Recovery form search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery form search model
    /// </returns>
    public virtual async Task<RecoveryFormSearchModel> PrepareRecoveryFormSearchModelAsync(RecoveryFormSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        var cities = await _cityService.GetAllCitiesAsync(pageIndex: 0, pageSize: int.MaxValue);
        searchModel.AvailableCities.Add(new SelectListItem
        {
            Value = "-1",
            Text = await _localizationService.GetResourceAsync("Admin.Passengers.Fields.City.ShowAllAgenciesClinics"),
            Selected = searchModel.SearchCityId == -1
        });
        searchModel.AvailableCities.Add(new SelectListItem
        {
            Value = "0",
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select")
        });
        foreach (var city in cities)
        {
            searchModel.AvailableCities.Add(new SelectListItem
            {
                Value = city.Id.ToString(),
                Text = city.Name,
                Selected = city.Id == searchModel.SearchCityId
            });
        }

        searchModel.AvailableAgencies.Add(new SelectListItem
        {
            Value = "0",
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select")
        });
        if (searchModel.SearchCityId > 0)
        {
            var agencies = await _agencyService.GetAgenciesByCityIdAsync(searchModel.SearchCityId, showHidden: true);
            foreach (var agency in agencies)
            {
                searchModel.AvailableAgencies.Add(new SelectListItem
                {
                    Value = agency.Id.ToString(),
                    Text = agency.Name,
                    Selected = agency.Id == searchModel.SearchAgencyId
                });
            }
        }

        searchModel.AvailableClinics.Add(new SelectListItem
        {
            Value = "0",
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select")
        });
        if (searchModel.SearchCityId > 0)
        {
            var clinics = await _clinicService.GetClinicsByCityIdAsync(searchModel.SearchCityId, showHidden: true);
            foreach (var clinic in clinics)
            {
                searchModel.AvailableClinics.Add(new SelectListItem
                {
                    Value = clinic.Id.ToString(),
                    Text = clinic.Name,
                    Selected = clinic.Id == searchModel.SearchClinicId
                });
            }
        }

        searchModel.AvailableAntiXItems.Add(new SelectListItem
        {
            Value = "0",
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select")
        });
        var antiXItems = await _antiXService.GetAllAntiXAsync(pageIndex: 0, pageSize: int.MaxValue);
        foreach (var item in antiXItems)
        {
            searchModel.AvailableAntiXItems.Add(new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = item.Id == searchModel.SearchAntiXId
            });
        }

        //prepare available recovery years (distinct Persian years from TravelEndDateUtc)
        searchModel.AvailableRecoveryYears.Add(new SelectListItem
        {
            Value = "0",
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select")
        });
        var availableYears = await _recoveryFormService.GetAvailableRecoveryYearsAsync();
        foreach (var year in availableYears)
        {
            searchModel.AvailableRecoveryYears.Add(new SelectListItem
            {
                Value = year.ToString(),
                Text = year.ToString(),
                Selected = searchModel.SearchRecoveryYear.HasValue && searchModel.SearchRecoveryYear.Value == year
            });
        }

        //prepare available recovery months (Persian months)
        searchModel.AvailableRecoveryMonths.Add(new SelectListItem
        {
            Value = "0",
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select")
        });
        var persianMonthNames = new[]
        {
            "فروردین", "اردیبهشت", "خرداد",
            "تیر", "مرداد", "شهریور",
            "مهر", "آبان", "آذر",
            "دی", "بهمن", "اسفند"
        };
        for (var i = 1; i <= 12; i++)
        {
            searchModel.AvailableRecoveryMonths.Add(new SelectListItem
            {
                Value = i.ToString(),
                Text = persianMonthNames[i - 1],
                Selected = searchModel.SearchRecoveryMonth.HasValue && searchModel.SearchRecoveryMonth.Value == i
            });
        }

        return searchModel;
    }

    /// <summary>
    /// Prepare paged recovery form list model
    /// </summary>
    /// <param name="searchModel">Recovery form search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery form list model
    /// </returns>
    public virtual async Task<RecoveryFormListModel> PrepareRecoveryFormListModelAsync(RecoveryFormSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var searchRecoveryNo = string.IsNullOrWhiteSpace(searchModel.SearchRecoveryNo)
            ? null
            : _recoveryFormService.NormalizeRecoveryNo(searchModel.SearchRecoveryNo, null);
        var searchCardNo = string.IsNullOrWhiteSpace(searchModel.SearchCardNo)
            ? null
            : DigitHelper.ToEnglishDigits(searchModel.SearchCardNo.Trim());

        //get recovery forms
        var recoveryForms = await _recoveryFormService.GetAllRecoveryFormsAsync(
            recoveryNo: searchRecoveryNo,
            personName: searchModel.SearchPersonName,
            cityId: searchModel.SearchCityId,
            agencyId: searchModel.SearchAgencyId,
            clinicId: searchModel.SearchClinicId,
            antiXId: searchModel.SearchAntiXId,
            guideNameAndLegionNo: searchModel.SearchGuideNameAndLegionNo,
            cardNo: searchCardNo,
            travelStartDateUtc: searchModel.SearchTravelStartDateUtc,
            travelEndDateUtc: searchModel.SearchTravelEndDateUtc,
            recoveryYear: searchModel.SearchRecoveryYear,
            recoveryMonth: searchModel.SearchRecoveryMonth,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var agencyIds = recoveryForms.Select(recoveryForm => recoveryForm.AgencyId)
            .Distinct()
            .Where(id => id > 0)
            .ToArray();
        var antiXIds = recoveryForms.SelectMany(recoveryForm => new int?[] { recoveryForm.AntiX1, recoveryForm.AntiX2 })
            .Where(id => id.HasValue && id.Value > 0)
            .Select(id => id.Value)
            .Distinct()
            .ToArray();

        var personIds = recoveryForms.Select(recoveryForm => recoveryForm.PersonId)
            .Where(id => id > 0)
            .Distinct()
            .ToArray();

        var agencies = await _agencyService.GetAgenciesByIdsAsync(agencyIds);
        var antiXItems = await _antiXService.GetAntiXByIdsAsync(antiXIds);
        var persons = await _personService.GetPersonsByIdsAsync(personIds);
        var agencyNames = agencies.ToDictionary(agency => agency.Id, agency => agency.Name);
        var antiXNames = antiXItems.ToDictionary(item => item.Id, item => item.Name);
        var personsById = persons.ToDictionary(person => person.Id);

        //prepare list model
        var model = await new RecoveryFormListModel().PrepareToGridAsync(searchModel, recoveryForms, () =>
        {
            return recoveryForms.SelectAwait(async recoveryForm =>
            {
                //fill in model values from the entity
                var recoveryFormModel = recoveryForm.ToModel<RecoveryFormModel>();

                //fill identity values from the linked person
                if (personsById.TryGetValue(recoveryForm.PersonId, out var person))
                {
                    recoveryFormModel.PersonName = person.FirstName;
                    recoveryFormModel.CardNo = person.CardNo;
                    recoveryFormModel.BirthYear = person.BirthYear;
                    recoveryFormModel.MobileNumber = person.MobileNumber;
                }

                //convert dates to the user time
                recoveryFormModel.CreatedOnUtc = await _dateTimeHelper.ConvertToUserTimeAsync(recoveryForm.CreatedOnUtc, DateTimeKind.Utc);
                if (recoveryForm.TravelStartDateUtc.HasValue)
                    recoveryFormModel.TravelStartDateUtc = await _dateTimeHelper.ConvertToUserTimeAsync(recoveryForm.TravelStartDateUtc.Value, DateTimeKind.Utc);
                if (recoveryForm.TravelEndDateUtc.HasValue)
                    recoveryFormModel.TravelEndDateUtc = await _dateTimeHelper.ConvertToUserTimeAsync(recoveryForm.TravelEndDateUtc.Value, DateTimeKind.Utc);

                recoveryFormModel.EndDateOnPersian = new PersianDateTime(recoveryFormModel.TravelEndDateUtc)
                {
                    EnglishNumber = true
                }.ToString("yyyy/MM/dd");
                recoveryFormModel.StartDateOnPersian = recoveryFormModel.TravelStartDateUtc.HasValue
                    ? new PersianDateTime(recoveryFormModel.TravelStartDateUtc.Value) { EnglishNumber = true }.ToString("yyyy/MM/dd")
                    : string.Empty;

                if (recoveryForm.PictureId > 0)
                {
                    recoveryFormModel.PictureUrl = await _pictureService.GetPictureUrlAsync(
                        recoveryForm.PictureId,
                        targetSize: 60,
                        showDefaultPicture: false);
                    recoveryFormModel.PictureFullSizeUrl = await _pictureService.GetPictureUrlAsync(
                        recoveryForm.PictureId,
                        targetSize: 0,
                        showDefaultPicture: false);
                }

                recoveryFormModel.AgencyName = agencyNames.TryGetValue(recoveryForm.AgencyId, out var agencyName)
                    ? agencyName
                    : string.Empty;
                recoveryFormModel.AntiX1Name = antiXNames.TryGetValue(recoveryForm.AntiX1, out var antiX1Name)
                    ? antiX1Name
                    : string.Empty;
                recoveryFormModel.AntiX2Name = recoveryForm.AntiX2.HasValue &&
                                            antiXNames.TryGetValue(recoveryForm.AntiX2.Value, out var antiX2Name)
                    ? antiX2Name
                    : string.Empty;

                return recoveryFormModel;
            });
        });

        return model;
    }

    /// <summary>
    /// Prepare recovery form model
    /// </summary>
    /// <param name="model">Recovery form model</param>
    /// <param name="recoveryForm">Recovery form</param>
    /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery form model
    /// </returns>
    public virtual async Task<RecoveryFormModel> PrepareRecoveryFormModelAsync(RecoveryFormModel model, RecoveryForm recoveryForm, bool excludeProperties = false)
    {
        if (recoveryForm != null)
        {
            //fill in model values from the entity
            model ??= new RecoveryFormModel();
            if (!excludeProperties)
            {
                model = recoveryForm.ToModel(model);
                //convert enum values
                model.Education = (int)recoveryForm.Education;

                //fill identity values from the linked person
                var person = await _personService.GetPersonByIdAsync(recoveryForm.PersonId);
                if (person != null)
                {
                    model.PersonName = person.FirstName;
                    model.CardNo = person.CardNo;
                    model.BirthYear = person.BirthYear;
                    model.MobileNumber = person.MobileNumber;
                }

                //convert dates to the user time
                model.CreatedOnUtc = await _dateTimeHelper.ConvertToUserTimeAsync(recoveryForm.CreatedOnUtc, DateTimeKind.Utc);
                if (recoveryForm.TravelStartDateUtc.HasValue)
                    model.TravelStartDateUtc = await _dateTimeHelper.ConvertToUserTimeAsync(recoveryForm.TravelStartDateUtc.Value, DateTimeKind.Utc);
                if (recoveryForm.TravelEndDateUtc.HasValue)
                    model.TravelEndDateUtc = await _dateTimeHelper.ConvertToUserTimeAsync(recoveryForm.TravelEndDateUtc.Value, DateTimeKind.Utc);

                if (model.CityId == 0 && model.AgencyId > 0)
                {
                    var agency = await _agencyService.GetAgencyByIdAsync(model.AgencyId);
                    if (agency != null)
                        model.CityId = agency.CityId;
                }

                if (model.CityId == 0 && model.ClinicId.GetValueOrDefault() > 0)
                {
                    var clinic = await _clinicService.GetClinicByIdAsync(model.ClinicId.Value);
                    if (clinic != null)
                        model.CityId = clinic.CityId;
                }
            }
        }

        model ??= new RecoveryFormModel();

        if (model.CityId == 0 && model.AgencyId > 0)
        {
            var agency = await _agencyService.GetAgencyByIdAsync(model.AgencyId);
            if (agency != null)
                model.CityId = agency.CityId;
        }

        if (model.CityId == 0 && model.ClinicId.GetValueOrDefault() > 0)
        {
            var clinic = await _clinicService.GetClinicByIdAsync(model.ClinicId.Value);
            if (clinic != null)
                model.CityId = clinic.CityId;
        }

        //prepare available education levels
        var educationLevels = new List<SelectListItem>();
        foreach (EducationLevel e in Enum.GetValues(typeof(EducationLevel)))
        {
            educationLevels.Add(new SelectListItem
            {
                Text = await _localizationService.GetLocalizedEnumAsync(e),
                Value = ((int)e).ToString(),
                Selected = recoveryForm != null && (int)e == (int)recoveryForm.Education
            });
        }
        model.AvailableEducationLevels = educationLevels;

        //prepare available AntiX items
        var antiXItems = await _antiXService.GetAllAntiXAsync(published: null, pageIndex: 0, pageSize: int.MaxValue);
        model.AvailableAntiXItems.Add(new SelectListItem
        {
            Value = "0",
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select")
        });
        foreach (var item in antiXItems)
        {
            model.AvailableAntiXItems.Add(new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            });
        }

        //prepare available cities
        var cities = await _cityService.GetAllCitiesAsync(pageIndex: 0, pageSize: int.MaxValue);
        model.AvailableCities.Add(new SelectListItem
        {
            Value = "-1",
            Text = await _localizationService.GetResourceAsync("Admin.Passengers.Fields.City.ShowAllAgenciesClinics"),
            Selected = model.CityId == -1
        });
        model.AvailableCities.Add(new SelectListItem
        {
            Value = "0",
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select")
        });
        foreach (var city in cities)
        {
            model.AvailableCities.Add(new SelectListItem
            {
                Value = city.Id.ToString(),
                Text = city.Name,
                Selected = city.Id == model.CityId
            });
        }

        //prepare available agencies
        model.AvailableAgencies.Add(new SelectListItem
        {
            Value = "0",
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select")
        });
        if (model.CityId > 0)
        {
            var agencies = await _agencyService.GetAgenciesByCityIdAsync(model.CityId, showHidden: true);
            foreach (var agency in agencies)
            {
                model.AvailableAgencies.Add(new SelectListItem
                {
                    Value = agency.Id.ToString(),
                    Text = agency.Name,
                    Selected = agency.Id == model.AgencyId
                });
            }
        }

        //prepare available clinics
        model.AvailableClinics.Add(new SelectListItem
        {
            Value = "0",
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select")
        });
        if (model.CityId > 0)
        {
            var clinics = await _clinicService.GetClinicsByCityIdAsync(model.CityId, showHidden: true);
            foreach (var clinic in clinics)
            {
                model.AvailableClinics.Add(new SelectListItem
                {
                    Value = clinic.Id.ToString(),
                    Text = clinic.Name,
                    Selected = clinic.Id == model.ClinicId
                });
            }
        }

        return model;
    }

    #endregion
}
