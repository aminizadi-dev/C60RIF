using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Data;

namespace Nop.Services.Customers;

/// <summary>
/// Agency service
/// </summary>
public partial class AgencyService : IAgencyService
{
    #region Fields

    protected readonly IEventPublisher _eventPublisher;
    protected readonly IRepository<Agency> _agencyRepository;

    #endregion

    #region Ctor

    public AgencyService(
        IEventPublisher eventPublisher,
        IRepository<Agency> agencyRepository)
    {
        _eventPublisher = eventPublisher;
        _agencyRepository = agencyRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets all agencies
    /// </summary>
    /// <param name="cityId">City identifier; 0 to load all agencies</param>
    /// <param name="name">Agency name; null to load all agencies</param>
    /// <param name="published">Published status; null to load all agencies</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">A value indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the agencies
    /// </returns>
    public virtual async Task<IPagedList<Agency>> GetAllAgenciesAsync(int cityId = 0, string name = null, bool? published = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        var agencies = await _agencyRepository.GetAllPagedAsync(query =>
        {
            if (cityId > 0)
                query = query.Where(a => a.CityId == cityId);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(a => a.Name.Contains(name));
            if (published.HasValue)
                query = query.Where(a => a.Published == published.Value);

            query = query.OrderBy(a => a.Name).ThenBy(a => a.DisplayOrder);

            return query;
        }, pageIndex, pageSize, getOnlyTotalCount);

        return agencies;
    }

    /// <summary>
    /// Gets a agency
    /// </summary>
    /// <param name="agencyId">Agency identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the agency
    /// </returns>
    public virtual async Task<Agency> GetAgencyByIdAsync(int agencyId)
    {
        return await _agencyRepository.GetByIdAsync(agencyId);
    }

    /// <summary>
    /// Gets agencies by identifiers
    /// </summary>
    /// <param name="agencyIds">Agency identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the agencies
    /// </returns>
    public virtual async Task<IList<Agency>> GetAgenciesByIdsAsync(int[] agencyIds)
    {
        if (agencyIds == null || agencyIds.Length == 0)
            return new List<Agency>();

        var query = await _agencyRepository.GetAllAsync(query =>
        {
            query = query.Where(a => agencyIds.Contains(a.Id));
            return query;
        });

        return query.ToList();
    }

    /// <summary>
    /// Gets agencies by city identifier
    /// </summary>
    /// <param name="cityId">City identifier</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the agencies
    /// </returns>
    public virtual async Task<IList<Agency>> GetAgenciesByCityIdAsync(int cityId, bool showHidden = false)
    {
        var query = await _agencyRepository.GetAllAsync(query =>
        {
            query = query.Where(a => a.CityId == cityId);
            if (!showHidden)
                query = query.Where(a => a.Published);
            query = query.OrderBy(a => a.DisplayOrder).ThenBy(a => a.Name);
            return query;
        });

        return query.ToList();
    }

    /// <summary>
    /// Inserts a agency
    /// </summary>
    /// <param name="agency">Agency</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertAgencyAsync(Agency agency)
    {
        ArgumentNullException.ThrowIfNull(agency);

        await _agencyRepository.InsertAsync(agency);
        await _eventPublisher.EntityInsertedAsync(agency);
    }

    /// <summary>
    /// Updates the agency
    /// </summary>
    /// <param name="agency">Agency</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateAgencyAsync(Agency agency)
    {
        ArgumentNullException.ThrowIfNull(agency);

        await _agencyRepository.UpdateAsync(agency);
        await _eventPublisher.EntityUpdatedAsync(agency);
    }

    /// <summary>
    /// Delete a agency
    /// </summary>
    /// <param name="agency">Agency</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteAgencyAsync(Agency agency)
    {
        ArgumentNullException.ThrowIfNull(agency);

        await _agencyRepository.DeleteAsync(agency);
        await _eventPublisher.EntityDeletedAsync(agency);
    }

    #endregion
}

