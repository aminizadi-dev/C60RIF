using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the city model factory implementation
/// </summary>
public partial class CityModelFactory : ICityModelFactory
{
    #region Fields

    protected readonly IAgencyService _agencyService;
    protected readonly ICityService _cityService;

    #endregion

    #region Ctor

    public CityModelFactory(
        IAgencyService agencyService,
        ICityService cityService)
    {
        _agencyService = agencyService;
        _cityService = cityService;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Prepare agency search model
    /// </summary>
    /// <param name="searchModel">Agency search model</param>
    /// <param name="city">City</param>
    public virtual void PrepareAgencySearchModel(AgencySearchModel searchModel, City city)
    {
        ArgumentNullException.ThrowIfNull(searchModel);
        ArgumentNullException.ThrowIfNull(city);

        searchModel.CityId = city.Id;

        //prepare page parameters
        searchModel.SetGridPageSize();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare city search model
    /// </summary>
    /// <param name="searchModel">City search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the city search model
    /// </returns>
    public virtual Task<CitySearchModel> PrepareCitySearchModelAsync(CitySearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    /// <summary>
    /// Prepare paged city list model
    /// </summary>
    /// <param name="searchModel">City search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the city list model
    /// </returns>
    public virtual async Task<CityListModel> PrepareCityListModelAsync(CitySearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var cities = await _cityService.GetAllCitiesAsync(
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var cityModels = new List<CityModel>();
        foreach (var city in cities)
        {
            var cityModel = city.ToModel<CityModel>();
            cityModel.NumberOfAgencies = (await _agencyService.GetAllAgenciesAsync(
                cityId: city.Id,
                getOnlyTotalCount: true)).TotalCount;
            cityModels.Add(cityModel);
        }

        var model = new CityListModel().PrepareToGrid(searchModel, cities, () => cityModels);

        return model;
    }

    /// <summary>
    /// Prepare city model
    /// </summary>
    /// <param name="model">City model</param>
    /// <param name="city">City</param>
    /// <param name="excludeProperties">Whether to exclude properties</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the city model
    /// </returns>
    public virtual async Task<CityModel> PrepareCityModelAsync(CityModel model, City city, bool excludeProperties = false)
    {
        if (city != null)
        {
            model ??= city.ToModel<CityModel>();
            model.NumberOfAgencies = (await _agencyService.GetAllAgenciesAsync(
                cityId: city.Id,
                getOnlyTotalCount: true)).TotalCount;
        }

        //set default values for the new model
        if (city == null)
        {
            model ??= new CityModel();
            model.Published = true;
        }

        if (city != null)
            PrepareAgencySearchModel(model.AgencySearchModel, city);

        return model;
    }

    /// <summary>
    /// Prepare paged agency list model
    /// </summary>
    /// <param name="searchModel">Agency search model</param>
    /// <param name="city">City</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the agency list model
    /// </returns>
    public virtual async Task<AgencyListModel> PrepareAgencyListModelAsync(AgencySearchModel searchModel, City city)
    {
        ArgumentNullException.ThrowIfNull(searchModel);
        ArgumentNullException.ThrowIfNull(city);

        var agencies = (await _agencyService.GetAgenciesByCityIdAsync(city.Id, showHidden: true)).ToPagedList(searchModel);

        var model = new AgencyListModel().PrepareToGrid(searchModel, agencies, () =>
        {
            return agencies.Select(agency => agency.ToModel<AgencyModel>());
        });

        return model;
    }

    /// <summary>
    /// Prepare agency model
    /// </summary>
    /// <param name="model">Agency model</param>
    /// <param name="city">City</param>
    /// <param name="agency">Agency</param>
    /// <param name="excludeProperties">Whether to exclude properties</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the agency model
    /// </returns>
    public virtual Task<AgencyModel> PrepareAgencyModelAsync(AgencyModel model, City city, Agency agency, bool excludeProperties = false)
    {
        ArgumentNullException.ThrowIfNull(city);

        if (agency != null)
            model ??= agency.ToModel<AgencyModel>();

        //set default values for the new model
        if (agency == null)
        {
            model ??= new AgencyModel();
            model.Published = true;
        }

        model.CityId = city.Id;

        return Task.FromResult(model);
    }

    #endregion
}

