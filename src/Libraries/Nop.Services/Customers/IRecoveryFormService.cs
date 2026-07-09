using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers;

/// <summary>
/// Recovery form service interface
/// </summary>
public partial interface IRecoveryFormService
{
    #region Recovery forms

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
    Task<IPagedList<RecoveryForm>> GetAllRecoveryFormsAsync(string recoveryNo = null,
        string personName = null, int cityId = 0, int agencyId = 0, int clinicId = 0, int antiXId = 0,
        string guideNameAndLegionNo = null, string cardNo = null,
        DateTime? travelStartDateUtc = null, DateTime? travelEndDateUtc = null,
        int? recoveryYear = null, int? recoveryMonth = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

    /// <summary>
    /// Gets a recovery form
    /// </summary>
    /// <param name="recoveryFormId">Recovery form identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery form
    /// </returns>
    Task<RecoveryForm> GetRecoveryFormByIdAsync(int recoveryFormId);

    /// <summary>
    /// Gets recovery forms by identifiers
    /// </summary>
    /// <param name="recoveryFormIds">Recovery form identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery forms
    /// </returns>
    Task<IList<RecoveryForm>> GetRecoveryFormsByIdsAsync(int[] recoveryFormIds);

    /// <summary>
    /// Gets the recovery form linked to a person
    /// </summary>
    /// <param name="personId">Person identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery form (or null)
    /// </returns>
    Task<RecoveryForm> GetRecoveryFormByPersonIdAsync(int personId);

    /// <summary>
    /// Inserts a recovery form
    /// </summary>
    /// <param name="recoveryForm">Recovery form</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertRecoveryFormAsync(RecoveryForm recoveryForm);

    /// <summary>
    /// Updates the recovery form
    /// </summary>
    /// <param name="recoveryForm">Recovery form</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateRecoveryFormAsync(RecoveryForm recoveryForm);

    /// <summary>
    /// Delete a recovery form
    /// </summary>
    /// <param name="recoveryForm">Recovery form</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteRecoveryFormAsync(RecoveryForm recoveryForm);

    /// <summary>
    /// Gets available recovery years (distinct Persian years from TravelEndDateUtc)
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of distinct Persian years (descending)
    /// </returns>
    Task<IList<int>> GetAvailableRecoveryYearsAsync();

    /// <summary>
    /// Normalizes recovery number (e.g. prepends 05 for Persian year 1405)
    /// </summary>
    /// <param name="recoveryNo">Recovery number</param>
    /// <param name="travelEndDateUtc">Travel end date (UTC)</param>
    /// <returns>Normalized recovery number</returns>
    string NormalizeRecoveryNo(string recoveryNo, DateTime? travelEndDateUtc);

    /// <summary>
    /// Checks whether a recovery number already exists
    /// </summary>
    /// <param name="recoveryNo">Recovery number</param>
    /// <param name="exceptRecoveryFormId">Exclude recovery form identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<bool> IsRecoveryNoExistsAsync(string recoveryNo, int? exceptRecoveryFormId = null);

    #endregion
}
