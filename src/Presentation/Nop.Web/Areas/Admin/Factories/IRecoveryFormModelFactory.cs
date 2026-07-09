using Nop.Core.Domain.Customers;
using Nop.Web.Areas.Admin.Models.Customers;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the recovery form model factory
/// </summary>
public partial interface IRecoveryFormModelFactory
{
    /// <summary>
    /// Prepare recovery form search model
    /// </summary>
    /// <param name="searchModel">Recovery form search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery form search model
    /// </returns>
    Task<RecoveryFormSearchModel> PrepareRecoveryFormSearchModelAsync(RecoveryFormSearchModel searchModel);

    /// <summary>
    /// Prepare paged recovery form list model
    /// </summary>
    /// <param name="searchModel">Recovery form search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery form list model
    /// </returns>
    Task<RecoveryFormListModel> PrepareRecoveryFormListModelAsync(RecoveryFormSearchModel searchModel);

    /// <summary>
    /// Prepare recovery form model
    /// </summary>
    /// <param name="model">Recovery form model</param>
    /// <param name="recoveryForm">Recovery form</param>
    /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the recovery form model
    /// </returns>
    Task<RecoveryFormModel> PrepareRecoveryFormModelAsync(RecoveryFormModel model, RecoveryForm recoveryForm, bool excludeProperties = false);
}
