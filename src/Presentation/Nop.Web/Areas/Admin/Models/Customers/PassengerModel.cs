using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Customers;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a passenger model
/// </summary>
public partial record PassengerModel : BaseNopEntityModel
{
    #region Ctor

    public PassengerModel()
    {
        AvailableEducationLevels = new List<SelectListItem>();
        AvailableMaritalStatuses = new List<SelectListItem>();
        AvailableEmploymentStatuses = new List<SelectListItem>();
        AvailableAntiXItems = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Passengers.Fields.RecoveryNo")]
    public int RecoveryNo { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.PersonName")]
    public string PersonName { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.GuideNameAndLegionNo")]
    public string GuideNameAndLegionNo { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.ClinicName")]
    public string ClinicName { get; set; }

    [UIHint("PersianDateNullable")]
    [PersianDate]
    [NopResourceDisplayName("Admin.Passengers.Fields.BirthDate")]
    public DateTime? BirthDateUtc { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.Education")]
    public int Education { get; set; }
    public IList<SelectListItem> AvailableEducationLevels { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.MaritalStatus")]
    public int MaritalStatus { get; set; }
    public IList<SelectListItem> AvailableMaritalStatuses { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.EmploymentStatus")]
    public int EmploymentStatus { get; set; }
    public IList<SelectListItem> AvailableEmploymentStatuses { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.CardNo")]
    public long? CardNo { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.AntiX1")]
    public int AntiX1 { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.AntiX2")]
    public int AntiX2 { get; set; }

    public IList<SelectListItem> AvailableAntiXItems { get; set; }

    [UIHint("PersianDateNullable")]
    [PersianDate]
    [NopResourceDisplayName("Admin.Passengers.Fields.TravelStartDate")]
    public DateTime? TravelStartDateUtc { get; set; }

    [UIHint("PersianDateNullable")]
    [PersianDate]
    [NopResourceDisplayName("Admin.Passengers.Fields.TravelEndDate")]
    public DateTime? TravelEndDateUtc { get; set; }

    [UIHint("Picture")]
    [NopResourceDisplayName("Admin.Passengers.Fields.PictureId")]
    public int PictureId { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.CreatedOn")]
    public DateTime CreatedOnUtc { get; set; }

    #endregion
}

