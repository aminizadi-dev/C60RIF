using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Infrastructure;
using Nop.Services.Localization;
using Nop.Services.Reports;
using Nop.Web.Areas.Admin.Models.Reports;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the passenger performance report model factory
/// </summary>
public partial class PassengerPerformanceReportModelFactory : IPassengerPerformanceReportModelFactory
{
    protected readonly ILocalizationService _localizationService;
    protected readonly IPassengerPerformanceReportService _passengerPerformanceReportService;

    public PassengerPerformanceReportModelFactory(
        ILocalizationService localizationService,
        IPassengerPerformanceReportService passengerPerformanceReportService)
    {
        _localizationService = localizationService;
        _passengerPerformanceReportService = passengerPerformanceReportService;
    }

    /// <inheritdoc />
    public virtual async Task<PassengerPerformanceReportIndexModel> PrepareIndexModelAsync()
    {
        var summary = await _passengerPerformanceReportService.GetSummaryAsync();
        var searchModel = new PassengerPerformanceReportSearchModel();
        var adminAreaSettings = EngineContext.Current.Resolve<AdminAreaSettings>();
        searchModel.SetGridPageSize(50, adminAreaSettings?.GridPageSizes);

        return new PassengerPerformanceReportIndexModel
        {
            Summary = new PassengerPerformanceReportSummaryModel
            {
                CountToday = summary.CountToday,
                CountThisWeek = summary.CountThisWeek,
                CountThisMonth = summary.CountThisMonth,
                CountThisYear = summary.CountThisYear,
                CountAllTime = summary.CountAllTime,
                CountUnattributed = summary.CountUnattributed,
                CurrentPersianYear = summary.CurrentPersianYear
            },
            SearchModel = searchModel
        };
    }

    /// <inheritdoc />
    public virtual async Task<PassengerPerformanceReportListModel> PrepareListModelAsync(PassengerPerformanceReportSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var lines = await _passengerPerformanceReportService.GetUserReportLinesAsync();
        var totalLabel = await _localizationService.GetResourceAsync("Admin.Reports.PassengerPerformance.SystemTotal");

        var reportModels = lines.Select(line => new PassengerPerformanceReportModel
        {
            CustomerId = line.CustomerId,
            UserDisplay = line.IsSystemTotal ? totalLabel : line.FullName,
            Email = line.IsSystemTotal ? string.Empty : line.Email,
            IsSystemTotal = line.IsSystemTotal,
            CountToday = line.CountToday,
            CountThisWeek = line.CountThisWeek,
            CountThisMonth = line.CountThisMonth,
            CountThisYear = line.CountThisYear,
            CountAllTime = line.CountAllTime,
            CountUnattributed = line.CountUnattributed
        }).ToList();

        var paged = new PagedList<PassengerPerformanceReportModel>(reportModels, searchModel.Page - 1, searchModel.PageSize);

        return new PassengerPerformanceReportListModel().PrepareToGrid(searchModel, paged, () => paged);
    }
}
