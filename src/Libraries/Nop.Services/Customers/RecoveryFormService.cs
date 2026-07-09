using System;
using System.Globalization;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Data;

namespace Nop.Services.Customers;

/// <summary>
/// Recovery form service
/// </summary>
public partial class RecoveryFormService : IRecoveryFormService
{
    #region Fields

    protected readonly IEventPublisher _eventPublisher;
    protected readonly IRepository<Agency> _agencyRepository;
    protected readonly IRepository<RecoveryForm> _recoveryFormRepository;
    protected readonly IRepository<Person> _personRepository;

    #endregion

    #region Ctor

    public RecoveryFormService(
        IEventPublisher eventPublisher,
        IRepository<Agency> agencyRepository,
        IRepository<RecoveryForm> recoveryFormRepository,
        IRepository<Person> personRepository)
    {
        _eventPublisher = eventPublisher;
        _agencyRepository = agencyRepository;
        _recoveryFormRepository = recoveryFormRepository;
        _personRepository = personRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets all recovery forms
    /// </summary>
    /// <param name="recoveryNo">Recovery number; null or empty to load all recovery forms</param>
    /// <param name="personName">Person name; null to load all recovery forms</param>
    /// <param name="cityId">City identifier; 0 to load all recovery forms</param>
    /// <param name="agencyId">Agency identifier; 0 to load all recovery forms</param>
    /// <param name="antiXId">AntiX identifier; 0 to load all recovery forms</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">A value indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery forms
    /// </returns>
    public virtual async Task<IPagedList<RecoveryForm>> GetAllRecoveryFormsAsync(string recoveryNo = null,
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

        var recoveryForms = await _recoveryFormRepository.GetAllPagedAsync(query =>
        {
            if (!string.IsNullOrWhiteSpace(recoveryNo))
                query = query.Where(p => p.RecoveryNo == recoveryNo);
            if (!string.IsNullOrWhiteSpace(personName))
            {
                query = from recoveryForm in query
                        join person in _personRepository.Table on recoveryForm.PersonId equals person.Id
                        where (person.FirstName != null && person.FirstName.Contains(personName)) ||
                              (person.LastName != null && person.LastName.Contains(personName))
                        select recoveryForm;
            }
            if (!string.IsNullOrWhiteSpace(guideNameAndLegionNo))
                query = query.Where(p => p.GuideNameAndLegionNo.Contains(guideNameAndLegionNo));
            if (!string.IsNullOrWhiteSpace(cardNo))
            {
                query = from recoveryForm in query
                        join person in _personRepository.Table on recoveryForm.PersonId equals person.Id
                        where person.CardNo == cardNo
                        select recoveryForm;
            }
            if (agencyId > 0)
                query = query.Where(p => p.AgencyId == agencyId);
            if (clinicId > 0)
                query = query.Where(p => p.ClinicId.HasValue && p.ClinicId.Value == clinicId);
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
                query = from recoveryForm in query
                        join agency in _agencyRepository.Table on recoveryForm.AgencyId equals agency.Id
                        where agency.CityId == cityId
                        select recoveryForm;
            }

            query = query
                .OrderByDescending(p => p.CreatedOnUtc)
                .ThenByDescending(p => p.RecoveryNo);

            return query;
        }, pageIndex, pageSize, getOnlyTotalCount);

        return recoveryForms;
    }

    /// <summary>
    /// Gets a recovery form
    /// </summary>
    /// <param name="recoveryFormId">Recovery form identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery form
    /// </returns>
    public virtual async Task<RecoveryForm> GetRecoveryFormByIdAsync(int recoveryFormId)
    {
        return await _recoveryFormRepository.GetByIdAsync(recoveryFormId);
    }

