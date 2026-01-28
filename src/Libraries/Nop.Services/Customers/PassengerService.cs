using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Data;

namespace Nop.Services.Customers;

/// <summary>
/// Passenger service
/// </summary>
public partial class PassengerService : IPassengerService
{
    #region Fields

    protected readonly IEventPublisher _eventPublisher;
    protected readonly IRepository<Passenger> _passengerRepository;

    #endregion

    #region Ctor

    public PassengerService(
        IEventPublisher eventPublisher,
        IRepository<Passenger> passengerRepository)
    {
        _eventPublisher = eventPublisher;
        _passengerRepository = passengerRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets all passengers
    /// </summary>
    /// <param name="recoveryNo">Recovery number; 0 to load all passengers</param>
    /// <param name="personName">Person name; null to load all passengers</param>
    /// <param name="agencyId">Agency identifier; 0 to load all passengers</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">A value indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the passengers
    /// </returns>
    public virtual async Task<IPagedList<Passenger>> GetAllPassengersAsync(int recoveryNo = 0,
        string personName = null, int agencyId = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        var passengers = await _passengerRepository.GetAllPagedAsync(query =>
        {
            if (recoveryNo > 0)
                query = query.Where(p => p.RecoveryNo == recoveryNo);
            if (!string.IsNullOrWhiteSpace(personName))
                query = query.Where(p => p.PersonName.Contains(personName));
            if (agencyId > 0)
                query = query.Where(p => p.AgencyId == agencyId);

            query = query.OrderByDescending(p => p.CreatedOnUtc);

            return query;
        }, pageIndex, pageSize, getOnlyTotalCount);

        return passengers;
    }

    /// <summary>
    /// Gets a passenger
    /// </summary>
    /// <param name="passengerId">Passenger identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the passenger
    /// </returns>
    public virtual async Task<Passenger> GetPassengerByIdAsync(int passengerId)
    {
        return await _passengerRepository.GetByIdAsync(passengerId);
    }

    /// <summary>
    /// Gets passengers by identifiers
    /// </summary>
    /// <param name="passengerIds">Passenger identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the passengers
    /// </returns>
    public virtual async Task<IList<Passenger>> GetPassengersByIdsAsync(int[] passengerIds)
    {
        if (passengerIds == null || passengerIds.Length == 0)
            return new List<Passenger>();

        var query = await _passengerRepository.GetAllAsync(query =>
        {
            query = query.Where(p => passengerIds.Contains(p.Id));
            return query;
        });

        return query.ToList();
    }

    /// <summary>
    /// Inserts a passenger
    /// </summary>
    /// <param name="passenger">Passenger</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertPassengerAsync(Passenger passenger)
    {
        ArgumentNullException.ThrowIfNull(passenger);

        await _passengerRepository.InsertAsync(passenger);
    }

    /// <summary>
    /// Updates the passenger
    /// </summary>
    /// <param name="passenger">Passenger</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdatePassengerAsync(Passenger passenger)
    {
        ArgumentNullException.ThrowIfNull(passenger);

        await _passengerRepository.UpdateAsync(passenger);
    }

    /// <summary>
    /// Delete a passenger
    /// </summary>
    /// <param name="passenger">Passenger</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeletePassengerAsync(Passenger passenger)
    {
        ArgumentNullException.ThrowIfNull(passenger);

        await _passengerRepository.DeleteAsync(passenger);
    }

    #endregion
}

