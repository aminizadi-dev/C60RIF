using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Data;

namespace Nop.Services.Customers;

/// <summary>
/// City service
/// </summary>
public partial class CityService : ICityService
{
    #region Fields

    protected readonly IEventPublisher _eventPublisher;
    protected readonly IRepository<City> _cityRepository;

    #endregion

    #region Ctor

    public CityService(
        IEventPublisher eventPublisher,
        IRepository<City> cityRepository)
    {
        _eventPublisher = eventPublisher;
        _cityRepository = cityRepository;
    }

    #endregion

    #region Methods

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
    public virtual async Task<IPagedList<City>> GetAllCitiesAsync(string name = null, bool? published = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        var cities = await _cityRepository.GetAllPagedAsync(query =>
        {
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name));
            if (published.HasValue)
                query = query.Where(c => c.Published == published.Value);

            query = query.OrderBy(c => c.Name).ThenBy(c => c.DisplayOrder);

            return query;
        }, pageIndex, pageSize, getOnlyTotalCount);

        return cities;
    }

    /// <summary>
    /// Gets a city
    /// </summary>
    /// <param name="cityId">City identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the city
    /// </returns>
    public virtual async Task<City> GetCityByIdAsync(int cityId)
    {
        return await _cityRepository.GetByIdAsync(cityId);
    }

    /// <summary>
    /// Gets cities by identifiers
    /// </summary>
    /// <param name="cityIds">City identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the cities
    /// </returns>
    public virtual async Task<IList<City>> GetCitiesByIdsAsync(int[] cityIds)
    {
        if (cityIds == null || cityIds.Length == 0)
            return new List<City>();

        var query = await _cityRepository.GetAllAsync(query =>
        {
            query = query.Where(c => cityIds.Contains(c.Id));
            return query;
        });

        return query.ToList();
    }

    /// <summary>
    /// Inserts a city
    /// </summary>
    /// <param name="city">City</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertCityAsync(City city)
    {
        ArgumentNullException.ThrowIfNull(city);

        await _cityRepository.InsertAsync(city);
        await _eventPublisher.EntityInsertedAsync(city);
    }

    /// <summary>
    /// Updates the city
    /// </summary>
    /// <param name="city">City</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateCityAsync(City city)
    {
        ArgumentNullException.ThrowIfNull(city);

        await _cityRepository.UpdateAsync(city);
        await _eventPublisher.EntityUpdatedAsync(city);
    }

    /// <summary>
    /// Delete a city
    /// </summary>
    /// <param name="city">City</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteCityAsync(City city)
    {
        ArgumentNullException.ThrowIfNull(city);

        await _cityRepository.DeleteAsync(city);
        await _eventPublisher.EntityDeletedAsync(city);
    }

    #endregion
}

