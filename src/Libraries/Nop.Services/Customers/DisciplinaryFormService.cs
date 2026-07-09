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
    protected readonly IRepository<RecoveryForm> _recoveryFormRepository;
    protected readonly IRepository<Person> _personRepository;

    #endregion

    #region Ctor

    public DisciplinaryFormService(
        IEventPublisher eventPublisher,
        IRepository<AntiX> antiXRepository,
        IRepository<DisciplinaryForm> disciplinaryFormRepository,
        IRepository<RecoveryForm> recoveryFormRepository,
        IRepository<Person> personRepository)
    {
        _eventPublisher = eventPublisher;
        _antiXRepository = antiXRepository;
        _disciplinaryFormRepository = disciplinaryFormRepository;
        _recoveryFormRepository = recoveryFormRepository;
        _personRepository = personRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets all disciplinary forms
    /// </summary>
    public virtual async Task<IPagedList<DisciplinaryForm>> GetAllDisciplinaryFormsAsync(string personName = null,
        string familyName = null, string cardNo = null, int personId = 0, string agencyName = null,
        DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        var forms = await _disciplinaryFormRepository.GetAllPagedAsync(query =>
        {
            if (!string.IsNullOrWhiteSpace(personName))
            {
                query = from form in query
                        join person in _personRepository.Table on form.PersonId equals person.Id
                        where person.FirstName != null && person.FirstName.Contains(personName)
                        select form;
            }
            if (!string.IsNullOrWhiteSpace(familyName))
            {
                query = from form in query
                        join person in _personRepository.Table on form.PersonId equals person.Id
                        where person.LastName != null && person.LastName.Contains(familyName)
                        select form;
            }
            if (!string.IsNullOrWhiteSpace(cardNo))
            {
                query = from form in query
                        join person in _personRepository.Table on form.PersonId equals person.Id
                        where person.CardNo == cardNo
                        select form;
            }
            if (personId > 0)
                query = query.Where(f => f.PersonId == personId);
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
    /// Gets disciplinary forms linked to a person
    /// </summary>
    public virtual async Task<IList<DisciplinaryForm>> GetDisciplinaryFormsByPersonIdAsync(int personId)
    {
        if (personId <= 0)
            return new List<DisciplinaryForm>();

        return await _disciplinaryFormRepository.Table
            .Where(f => f.PersonId == personId)
            .OrderByDescending(f => f.CreatedOnUtc)
            .ToListAsync();
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

    #endregion

    #region Utilities

    protected virtual async Task PrepareDisciplinaryFormAsync(DisciplinaryForm disciplinaryForm, bool isNew)
    {
        NormalizeDisciplinaryFormIdentifiers(disciplinaryForm);

        if (isNew && disciplinaryForm.CreatedOnUtc == default)
            disciplinaryForm.CreatedOnUtc = DateTime.UtcNow;

        if (disciplinaryForm.PersonId <= 0)
            return;

        //prefill agency and substance details from the person's recovery form (if any)
        var recoveryForm = await _recoveryFormRepository.Table
            .Where(p => p.PersonId == disciplinaryForm.PersonId)
            .OrderByDescending(p => p.CreatedOnUtc)
            .FirstOrDefaultAsync();

        if (recoveryForm == null)
            return;

        if (!disciplinaryForm.AgencyId.HasValue || disciplinaryForm.AgencyId.Value <= 0)
            disciplinaryForm.AgencyId = recoveryForm.AgencyId;

        if (string.IsNullOrWhiteSpace(disciplinaryForm.PreviousSubstanceUseDetails))
        {
            var antiX = await _antiXRepository.GetByIdAsync(recoveryForm.AntiX1);
            if (antiX != null)
                disciplinaryForm.PreviousSubstanceUseDetails = antiX.Name;
        }

        if (string.IsNullOrWhiteSpace(disciplinaryForm.CurrentSubstanceUseDetails) && recoveryForm.AntiX2.HasValue)
        {
            var antiX = await _antiXRepository.GetByIdAsync(recoveryForm.AntiX2.Value);
            if (antiX != null)
                disciplinaryForm.CurrentSubstanceUseDetails = antiX.Name;
        }
    }

    protected virtual void NormalizeDisciplinaryFormIdentifiers(DisciplinaryForm disciplinaryForm)
    {
        if (disciplinaryForm.AgencyId.GetValueOrDefault() <= 0)
            disciplinaryForm.AgencyId = null;
        disciplinaryForm.AgencyName = disciplinaryForm.AgencyName?.Trim();
        disciplinaryForm.LegionNo = disciplinaryForm.LegionNo?.Trim();
        disciplinaryForm.CurrentSubstanceUseDetails = disciplinaryForm.CurrentSubstanceUseDetails?.Trim();
    }

    #endregion
}
