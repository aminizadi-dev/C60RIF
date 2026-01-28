using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
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

    protected readonly IDateTimeHelper _dateTimeHelper;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPassengerService _passengerService;

    #endregion

    #region Ctor

    public PassengerModelFactory(
        IDateTimeHelper dateTimeHelper,
        ILocalizationService localizationService,
        IPassengerService passengerService)
    {
        _dateTimeHelper = dateTimeHelper;
        _localizationService = localizationService;
        _passengerService = passengerService;
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
    public virtual Task<PassengerSearchModel> PreparePassengerSearchModelAsync(PassengerSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
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
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

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
            }
        }

        model ??= new PassengerModel();

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

        return model;
    }

    #endregion
}

