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
        AvailableAntiXItems = new List<SelectListItem>();
        AvailableCities = new List<SelectListItem>();
        AvailableAgencies = new List<SelectListItem>();
        AvailableClinics = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Passengers.Fields.RecoveryNo")]
    public int RecoveryNo { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.PersonName")]
    public string PersonName { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.GuideNameAndLegionNo")]
    public string GuideNameAndLegionNo { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.Clinic")]
    public int ClinicId { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.BirthYear")]
    public int? BirthYear { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.Education")]
    public int Education { get; set; }
    public IList<SelectListItem> AvailableEducationLevels { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.IsMarried")]
    public bool IsMarried { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.IsEmployed")]
    public bool IsEmployed { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.HasCompanion")]
    public bool? HasCompanion { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.CardNo")]
    public string CardNo { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.AntiX1")]
    public int AntiX1 { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.AntiX2")]
    public int? AntiX2 { get; set; }

    public IList<SelectListItem> AvailableAntiXItems { get; set; }

    public string AntiX1Name { get; set; }
    public string AntiX2Name { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.City")]
    public int CityId { get; set; }

    [NopResourceDisplayName("Admin.Passengers.Fields.Agency")]
    public int AgencyId { get; set; }

    public IList<SelectListItem> AvailableCities { get; set; }
    public IList<SelectListItem> AvailableAgencies { get; set; }
    public IList<SelectListItem> AvailableClinics { get; set; }

    public string AgencyName { get; set; }

    public string PictureUrl { get; set; }

    public string PictureFullSizeUrl { get; set; }

    public string EndDateOnPersian { get; set; }

    public string StartDateOnPersian { get; set; }

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

