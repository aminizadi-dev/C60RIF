using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers;

/// <summary>
/// Agency service interface
/// </summary>
public partial interface IAgencyService
{
    #region Agencies

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
    Task<IPagedList<Agency>> GetAllAgenciesAsync(int cityId = 0, string name = null, bool? published = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

    /// <summary>
    /// Gets a agency
    /// </summary>
    /// <param name="agencyId">Agency identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the agency
    /// </returns>
    Task<Agency> GetAgencyByIdAsync(int agencyId);

    /// <summary>
    /// Gets agencies by identifiers
    /// </summary>
    /// <param name="agencyIds">Agency identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the agencies
    /// </returns>
    Task<IList<Agency>> GetAgenciesByIdsAsync(int[] agencyIds);

    /// <summary>
    /// Gets agencies by city identifier
    /// </summary>
    /// <param name="cityId">City identifier</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the agencies
    /// </returns>
    Task<IList<Agency>> GetAgenciesByCityIdAsync(int cityId, bool showHidden = false);

    /// <summary>
    /// Inserts a agency
    /// </summary>
    /// <param name="agency">Agency</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertAgencyAsync(Agency agency);

    /// <summary>
    /// Updates the agency
    /// </summary>
    /// <param name="agency">Agency</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateAgencyAsync(Agency agency);

    /// <summary>
    /// Delete a agency
    /// </summary>
    /// <param name="agency">Agency</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteAgencyAsync(Agency agency);

    #endregion
}

