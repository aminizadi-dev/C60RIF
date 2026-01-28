using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers;

/// <summary>
/// Passenger service interface
/// </summary>
public partial interface IPassengerService
{
    #region Passengers

    /// <summary>
    /// Gets all passengers
    /// </summary>
    /// <param name="recoveryNo">Recovery number; 0 to load all passengers</param>
    /// <param name="personName">Person name; null to load all passengers</param>
    /// <param name="cityId">City identifier; 0 to load all passengers</param>
    /// <param name="agencyId">Agency identifier; 0 to load all passengers</param>
    /// <param name="antiXId">AntiX identifier; 0 to load all passengers</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">A value indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the passengers
    /// </returns>
    Task<IPagedList<Passenger>> GetAllPassengersAsync(int recoveryNo = 0,
        string personName = null, int cityId = 0, int agencyId = 0, int antiXId = 0,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

    /// <summary>
    /// Gets a passenger
    /// </summary>
    /// <param name="passengerId">Passenger identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the passenger
    /// </returns>
    Task<Passenger> GetPassengerByIdAsync(int passengerId);

    /// <summary>
    /// Gets passengers by identifiers
    /// </summary>
    /// <param name="passengerIds">Passenger identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the passengers
    /// </returns>
    Task<IList<Passenger>> GetPassengersByIdsAsync(int[] passengerIds);

    /// <summary>
    /// Inserts a passenger
    /// </summary>
    /// <param name="passenger">Passenger</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertPassengerAsync(Passenger passenger);

    /// <summary>
    /// Updates the passenger
    /// </summary>
    /// <param name="passenger">Passenger</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdatePassengerAsync(Passenger passenger);

    /// <summary>
    /// Delete a passenger
    /// </summary>
    /// <param name="passenger">Passenger</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeletePassengerAsync(Passenger passenger);

    #endregion
}

