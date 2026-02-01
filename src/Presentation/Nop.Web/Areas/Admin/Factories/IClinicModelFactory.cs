using Nop.Core.Domain.Customers;
using Nop.Web.Areas.Admin.Models.Customers;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the clinic model factory
/// </summary>
public partial interface IClinicModelFactory
{
    /// <summary>
    /// Prepare clinic search model
    /// </summary>
    /// <param name="searchModel">Clinic search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinic search model
    /// </returns>
    Task<ClinicSearchModel> PrepareClinicSearchModelAsync(ClinicSearchModel searchModel);

    /// <summary>
    /// Prepare paged clinic list model
    /// </summary>
    /// <param name="searchModel">Clinic search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinic list model
    /// </returns>
    Task<ClinicListModel> PrepareClinicListModelAsync(ClinicSearchModel searchModel);

    /// <summary>
    /// Prepare clinic model
    /// </summary>
    /// <param name="model">Clinic model</param>
    /// <param name="clinic">Clinic</param>
    /// <param name="excludeProperties">Whether to exclude properties</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the clinic model
    /// </returns>
    Task<ClinicModel> PrepareClinicModelAsync(ClinicModel model, Clinic clinic, bool excludeProperties = false);
}

