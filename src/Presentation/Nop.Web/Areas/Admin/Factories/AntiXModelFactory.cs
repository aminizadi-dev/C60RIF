using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the AntiX model factory implementation
/// </summary>
public partial class AntiXModelFactory : IAntiXModelFactory
{
    #region Fields

    protected readonly IAntiXService _antiXService;

    #endregion

    #region Ctor

    public AntiXModelFactory(IAntiXService antiXService)
    {
        _antiXService = antiXService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare AntiX search model
    /// </summary>
    /// <param name="searchModel">AntiX search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the AntiX search model
    /// </returns>
    public virtual Task<AntiXSearchModel> PrepareAntiXSearchModelAsync(AntiXSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    /// <summary>
    /// Prepare paged AntiX list model
    /// </summary>
    /// <param name="searchModel">AntiX search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the AntiX list model
    /// </returns>
    public virtual async Task<AntiXListModel> PrepareAntiXListModelAsync(AntiXSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var items = await _antiXService.GetAllAntiXAsync(
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var model = new AntiXListModel().PrepareToGrid(searchModel, items, () =>
        {
            return items.Select(item => item.ToModel<AntiXModel>());
        });

        return model;
    }

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
    public virtual Task<AntiXModel> PrepareAntiXModelAsync(AntiXModel model, AntiX antiX, bool excludeProperties = false)
    {
        if (antiX != null)
        {
            model ??= antiX.ToModel<AntiXModel>();
        }

        if (antiX == null)
        {
            model ??= new AntiXModel();
            model.Published = true;
        }

        return Task.FromResult(model);
    }

    #endregion
}

