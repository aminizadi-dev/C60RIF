using Nop.Core.Domain.Customers;
using Nop.Web.Areas.Admin.Models.Customers;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the city model factory
/// </summary>
public partial interface ICityModelFactory
{
    /// <summary>
    /// Prepare city search model
    /// </summary>
    /// <param name="searchModel">City search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the city search model
    /// </returns>
    Task<CitySearchModel> PrepareCitySearchModelAsync(CitySearchModel searchModel);

    /// <summary>
    /// Prepare paged city list model
    /// </summary>
    /// <param name="searchModel">City search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the city list model
    /// </returns>
    Task<CityListModel> PrepareCityListModelAsync(CitySearchModel searchModel);

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
    Task<CityModel> PrepareCityModelAsync(CityModel model, City city, bool excludeProperties = false);

    /// <summary>
    /// Prepare agency search model
    /// </summary>
    /// <param name="searchModel">Agency search model</param>
    /// <param name="city">City</param>
    void PrepareAgencySearchModel(AgencySearchModel searchModel, City city);

    /// <summary>
    /// Prepare paged agency list model
    /// </summary>
    /// <param name="searchModel">Agency search model</param>
    /// <param name="city">City</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the agency list model
    /// </returns>
    Task<AgencyListModel> PrepareAgencyListModelAsync(AgencySearchModel searchModel, City city);

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
    Task<AgencyModel> PrepareAgencyModelAsync(AgencyModel model, City city, Agency agency, bool excludeProperties = false);

    /// <summary>
    /// Prepare clinic search model
    /// </summary>
    /// <param name="searchModel">Clinic search model</param>
    /// <param name="city">City</param>
    void PrepareClinicSearchModel(ClinicSearchModel searchModel, City city);

    /// <summary>
    /// Prepare paged clinic list model
    /// </summary>
    /// <param name="searchModel">Clinic search model</param>
    /// <param name="city">City</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinic list model
    /// </returns>
    Task<ClinicListModel> PrepareClinicListModelAsync(ClinicSearchModel searchModel, City city);

    /// <summary>
    /// Prepare clinic model
    /// </summary>
    /// <param name="model">Clinic model</param>
    /// <param name="city">City</param>
    /// <param name="clinic">Clinic</param>
    /// <param name="excludeProperties">Whether to exclude properties</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinic model
    /// </returns>
    Task<ClinicModel> PrepareClinicModelAsync(ClinicModel model, City city, Clinic clinic, bool excludeProperties = false);
}

