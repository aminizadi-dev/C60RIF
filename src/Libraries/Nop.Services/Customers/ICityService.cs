using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers;

/// <summary>
/// City service interface
/// </summary>
public partial interface ICityService
{
    #region Cities

    /// <summary>
    /// Gets all cities
    /// </summary>
    /// <param name="name">City name; null to load all cities</param>
    /// <param name="published">Published status; null to load all cities</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">A value indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the cities
    /// </returns>
    Task<IPagedList<City>> GetAllCitiesAsync(string name = null, bool? published = null, 
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

    /// <summary>
    /// Gets a city
    /// </summary>
    /// <param name="cityId">City identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the city
    /// </returns>
    Task<City> GetCityByIdAsync(int cityId);

    /// <summary>
    /// Gets cities by identifiers
    /// </summary>
    /// <param name="cityIds">City identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the cities
    /// </returns>
    Task<IList<City>> GetCitiesByIdsAsync(int[] cityIds);

    /// <summary>
    /// Inserts a city
    /// </summary>
    /// <param name="city">City</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertCityAsync(City city);

    /// <summary>
    /// Updates the city
    /// </summary>
    /// <param name="city">City</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateCityAsync(City city);

    /// <summary>
    /// Delete a city
    /// </summary>
    /// <param name="city">City</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteCityAsync(City city);

    #endregion
}

