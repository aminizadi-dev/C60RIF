using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Common;

public partial record CommonStatisticsModel : BaseNopModel
{
    public int NumberOfPassengers { get; set; }
    public int NumberOfCities { get; set; }
    public int NumberOfAgencies { get; set; }
    public int NumberOfClinics { get; set; }
}