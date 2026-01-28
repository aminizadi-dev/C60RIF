using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Data;

namespace Nop.Services.Customers;

/// <summary>
/// AntiX service
/// </summary>
public partial class AntiXService : IAntiXService
{
    #region Fields

    protected readonly IEventPublisher _eventPublisher;
    protected readonly IRepository<AntiX> _antiXRepository;

    #endregion

    #region Ctor

    public AntiXService(
        IEventPublisher eventPublisher,
        IRepository<AntiX> antiXRepository)
    {
        _eventPublisher = eventPublisher;
        _antiXRepository = antiXRepository;
    }

    #endregion

    #region Methods

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
    public virtual async Task<IPagedList<AntiX>> GetAllAntiXAsync(string name = null, bool? published = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        var items = await _antiXRepository.GetAllPagedAsync(query =>
        {
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));
            if (published.HasValue)
                query = query.Where(x => x.Published == published.Value);

            query = query.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name);

            return query;
        }, pageIndex, pageSize, getOnlyTotalCount);

        return items;
    }

    /// <summary>
    /// Gets an AntiX item
    /// </summary>
    /// <param name="antiXId">AntiX identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the AntiX item
    /// </returns>
    public virtual async Task<AntiX> GetAntiXByIdAsync(int antiXId)
    {
        return await _antiXRepository.GetByIdAsync(antiXId);
    }

    /// <summary>
    /// Gets AntiX items by identifiers
    /// </summary>
    /// <param name="antiXIds">AntiX identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the AntiX items
    /// </returns>
    public virtual async Task<IList<AntiX>> GetAntiXByIdsAsync(int[] antiXIds)
    {
        if (antiXIds == null || antiXIds.Length == 0)
            return new List<AntiX>();

        var query = await _antiXRepository.GetAllAsync(query =>
        {
            query = query.Where(x => antiXIds.Contains(x.Id));
            return query;
        });

        return query.ToList();
    }

    /// <summary>
    /// Inserts an AntiX item
    /// </summary>
    /// <param name="antiX">AntiX item</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertAntiXAsync(AntiX antiX)
    {
        ArgumentNullException.ThrowIfNull(antiX);

        await _antiXRepository.InsertAsync(antiX);
        await _eventPublisher.EntityInsertedAsync(antiX);
    }

    /// <summary>
    /// Updates the AntiX item
    /// </summary>
    /// <param name="antiX">AntiX item</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateAntiXAsync(AntiX antiX)
    {
        ArgumentNullException.ThrowIfNull(antiX);

        await _antiXRepository.UpdateAsync(antiX);
        await _eventPublisher.EntityUpdatedAsync(antiX);
    }

    /// <summary>
    /// Delete an AntiX item
    /// </summary>
    /// <param name="antiX">AntiX item</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteAntiXAsync(AntiX antiX)
    {
        ArgumentNullException.ThrowIfNull(antiX);

        await _antiXRepository.DeleteAsync(antiX);
        await _eventPublisher.EntityDeletedAsync(antiX);
    }

    #endregion
}

