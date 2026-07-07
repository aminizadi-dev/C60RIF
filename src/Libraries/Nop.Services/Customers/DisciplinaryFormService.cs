using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Data;

namespace Nop.Services.Customers;

/// <summary>
/// Disciplinary form service
/// </summary>
public partial class DisciplinaryFormService : IDisciplinaryFormService
{
    #region Fields

    protected readonly IEventPublisher _eventPublisher;
    protected readonly IRepository<AntiX> _antiXRepository;
    protected readonly IRepository<DisciplinaryForm> _disciplinaryFormRepository;
    protected readonly IRepository<Passenger> _passengerRepository;

    #endregion

    #region Ctor

    public DisciplinaryFormService(
        IEventPublisher eventPublisher,
        IRepository<AntiX> antiXRepository,
        IRepository<DisciplinaryForm> disciplinaryFormRepository,
        IRepository<Passenger> passengerRepository)
    {
        _eventPublisher = eventPublisher;
        _antiXRepository = antiXRepository;
        _disciplinaryFormRepository = disciplinaryFormRepository;
        _passengerRepository = passengerRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets all disciplinary forms
    /// </summary>
    public virtual async Task<IPagedList<DisciplinaryForm>> GetAllDisciplinaryFormsAsync(string personName = null,
        string familyName = null, string cardNo = null, int passengerId = 0, string agencyName = null,
        DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        var forms = await _disciplinaryFormRepository.GetAllPagedAsync(query =>
        {
            if (!string.IsNullOrWhiteSpace(personName))
                query = query.Where(f => f.PersonName.Contains(personName));
            if (!string.IsNullOrWhiteSpace(familyName))
                query = query.Where(f => f.FamilyName.Contains(familyName));
            if (!string.IsNullOrWhiteSpace(cardNo))
                query = query.Where(f => f.CardNo == cardNo);
            if (passengerId > 0)
                query = query.Where(f => f.PassengerId.HasValue && f.PassengerId.Value == passengerId);
            if (!string.IsNullOrWhiteSpace(agencyName))
                query = query.Where(f => f.AgencyName.Contains(agencyName));
            if (createdFromUtc.HasValue)
                query = query.Where(f => f.CreatedOnUtc >= createdFromUtc.Value);
            if (createdToUtc.HasValue)
                query = query.Where(f => f.CreatedOnUtc <= createdToUtc.Value);

            query = query
                .OrderByDescending(f => f.CreatedOnUtc)
                .ThenByDescending(f => f.Id);

            return query;
        }, pageIndex, pageSize, getOnlyTotalCount);

        return forms;
    }

    /// <summary>
    /// Gets a disciplinary form
    /// </summary>
    public virtual async Task<DisciplinaryForm> GetDisciplinaryFormByIdAsync(int disciplinaryFormId)
    {
        return await _disciplinaryFormRepository.GetByIdAsync(disciplinaryFormId);
    }

    /// <summary>
    /// Gets disciplinary forms by identifiers
    /// </summary>
    public virtual async Task<IList<DisciplinaryForm>> GetDisciplinaryFormsByIdsAsync(int[] disciplinaryFormIds)
    {
        if (disciplinaryFormIds == null || disciplinaryFormIds.Length == 0)
            return new List<DisciplinaryForm>();

        var query = await _disciplinaryFormRepository.GetAllAsync(query =>
        {
            query = query.Where(f => disciplinaryFormIds.Contains(f.Id));
            return query;
        });

        return query.ToList();
    }

    /// <summary>
    /// Gets a disciplinary form by passenger identifier
    /// </summary>
    public virtual async Task<DisciplinaryForm> GetDisciplinaryFormByPassengerIdAsync(int passengerId)
    {
        if (passengerId <= 0)
            return null;

        return await _disciplinaryFormRepository.Table
            .Where(f => f.PassengerId == passengerId)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Inserts a disciplinary form
    /// </summary>
    public virtual async Task InsertDisciplinaryFormAsync(DisciplinaryForm disciplinaryForm)
    {
        ArgumentNullException.ThrowIfNull(disciplinaryForm);

        await PrepareDisciplinaryFormAsync(disciplinaryForm, isNew: true);

        await _disciplinaryFormRepository.InsertAsync(disciplinaryForm);
        await _eventPublisher.EntityInsertedAsync(disciplinaryForm);
    }

    /// <summary>
    /// Updates the disciplinary form
    /// </summary>
    public virtual async Task UpdateDisciplinaryFormAsync(DisciplinaryForm disciplinaryForm)
    {
        ArgumentNullException.ThrowIfNull(disciplinaryForm);

        await PrepareDisciplinaryFormAsync(disciplinaryForm, isNew: false);

        await _disciplinaryFormRepository.UpdateAsync(disciplinaryForm);
        await _eventPublisher.EntityUpdatedAsync(disciplinaryForm);
    }

    /// <summary>
    /// Delete a disciplinary form
    /// </summary>
    public virtual async Task DeleteDisciplinaryFormAsync(DisciplinaryForm disciplinaryForm)
    {
        ArgumentNullException.ThrowIfNull(disciplinaryForm);

        await _disciplinaryFormRepository.DeleteAsync(disciplinaryForm);
        await _eventPublisher.EntityDeletedAsync(disciplinaryForm);
    }

    /// <summary>
    /// Checks whether a passenger is already linked to a disciplinary form
    /// </summary>
    public virtual async Task<bool> IsPassengerLinkedAsync(int passengerId, int? exceptFormId = null)
    {
        if (passengerId <= 0)
            return false;

        var query = _disciplinaryFormRepository.Table.Where(f => f.PassengerId == passengerId);

        if (exceptFormId.HasValue && exceptFormId.Value > 0)
            query = query.Where(f => f.Id != exceptFormId.Value);

        return await query.AnyAsync();
    }

    #endregion

    #region Utilities

    protected virtual async Task PrepareDisciplinaryFormAsync(DisciplinaryForm disciplinaryForm, bool isNew)
    {
        NormalizeDisciplinaryFormIdentifiers(disciplinaryForm);

        if (isNew && disciplinaryForm.CreatedOnUtc == default)
            disciplinaryForm.CreatedOnUtc = DateTime.UtcNow;

        if (!disciplinaryForm.PassengerId.HasValue || disciplinaryForm.PassengerId.Value <= 0)
            return;

        var passengerId = disciplinaryForm.PassengerId.Value;

        if (await IsPassengerLinkedAsync(passengerId, isNew ? null : disciplinaryForm.Id))
            throw new NopException($"Passenger with id {passengerId} is already linked to a disciplinary form.");

        var passenger = await _passengerRepository.GetByIdAsync(passengerId)
            ?? throw new NopException($"Passenger with id {passengerId} was not found.");

        if (string.IsNullOrWhiteSpace(disciplinaryForm.PersonName))
            disciplinaryForm.PersonName = passenger.PersonName;

        if (string.IsNullOrWhiteSpace(disciplinaryForm.CardNo))
            disciplinaryForm.CardNo = passenger.CardNo;

        if (!disciplinaryForm.AgencyId.HasValue || disciplinaryForm.AgencyId.Value <= 0)
            disciplinaryForm.AgencyId = passenger.AgencyId;

        if (string.IsNullOrWhiteSpace(disciplinaryForm.PreviousSubstanceUseDetails))
        {
            var antiX = await _antiXRepository.GetByIdAsync(passenger.AntiX1);
            if (antiX != null)
                disciplinaryForm.PreviousSubstanceUseDetails = antiX.Name;
        }

        if (string.IsNullOrWhiteSpace(disciplinaryForm.CurrentSubstanceUseDetails) && passenger.AntiX2.HasValue)
        {
            var antiX = await _antiXRepository.GetByIdAsync(passenger.AntiX2.Value);
            if (antiX != null)
                disciplinaryForm.CurrentSubstanceUseDetails = antiX.Name;
        }
    }

    protected virtual void NormalizeDisciplinaryFormIdentifiers(DisciplinaryForm disciplinaryForm)
    {
        disciplinaryForm.PersonName = disciplinaryForm.PersonName?.Trim();
        disciplinaryForm.CardNo = disciplinaryForm.CardNo?.Trim();
        disciplinaryForm.FamilyName = disciplinaryForm.FamilyName?.Trim();
        if (disciplinaryForm.AgencyId.GetValueOrDefault() <= 0)
            disciplinaryForm.AgencyId = null;
        disciplinaryForm.AgencyName = disciplinaryForm.AgencyName?.Trim();
        disciplinaryForm.LegionNo = disciplinaryForm.LegionNo?.Trim();
        disciplinaryForm.CurrentSubstanceUseDetails = disciplinaryForm.CurrentSubstanceUseDetails?.Trim();
    }

    #endregion
}