    /// <summary>
    /// Gets recovery forms by identifiers
    /// </summary>
    /// <param name="recoveryFormIds">Recovery form identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery forms
    /// </returns>
    public virtual async Task<IList<RecoveryForm>> GetRecoveryFormsByIdsAsync(int[] recoveryFormIds)
    {
        if (recoveryFormIds == null || recoveryFormIds.Length == 0)
            return new List<RecoveryForm>();

        var query = await _recoveryFormRepository.GetAllAsync(query =>
        {
            query = query.Where(p => recoveryFormIds.Contains(p.Id));
            return query;
        });

        return query.ToList();
    }

    /// <summary>
    /// Gets the recovery form linked to a person
    /// </summary>
    /// <param name="personId">Person identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery form (or null)
    /// </returns>
    public virtual async Task<RecoveryForm> GetRecoveryFormByPersonIdAsync(int personId)
    {
        if (personId <= 0)
            return null;

        return await _recoveryFormRepository.Table
            .Where(p => p.PersonId == personId)
            .OrderByDescending(p => p.CreatedOnUtc)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Inserts a recovery form
    /// </summary>
    /// <param name="recoveryForm">Recovery form</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertRecoveryFormAsync(RecoveryForm recoveryForm)
    {
        ArgumentNullException.ThrowIfNull(recoveryForm);

        NormalizeRecoveryNumber(recoveryForm);

        await _recoveryFormRepository.InsertAsync(recoveryForm);
    }

    /// <summary>
    /// Updates the recovery form
    /// </summary>
    /// <param name="recoveryForm">Recovery form</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateRecoveryFormAsync(RecoveryForm recoveryForm)
    {
        ArgumentNullException.ThrowIfNull(recoveryForm);

        NormalizeRecoveryNumber(recoveryForm);

        await _recoveryFormRepository.UpdateAsync(recoveryForm);
    }

    /// <summary>
    /// Delete a recovery form
    /// </summary>
    /// <param name="recoveryForm">Recovery form</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteRecoveryFormAsync(RecoveryForm recoveryForm)
    {
        ArgumentNullException.ThrowIfNull(recoveryForm);

        await _recoveryFormRepository.DeleteAsync(recoveryForm);
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

        var recoveryForms = await _recoveryFormRepository.GetAllAsync(query =>
            query.Where(p => p.TravelEndDateUtc.HasValue));

        var years = recoveryForms
            .Select(p => pc.GetYear(p.TravelEndDateUtc!.Value))
            .Distinct()
            .OrderByDescending(y => y)
            .ToList();

        return years;
    }

    /// <summary>
    /// Normalizes recovery number (e.g. prepends 05 for Persian year 1405)
    /// </summary>
    /// <param name="recoveryNo">Recovery number</param>
    /// <param name="travelEndDateUtc">Travel end date (UTC)</param>
    /// <returns>Normalized recovery number</returns>
    public virtual string NormalizeRecoveryNo(string recoveryNo, DateTime? travelEndDateUtc)
    {
        return RecoveryNoHelper.Normalize(recoveryNo, travelEndDateUtc);
    }

    protected virtual void NormalizeRecoveryNumber(RecoveryForm recoveryForm)
    {
        recoveryForm.RecoveryNo = NormalizeRecoveryNo(recoveryForm.RecoveryNo, recoveryForm.TravelEndDateUtc);
    }

    /// <summary>
    /// Checks whether a recovery number already exists
    /// </summary>
    /// <param name="recoveryNo">Recovery number</param>
    /// <param name="exceptRecoveryFormId">Exclude recovery form identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<bool> IsRecoveryNoExistsAsync(string recoveryNo, int? exceptRecoveryFormId = null)
    {
        if (string.IsNullOrWhiteSpace(recoveryNo))
            return false;

        var query = _recoveryFormRepository.Table.Where(p => p.RecoveryNo == recoveryNo);

        if (exceptRecoveryFormId.HasValue)
            query = query.Where(p => p.Id != exceptRecoveryFormId.Value);

        return await query.AnyAsync();
    }

    #endregion
}
