using System;

namespace Nop.Core.Domain.Customers;



/// <summary>
/// Represents a recovery form record
/// </summary>
public partial class Passenger : BaseEntity
{
    public int RecoveryNo { get; set; }

    public string PersonName { get; set; }

    public string GuideNameAndLegionNo { get; set; }

    public int ClinicId { get; set; }

    public int? BirthYear { get; set; }

    public EducationLevel Education { get; set; }

    public bool IsMarried { get; set; }

    public bool IsEmployed { get; set; }

    public bool? HasCompanion { get; set; }

    public string CardNo { get; set; }

    public int AntiX1 { get; set; }

    public int? AntiX2 { get; set; }

    public DateTime? TravelStartDateUtc { get; set; }

    public DateTime? TravelEndDateUtc { get; set; }

    public int PictureId { get; set; }

    public int AgencyId { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}

public enum EducationLevel
{
    Unknown = 0,          // نامشخص (وضعیت کلی نامعلوم)
    Illiterate = 20,      // بیسواد
    Primary = 30,         // ابتدایی
    MiddleSchool = 40,    // راهنمایی / سیکل
    HighSchool = 50,      // دبیرستان
    Diploma = 60,         // دیپلم
    Associate = 70,       // کاردانی
    Bachelor = 80,        // کارشناسی
    Master = 90,          // کارشناسی ارشد
    Doctorate = 100,      // دکتری
    PostDoctorate = 110   // فوق دکتری
}

