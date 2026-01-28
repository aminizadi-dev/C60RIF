using Nop.Core.Domain.Customers;
using Nop.Web.Areas.Admin.Models.Customers;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the AntiX model factory
/// </summary>
public partial interface IAntiXModelFactory
{
    /// <summary>
    /// Prepare AntiX search model
    /// </summary>
    /// <param name="searchModel">AntiX search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the AntiX search model
    /// </returns>
    Task<AntiXSearchModel> PrepareAntiXSearchModelAsync(AntiXSearchModel searchModel);

    /// <summary>
    /// Prepare paged AntiX list model
    /// </summary>
    /// <param name="searchModel">AntiX search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the AntiX list model
    /// </returns>
    Task<AntiXListModel> PrepareAntiXListModelAsync(AntiXSearchModel searchModel);

    /// <summary>
    /// Prepare AntiX model
    /// </summary>
    /// <param name="model">AntiX model</param>
    /// <param name="antiX">AntiX</param>
    /// <param name="excludeProperties">Whether to exclude properties</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the AntiX model
    /// </returns>
    Task<AntiXModel> PrepareAntiXModelAsync(AntiXModel model, AntiX antiX, bool excludeProperties = false);
}

