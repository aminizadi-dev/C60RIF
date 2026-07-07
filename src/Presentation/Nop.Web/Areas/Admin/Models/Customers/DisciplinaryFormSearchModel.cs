using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a disciplinary form search model
/// </summary>
public partial record DisciplinaryFormSearchModel : BaseSearchModel
{
    [NopResourceDisplayName("Admin.DisciplinaryForms.List.SearchPersonName")]
    public string SearchPersonName { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.List.SearchFamilyName")]
    public string SearchFamilyName { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.List.SearchCardNo")]
    public string SearchCardNo { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.List.SearchAgencyName")]
    public string SearchAgencyName { get; set; }

    [UIHint("PersianDateNullable")]
    [PersianDate]
    [NopResourceDisplayName("Admin.DisciplinaryForms.List.SearchCreatedFrom")]
    public DateTime? SearchCreatedFromUtc { get; set; }

    [UIHint("PersianDateNullable")]
    [PersianDate]
    [NopResourceDisplayName("Admin.DisciplinaryForms.List.SearchCreatedTo")]
    public DateTime? SearchCreatedToUtc { get; set; }
}
