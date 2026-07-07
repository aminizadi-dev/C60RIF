using Nop.Core.Domain.Customers;
using Nop.Web.Areas.Admin.Models.Customers;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the disciplinary form model factory
/// </summary>
public partial interface IDisciplinaryFormModelFactory
{
    Task<DisciplinaryFormSearchModel> PrepareDisciplinaryFormSearchModelAsync(DisciplinaryFormSearchModel searchModel);

    Task<DisciplinaryFormListModel> PrepareDisciplinaryFormListModelAsync(DisciplinaryFormSearchModel searchModel);

    Task<DisciplinaryFormModel> PrepareDisciplinaryFormModelAsync(DisciplinaryFormModel model, DisciplinaryForm disciplinaryForm, bool excludeProperties = false);

    void ApplyModelFlagsToEntity(DisciplinaryFormModel model, DisciplinaryForm entity);

    void ApplyEntityFlagsToModel(DisciplinaryForm entity, DisciplinaryFormModel model);
}
