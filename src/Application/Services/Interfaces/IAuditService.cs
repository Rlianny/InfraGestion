public interface IAuditService
{
    Task LogUserActionAsync(UserActionLog request);
    Task<List<UserActionLog>> GetUserActionLogsAsync(string userId, DateRange dateRange);
    Task LogSystemEventAsync(SystemEventLog request);
    Task<List<SystemEventLog>> GetSystemEventLogsAsync(DateRange dateRange);
}