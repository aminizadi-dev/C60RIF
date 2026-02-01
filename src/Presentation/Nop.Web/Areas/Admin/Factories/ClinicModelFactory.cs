using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the clinic model factory implementation
/// </summary>
public partial class ClinicModelFactory : IClinicModelFactory
{
    #region Fields

    protected readonly ICityService _cityService;
    protected readonly IClinicService _clinicService;
    protected readonly ILocalizationService _localizationService;

    #endregion

    #region Ctor

    public ClinicModelFactory(
        ICityService cityService,
        IClinicService clinicService,
        ILocalizationService localizationService)
    {
        _cityService = cityService;
        _clinicService = clinicService;
        _localizationService = localizationService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare clinic search model
    /// </summary>
    /// <param name="searchModel">Clinic search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinic search model
    /// </returns>
    public virtual Task<ClinicSearchModel> PrepareClinicSearchModelAsync(ClinicSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    /// <summary>
    /// Prepare paged clinic list model
    /// </summary>
    /// <param name="searchModel">Clinic search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinic list model
    /// </returns>
    public virtual async Task<ClinicListModel> PrepareClinicListModelAsync(ClinicSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var clinics = await _clinicService.GetAllClinicsAsync(
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var cityIds = clinics.Select(clinic => clinic.CityId)
            .Distinct()
            .ToArray();
        var cities = await _cityService.GetCitiesByIdsAsync(cityIds);
        var cityNames = cities.ToDictionary(city => city.Id, city => city.Name);

        var model = new ClinicListModel().PrepareToGrid(searchModel, clinics, () =>
        {
            return clinics.Select(clinic =>
            {
                var clinicModel = clinic.ToModel<ClinicModel>();
                clinicModel.CityName = cityNames.TryGetValue(clinic.CityId, out var cityName)
                    ? cityName
                    : string.Empty;
                return clinicModel;
            });
        });

        return model;
    }

    /// <summary>
    /// Prepare clinic model
    /// </summary>
    /// <param name="model">Clinic model</param>
    /// <param name="clinic">Clinic</param>
    /// <param name="excludeProperties">Whether to exclude properties</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinic model
    /// </returns>
    public virtual async Task<ClinicModel> PrepareClinicModelAsync(ClinicModel model, Clinic clinic, bool excludeProperties = false)
    {
        if (clinic != null)
        {
            model ??= clinic.ToModel<ClinicModel>();
        }

        if (clinic == null)
        {
            model ??= new ClinicModel();
            model.Published = true;
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

        return model;
    }

    #endregion
}

