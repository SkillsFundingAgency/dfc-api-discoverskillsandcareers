using Microsoft.ApplicationInsights.DataContracts;

namespace DFC.Api.DiscoverSkillsAndCareers.Common.Services
{
    public interface ILogService
    {
        void LogMessage(string message, SeverityLevel severityLevel);
    }
}