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
/// Represents the passenger model factory implementation
/// </summary>
public partial class PassengerModelFactory : IPassengerModelFactory
{
    #region Fields

    protected readonly IAgencyService _agencyService;
    protected readonly IAntiXService _antiXService;
    protected readonly ICityService _cityService;
    protected readonly IDateTimeHelper _dateTimeHelper;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPassengerService _passengerService;
    protected readonly IPictureService _pictureService;

    #endregion

    #region Ctor

    public PassengerModelFactory(
        IAgencyService agencyService,
        IAntiXService antiXService,
        ICityService cityService,
        IDateTimeHelper dateTimeHelper,
        ILocalizationService localizationService,
        IPassengerService passengerService,
        IPictureService pictureService)
    {
        _agencyService = agencyService;
        _antiXService = antiXService;
        _cityService = cityService;
        _dateTimeHelper = dateTimeHelper;
        _localizationService = localizationService;
        _passengerService = passengerService;
        _pictureService = pictureService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare passenger search model
    /// </summary>
    /// <param name="searchModel">Passenger search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the passenger search model
    /// </returns>
    public virtual async Task<PassengerSearchModel> PreparePassengerSearchModelAsync(PassengerSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        var cities = await _cityService.GetAllCitiesAsync(pageIndex: 0, pageSize: int.MaxValue);
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

        return searchModel;
    }

    /// <summary>
    /// Prepare paged passenger list model
    /// </summary>
    /// <param name="searchModel">Passenger search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the passenger list model
    /// </returns>
    public virtual async Task<PassengerListModel> PreparePassengerListModelAsync(PassengerSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get passengers
        var passengers = await _passengerService.GetAllPassengersAsync(
            recoveryNo: searchModel.SearchRecoveryNo,
            personName: searchModel.SearchPersonName,
            cityId: searchModel.SearchCityId,
            agencyId: searchModel.SearchAgencyId,
            antiXId: searchModel.SearchAntiXId,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var agencyIds = passengers.Select(passenger => passenger.AgencyId)
            .Distinct()
            .Where(id => id > 0)
            .ToArray();
        var antiXIds = passengers.SelectMany(passenger => new int?[] { passenger.AntiX1, passenger.AntiX2 })
            .Where(id => id.HasValue && id.Value > 0)
            .Select(id => id.Value)
            .Distinct()
            .ToArray();

        var agencies = await _agencyService.GetAgenciesByIdsAsync(agencyIds);
        var antiXItems = await _antiXService.GetAntiXByIdsAsync(antiXIds);
        var agencyNames = agencies.ToDictionary(agency => agency.Id, agency => agency.Name);
        var antiXNames = antiXItems.ToDictionary(item => item.Id, item => item.Name);

        //prepare list model
        var model = await new PassengerListModel().PrepareToGridAsync(searchModel, passengers, () =>
        {
            return passengers.SelectAwait(async passenger =>
            {
                //fill in model values from the entity
                var passengerModel = passenger.ToModel<PassengerModel>();

                //convert dates to the user time
                passengerModel.CreatedOnUtc = await _dateTimeHelper.ConvertToUserTimeAsync(passenger.CreatedOnUtc, DateTimeKind.Utc);
                if (passenger.BirthDateUtc.HasValue)
                    passengerModel.BirthDateUtc = await _dateTimeHelper.ConvertToUserTimeAsync(passenger.BirthDateUtc.Value, DateTimeKind.Utc);
                if (passenger.TravelStartDateUtc.HasValue)
                    passengerModel.TravelStartDateUtc = await _dateTimeHelper.ConvertToUserTimeAsync(passenger.TravelStartDateUtc.Value, DateTimeKind.Utc);
                if (passenger.TravelEndDateUtc.HasValue)
                    passengerModel.TravelEndDateUtc = await _dateTimeHelper.ConvertToUserTimeAsync(passenger.TravelEndDateUtc.Value, DateTimeKind.Utc);

                passengerModel.EndDateOnPersian = new PersianDateTime(passengerModel.TravelEndDateUtc)
                {
                    EnglishNumber = true
                }.ToString("yyyy/MM/dd");

                if (passenger.PictureId > 0)
                {
                    passengerModel.PictureUrl = await _pictureService.GetPictureUrlAsync(
                        passenger.PictureId,
                        targetSize: 60,
                        showDefaultPicture: false);
                    passengerModel.PictureFullSizeUrl = await _pictureService.GetPictureUrlAsync(
                        passenger.PictureId,
                        targetSize: 0,
                        showDefaultPicture: false);
                }

                passengerModel.AgencyName = agencyNames.TryGetValue(passenger.AgencyId, out var agencyName)
                    ? agencyName
                    : string.Empty;
                passengerModel.AntiX1Name = antiXNames.TryGetValue(passenger.AntiX1, out var antiX1Name)
                    ? antiX1Name
                    : string.Empty;
                passengerModel.AntiX2Name = passenger.AntiX2.HasValue &&
                                            antiXNames.TryGetValue(passenger.AntiX2.Value, out var antiX2Name)
                    ? antiX2Name
                    : string.Empty;

                return passengerModel;
            });
        });

        return model;
    }

    /// <summary>
    /// Prepare passenger model
    /// </summary>
    /// <param name="model">Passenger model</param>
    /// <param name="passenger">Passenger</param>
    /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the passenger model
    /// </returns>
    public virtual async Task<PassengerModel> PreparePassengerModelAsync(PassengerModel model, Passenger passenger, bool excludeProperties = false)
    {
        if (passenger != null)
        {
            //fill in model values from the entity
            model ??= new PassengerModel();
            if (!excludeProperties)
            {
                model = passenger.ToModel(model);
                //convert enum values
                model.Education = (int)passenger.Education;
                model.MaritalStatus = (int)passenger.MaritalStatus;
                model.EmploymentStatus = (int)passenger.EmploymentStatus;
                //convert dates to the user time
                model.CreatedOnUtc = await _dateTimeHelper.ConvertToUserTimeAsync(passenger.CreatedOnUtc, DateTimeKind.Utc);
                if (passenger.BirthDateUtc.HasValue)
                    model.BirthDateUtc = await _dateTimeHelper.ConvertToUserTimeAsync(passenger.BirthDateUtc.Value, DateTimeKind.Utc);
                if (passenger.TravelStartDateUtc.HasValue)
                    model.TravelStartDateUtc = await _dateTimeHelper.ConvertToUserTimeAsync(passenger.TravelStartDateUtc.Value, DateTimeKind.Utc);
                if (passenger.TravelEndDateUtc.HasValue)
                    model.TravelEndDateUtc = await _dateTimeHelper.ConvertToUserTimeAsync(passenger.TravelEndDateUtc.Value, DateTimeKind.Utc);

                if (model.CityId == 0 && model.AgencyId > 0)
                {
                    var agency = await _agencyService.GetAgencyByIdAsync(model.AgencyId);
                    if (agency != null)
                        model.CityId = agency.CityId;
                }
            }
        }

        model ??= new PassengerModel();

        if (model.CityId == 0 && model.AgencyId > 0)
        {
            var agency = await _agencyService.GetAgencyByIdAsync(model.AgencyId);
            if (agency != null)
                model.CityId = agency.CityId;
        }

        //prepare available education levels
        var educationLevels = new List<SelectListItem>();
        foreach (EducationLevel e in Enum.GetValues(typeof(EducationLevel)))
        {
            educationLevels.Add(new SelectListItem
            {
                Text = await _localizationService.GetLocalizedEnumAsync(e),
                Value = ((int)e).ToString(),
                Selected = passenger != null && (int)e == (int)passenger.Education
            });
        }
        model.AvailableEducationLevels = educationLevels;

        //prepare available marital statuses
        var maritalStatuses = new List<SelectListItem>();
        foreach (MaritalStatus m in Enum.GetValues(typeof(MaritalStatus)))
        {
            maritalStatuses.Add(new SelectListItem
            {
                Text = await _localizationService.GetLocalizedEnumAsync(m),
                Value = ((int)m).ToString(),
                Selected = passenger != null && (int)m == (int)passenger.MaritalStatus
            });
        }
        model.AvailableMaritalStatuses = maritalStatuses;

        //prepare available employment statuses
        var employmentStatuses = new List<SelectListItem>();
        foreach (EmploymentStatus e in Enum.GetValues(typeof(EmploymentStatus)))
        {
            employmentStatuses.Add(new SelectListItem
            {
                Text = await _localizationService.GetLocalizedEnumAsync(e),
                Value = ((int)e).ToString(),
                Selected = passenger != null && (int)e == (int)passenger.EmploymentStatus
            });
        }
        model.AvailableEmploymentStatuses = employmentStatuses;

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

        return model;
    }

    #endregion
}

