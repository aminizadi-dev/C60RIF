using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Data;

namespace Nop.Services.Customers;

/// <summary>
/// Clinic service
/// </summary>
public partial class ClinicService : IClinicService
{
    #region Fields

    protected readonly IEventPublisher _eventPublisher;
    protected readonly IRepository<Clinic> _clinicRepository;

    #endregion

    #region Ctor

    public ClinicService(
        IEventPublisher eventPublisher,
        IRepository<Clinic> clinicRepository)
    {
        _eventPublisher = eventPublisher;
        _clinicRepository = clinicRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets all clinics
    /// </summary>
    /// <param name="cityId">City identifier; 0 to load all clinics</param>
    /// <param name="name">Clinic name; null to load all clinics</param>
    /// <param name="published">Published status; null to load all clinics</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">A value indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinics
    /// </returns>
    public virtual async Task<IPagedList<Clinic>> GetAllClinicsAsync(int cityId = 0, string name = null, bool? published = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        var clinics = await _clinicRepository.GetAllPagedAsync(query =>
        {
            if (cityId > 0)
                query = query.Where(c => c.CityId == cityId);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name));
            if (published.HasValue)
                query = query.Where(c => c.Published == published.Value);

            query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);

            return query;
        }, pageIndex, pageSize, getOnlyTotalCount);

        return clinics;
    }

    /// <summary>
    /// Gets a clinic
    /// </summary>
    /// <param name="clinicId">Clinic identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinic
    /// </returns>
    public virtual async Task<Clinic> GetClinicByIdAsync(int clinicId)
    {
        return await _clinicRepository.GetByIdAsync(clinicId);
    }

    /// <summary>
    /// Gets clinics by identifiers
    /// </summary>
    /// <param name="clinicIds">Clinic identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinics
    /// </returns>
    public virtual async Task<IList<Clinic>> GetClinicsByIdsAsync(int[] clinicIds)
    {
        if (clinicIds == null || clinicIds.Length == 0)
            return new List<Clinic>();

        var query = await _clinicRepository.GetAllAsync(query =>
        {
            query = query.Where(c => clinicIds.Contains(c.Id));
            return query;
        });

        return query.ToList();
    }

    /// <summary>
    /// Gets clinics by city identifier
    /// </summary>
    /// <param name="cityId">City identifier</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinics
    /// </returns>
    public virtual async Task<IList<Clinic>> GetClinicsByCityIdAsync(int cityId, bool showHidden = false)
    {
        var query = await _clinicRepository.GetAllAsync(query =>
        {
            query = query.Where(c => c.CityId == cityId);
            if (!showHidden)
                query = query.Where(c => c.Published);
            query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);
            return query;
        });

        return query.ToList();
    }

    /// <summary>
    /// Inserts a clinic
    /// </summary>
    /// <param name="clinic">Clinic</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertClinicAsync(Clinic clinic)
    {
        ArgumentNullException.ThrowIfNull(clinic);

        await _clinicRepository.InsertAsync(clinic);
        await _eventPublisher.EntityInsertedAsync(clinic);
    }

    /// <summary>
    /// Updates the clinic
    /// </summary>
    /// <param name="clinic">Clinic</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateClinicAsync(Clinic clinic)
    {
        ArgumentNullException.ThrowIfNull(clinic);

        await _clinicRepository.UpdateAsync(clinic);
        await _eventPublisher.EntityUpdatedAsync(clinic);
    }

    /// <summary>
    /// Delete a clinic
    /// </summary>
    /// <param name="clinic">Clinic</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteClinicAsync(Clinic clinic)
    {
        ArgumentNullException.ThrowIfNull(clinic);

        await _clinicRepository.DeleteAsync(clinic);
        await _eventPublisher.EntityDeletedAsync(clinic);
    }

    #endregion
}

