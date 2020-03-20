using DFC.Api.DiscoverSkillsAndCareers.Common.Constants;
using DFC.Api.DiscoverSkillsAndCareers.Common.Services;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace DFC.Api.DiscoverSkillsAndCareers.Extensions
{
    public static class HttpRequestExtensions
    {
        public static void LogRequestHeaders(this HttpRequest request, ILogService logger)
        {
            var message = new StringBuilder();

            request?.Headers.TryGetValue(HeaderName.ApimUrl, out var apimUrl);
            message.AppendLine($"Request Header Key: '{HeaderName.ApimUrl}', Value: '{apimUrl}'");

            request?.Headers.TryGetValue(HeaderName.RequestId, out var requestId);
            message.AppendLine($"Request Header Key: '{HeaderName.RequestId}', Value: '{requestId}'");

            request?.Headers.TryGetValue(HeaderName.Version, out var version);
            message.AppendLine($"Request Header Key: '{HeaderName.Version}', Value: '{version}'");

            logger?.LogMessage(message.ToString(), SeverityLevel.Information);
        }
    }
}