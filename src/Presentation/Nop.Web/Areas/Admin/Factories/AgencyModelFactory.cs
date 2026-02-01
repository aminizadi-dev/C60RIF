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
/// Represents the agency model factory implementation
/// </summary>
public partial class AgencyModelFactory : IAgencyModelFactory
{
    #region Fields

    protected readonly IAgencyService _agencyService;
    protected readonly ICityService _cityService;
    protected readonly ILocalizationService _localizationService;

    #endregion

    #region Ctor

    public AgencyModelFactory(
        IAgencyService agencyService,
        ICityService cityService,
        ILocalizationService localizationService)
    {
        _agencyService = agencyService;
        _cityService = cityService;
        _localizationService = localizationService;
    }

    #endregion

    #region Methods

    public virtual Task<AgencySearchModel> PrepareAgencySearchModelAsync(AgencySearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<AgencyListModel> PrepareAgencyListModelAsync(AgencySearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var agencies = await _agencyService.GetAllAgenciesAsync(
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var cityIds = agencies.Select(agency => agency.CityId)
            .Distinct()
            .ToArray();
        var cities = await _cityService.GetCitiesByIdsAsync(cityIds);
        var cityNames = cities.ToDictionary(city => city.Id, city => city.Name);

        var model = new AgencyListModel().PrepareToGrid(searchModel, agencies, () =>
        {
            return agencies.Select(agency =>
            {
                var agencyModel = agency.ToModel<AgencyModel>();
                agencyModel.CityName = cityNames.TryGetValue(agency.CityId, out var cityName)
                    ? cityName
                    : string.Empty;
                return agencyModel;
            });
        });

        return model;
    }

    public virtual async Task<AgencyModel> PrepareAgencyModelAsync(AgencyModel model, Agency agency, bool excludeProperties = false)
    {
        if (agency != null)
        {
            model ??= agency.ToModel<AgencyModel>();
        }

        if (agency == null)
        {
            model ??= new AgencyModel();
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

