using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers;

/// <summary>
/// AntiX service interface
/// </summary>
public partial interface IAntiXService
{
    #region AntiX

    /// <summary>
    /// Gets all AntiX items
    /// </summary>
    /// <param name="name">Name; null to load all items</param>
    /// <param name="published">Published status; null to load all items</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">A value indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the AntiX items
    /// </returns>
    Task<IPagedList<AntiX>> GetAllAntiXAsync(string name = null, bool? published = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

    /// <summary>
    /// Gets an AntiX item
    /// </summary>
    /// <param name="antiXId">AntiX identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the AntiX item
    /// </returns>
    Task<AntiX> GetAntiXByIdAsync(int antiXId);

    /// <summary>
    /// Gets AntiX items by identifiers
    /// </summary>
    /// <param name="antiXIds">AntiX identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the AntiX items
    /// </returns>
    Task<IList<AntiX>> GetAntiXByIdsAsync(int[] antiXIds);

    /// <summary>
    /// Inserts an AntiX item
    /// </summary>
    /// <param name="antiX">AntiX item</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertAntiXAsync(AntiX antiX);

    /// <summary>
    /// Updates the AntiX item
    /// </summary>
    /// <param name="antiX">AntiX item</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateAntiXAsync(AntiX antiX);

    /// <summary>
    /// Delete an AntiX item
    /// </summary>
    /// <param name="antiX">AntiX item</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteAntiXAsync(AntiX antiX);

    #endregion
}

