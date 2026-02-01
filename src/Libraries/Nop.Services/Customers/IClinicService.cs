using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers;

/// <summary>
/// Clinic service interface
/// </summary>
public partial interface IClinicService
{
    #region Clinics

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
    Task<IPagedList<Clinic>> GetAllClinicsAsync(int cityId = 0, string name = null, bool? published = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

    /// <summary>
    /// Gets a clinic
    /// </summary>
    /// <param name="clinicId">Clinic identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinic
    /// </returns>
    Task<Clinic> GetClinicByIdAsync(int clinicId);

    /// <summary>
    /// Gets clinics by identifiers
    /// </summary>
    /// <param name="clinicIds">Clinic identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinics
    /// </returns>
    Task<IList<Clinic>> GetClinicsByIdsAsync(int[] clinicIds);

    /// <summary>
    /// Gets clinics by city identifier
    /// </summary>
    /// <param name="cityId">City identifier</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinics
    /// </returns>
    Task<IList<Clinic>> GetClinicsByCityIdAsync(int cityId, bool showHidden = false);

    /// <summary>
    /// Inserts a clinic
    /// </summary>
    /// <param name="clinic">Clinic</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertClinicAsync(Clinic clinic);

    /// <summary>
    /// Updates the clinic
    /// </summary>
    /// <param name="clinic">Clinic</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateClinicAsync(Clinic clinic);

    /// <summary>
    /// Delete a clinic
    /// </summary>
    /// <param name="clinic">Clinic</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteClinicAsync(Clinic clinic);

    #endregion
}

