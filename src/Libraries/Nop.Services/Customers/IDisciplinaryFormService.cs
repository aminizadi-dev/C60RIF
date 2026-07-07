using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers;

/// <summary>
/// Disciplinary form service interface
/// </summary>
public partial interface IDisciplinaryFormService
{
    #region Disciplinary forms

    /// <summary>
    /// Gets all disciplinary forms
    /// </summary>
    /// <param name="personName">Person name; null to load all forms</param>
    /// <param name="familyName">Family name; null to load all forms</param>
    /// <param name="cardNo">Card number; null to load all forms</param>
    /// <param name="passengerId">Passenger identifier; 0 to load all forms</param>
    /// <param name="agencyName">Agency name; null to load all forms</param>
    /// <param name="createdFromUtc">Created from date (UTC); null to ignore</param>
    /// <param name="createdToUtc">Created to date (UTC); null to ignore</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">A value indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the disciplinary forms
    /// </returns>
    Task<IPagedList<DisciplinaryForm>> GetAllDisciplinaryFormsAsync(string personName = null,
        string familyName = null, string cardNo = null, int passengerId = 0, string agencyName = null,
        DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

    /// <summary>
    /// Gets a disciplinary form
    /// </summary>
    /// <param name="disciplinaryFormId">Disciplinary form identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the disciplinary form
    /// </returns>
    Task<DisciplinaryForm> GetDisciplinaryFormByIdAsync(int disciplinaryFormId);

    /// <summary>
    /// Gets disciplinary forms by identifiers
    /// </summary>
    /// <param name="disciplinaryFormIds">Disciplinary form identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the disciplinary forms
    /// </returns>
    Task<IList<DisciplinaryForm>> GetDisciplinaryFormsByIdsAsync(int[] disciplinaryFormIds);

    /// <summary>
    /// Gets a disciplinary form by passenger identifier
    /// </summary>
    /// <param name="passengerId">Passenger identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the disciplinary form
    /// </returns>
    Task<DisciplinaryForm> GetDisciplinaryFormByPassengerIdAsync(int passengerId);

    /// <summary>
    /// Inserts a disciplinary form
    /// </summary>
    /// <param name="disciplinaryForm">Disciplinary form</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertDisciplinaryFormAsync(DisciplinaryForm disciplinaryForm);

    /// <summary>
    /// Updates the disciplinary form
    /// </summary>
    /// <param name="disciplinaryForm">Disciplinary form</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateDisciplinaryFormAsync(DisciplinaryForm disciplinaryForm);

    /// <summary>
    /// Delete a disciplinary form
    /// </summary>
    /// <param name="disciplinaryForm">Disciplinary form</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteDisciplinaryFormAsync(DisciplinaryForm disciplinaryForm);

    /// <summary>
    /// Checks whether a passenger is already linked to a disciplinary form
    /// </summary>
    /// <param name="passengerId">Passenger identifier</param>
    /// <param name="exceptFormId">Exclude disciplinary form identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<bool> IsPassengerLinkedAsync(int passengerId, int? exceptFormId = null);

    #endregion
}
