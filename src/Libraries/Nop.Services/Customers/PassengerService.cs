using System;
using System.Globalization;
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
    protected readonly IRepository<Agency> _agencyRepository;
    protected readonly IRepository<Passenger> _passengerRepository;

    #endregion

    #region Ctor

    public PassengerService(
        IEventPublisher eventPublisher,
        IRepository<Agency> agencyRepository,
        IRepository<Passenger> passengerRepository)
    {
        _eventPublisher = eventPublisher;
        _agencyRepository = agencyRepository;
        _passengerRepository = passengerRepository;
    }

    #endregion

    #region Methods

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
    public virtual async Task<IPagedList<Passenger>> GetAllPassengersAsync(int recoveryNo = 0,
        string personName = null, int cityId = 0, int agencyId = 0, int clinicId = 0, int antiXId = 0,
        string guideNameAndLegionNo = null, string cardNo = null,
        DateTime? travelStartDateUtc = null, DateTime? travelEndDateUtc = null,
        int? recoveryYear = null, int? recoveryMonth = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        //convert Persian recovery year/month to Gregorian date range for TravelEndDateUtc filtering
        DateTime? recoveryRangeStart = null;
        DateTime? recoveryRangeEnd = null;
        if (recoveryYear.HasValue && recoveryYear.Value > 0)
        {
            var pc = new PersianCalendar();
            if (recoveryMonth.HasValue && recoveryMonth.Value >= 1 && recoveryMonth.Value <= 12)
            {
                //specific month: first day to first day of next month
                recoveryRangeStart = pc.ToDateTime(recoveryYear.Value, recoveryMonth.Value, 1, 0, 0, 0, 0);
                if (recoveryMonth.Value < 12)
                    recoveryRangeEnd = pc.ToDateTime(recoveryYear.Value, recoveryMonth.Value + 1, 1, 0, 0, 0, 0);
                else
                    recoveryRangeEnd = pc.ToDateTime(recoveryYear.Value + 1, 1, 1, 0, 0, 0, 0);
            }
            else
            {
                //whole year: first day of Farvardin to first day of next Farvardin
                recoveryRangeStart = pc.ToDateTime(recoveryYear.Value, 1, 1, 0, 0, 0, 0);
                recoveryRangeEnd = pc.ToDateTime(recoveryYear.Value + 1, 1, 1, 0, 0, 0, 0);
            }
        }

        var passengers = await _passengerRepository.GetAllPagedAsync(query =>
        {
            if (recoveryNo > 0)
                query = query.Where(p => p.RecoveryNo == recoveryNo);
            if (!string.IsNullOrWhiteSpace(personName))
                query = query.Where(p => p.PersonName.Contains(personName));
            if (!string.IsNullOrWhiteSpace(guideNameAndLegionNo))
                query = query.Where(p => p.GuideNameAndLegionNo.Contains(guideNameAndLegionNo));
            if (!string.IsNullOrWhiteSpace(cardNo))
                query = query.Where(p => p.CardNo == cardNo);
            if (agencyId > 0)
                query = query.Where(p => p.AgencyId == agencyId);
            if (clinicId > 0)
                query = query.Where(p => p.ClinicId == clinicId);
            if (antiXId > 0)
                query = query.Where(p => p.AntiX1 == antiXId || p.AntiX2 == antiXId);
            if (travelStartDateUtc.HasValue)
                query = query.Where(p => p.TravelStartDateUtc.HasValue && p.TravelStartDateUtc.Value >= travelStartDateUtc.Value);
            if (travelEndDateUtc.HasValue)
                query = query.Where(p => p.TravelEndDateUtc.HasValue && p.TravelEndDateUtc.Value <= travelEndDateUtc.Value);
            if (recoveryRangeStart.HasValue && recoveryRangeEnd.HasValue)
                query = query.Where(p => p.TravelEndDateUtc.HasValue &&
                    p.TravelEndDateUtc.Value >= recoveryRangeStart.Value &&
                    p.TravelEndDateUtc.Value < recoveryRangeEnd.Value);
            if (cityId > 0)
            {
                query = from passenger in query
                        join agency in _agencyRepository.Table on passenger.AgencyId equals agency.Id
                        where agency.CityId == cityId
                        select passenger;
            }

            query = query
                .OrderByDescending(p => p.RecoveryNo);

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

    /// <summary>
    /// Gets available recovery years (distinct Persian years from TravelEndDateUtc)
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of distinct Persian years (descending)
    /// </returns>
    public virtual async Task<IList<int>> GetAvailableRecoveryYearsAsync()
    {
        var pc = new PersianCalendar();

        var passengers = await _passengerRepository.GetAllAsync(query =>
            query.Where(p => p.TravelEndDateUtc.HasValue));

        var years = passengers
            .Select(p => pc.GetYear(p.TravelEndDateUtc!.Value))
            .Distinct()
            .OrderByDescending(y => y)
            .ToList();

        return years;
    }

    /// <summary>
    /// Checks whether a recovery number already exists
    /// </summary>
    /// <param name="recoveryNo">Recovery number</param>
    /// <param name="exceptPassengerId">Exclude passenger identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<bool> IsRecoveryNoExistsAsync(int recoveryNo, int? exceptPassengerId = null)
    {
        if (recoveryNo <= 0)
            return false;

        var query = _passengerRepository.Table.Where(p => p.RecoveryNo == recoveryNo);

        if (exceptPassengerId.HasValue)
            query = query.Where(p => p.Id != exceptPassengerId.Value);

        return await query.AnyAsync();
    }

    /// <summary>
    /// Checks whether a card number already exists
    /// </summary>
    /// <param name="cardNo">Card number</param>
    /// <param name="exceptPassengerId">Exclude passenger identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<bool> IsCardNoExistsAsync(string cardNo, int? exceptPassengerId = null)
    {
        if (string.IsNullOrWhiteSpace(cardNo))
            return false;

        var query = _passengerRepository.Table.Where(p => p.CardNo == cardNo);

        if (exceptPassengerId.HasValue)
            query = query.Where(p => p.Id != exceptPassengerId.Value);

        return await query.AnyAsync();
    }

    #endregion
}

