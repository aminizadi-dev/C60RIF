using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Web.Areas.Admin.Models.Customers;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the agency model factory
/// </summary>
public partial interface IAgencyModelFactory
{
    Task<AgencySearchModel> PrepareAgencySearchModelAsync(AgencySearchModel searchModel);

    Task<AgencyListModel> PrepareAgencyListModelAsync(AgencySearchModel searchModel);

    Task<AgencyModel> PrepareAgencyModelAsync(AgencyModel model, Agency agency, bool excludeProperties = false);
}

