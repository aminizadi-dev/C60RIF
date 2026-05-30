namespace Nop.Services.Reports;

/// <summary>
/// Passenger performance report service interface
/// </summary>
public partial interface IPassengerPerformanceReportService
{
    /// <summary>
    /// Gets system-wide summary counts for the current Persian periods
    /// </summary>
    Task<PassengerPerformanceReportSummary> GetSummaryAsync();

    /// <summary>
    /// Gets per-user report lines for customers with passenger create permission
    /// </summary>
    Task<IList<PassengerPerformanceReportLine>> GetUserReportLinesAsync();
}
