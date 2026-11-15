namespace Nop.Services.Reports;

/// <summary>
/// Filtered bar-chart data for the passenger reports
/// </summary>
public partial interface IPassengerChartReportService
{
    /// <summary>
    /// Age groups of visitors. Date range applies to the travel start date.
    /// </summary>
    Task<IList<ChartReportPoint>> GetAgeDistributionAsync(int cityId, int agencyId, DateTime? dateFrom, DateTime? dateTo);

    /// <summary>
    /// Education levels of visitors. Date range applies to the travel start date.
    /// </summary>
    Task<IList<ChartReportPoint>> GetEducationDistributionAsync(int cityId, int agencyId, DateTime? dateFrom, DateTime? dateTo);

    /// <summary>
    /// Registered liberations in a Persian year, one bar per month.
    /// </summary>
    Task<IList<ChartReportPoint>> GetLiberationByMonthAsync(int year, int cityId, int agencyId);

    /// <summary>
    /// Persian years that have at least one registered liberation for the location filter.
    /// </summary>
    Task<IList<int>> GetLiberationYearsAsync(int cityId, int agencyId);

    /// <summary>
    /// Registered liberations grouped by city. Date range applies to the liberation date.
    /// </summary>
    Task<IList<ChartReportPoint>> GetLiberationByCityAsync(DateTime? dateFrom, DateTime? dateTo);

    /// <summary>
    /// AntiX consumption distribution
    /// </summary>
    Task<IList<ChartReportPoint>> GetAntiXDistributionAsync();

    /// <summary>
    /// Marital status distribution
    /// </summary>
    Task<IList<ChartReportPoint>> GetMaritalStatusAsync();

    /// <summary>
    /// Employment status distribution
    /// </summary>
    Task<IList<ChartReportPoint>> GetEmploymentStatusAsync();
}
