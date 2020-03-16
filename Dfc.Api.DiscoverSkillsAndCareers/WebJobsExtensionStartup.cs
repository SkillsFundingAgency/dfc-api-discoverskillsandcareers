using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle;
using DFC.Api.DiscoverSkillsAndCareers;
using DFC.Api.DiscoverSkillsAndCareers.Common.Services;
using DFC.Functions.DI.Standard;
using DFC.Swagger.Standard;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[assembly: WebJobsStartup(typeof(WebJobsExtensionStartup), "Web Jobs Extension Startup")]

namespace DFC.Api.DiscoverSkillsAndCareers
{
    [ExcludeFromCodeCoverage]
    public class WebJobsExtensionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddDependencyInjection();
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
            builder?.Services.AddApplicationInsightsTelemetry();
            builder?.Services.AddAutoMapper(typeof(WebJobsExtensionStartup).Assembly);
            builder?.Services.AddSingleton<ILogger, Logger<WebJobsExtensionStartup>>();
            builder?.Services.AddTransient<ISwaggerDocumentGenerator, SwaggerDocumentGenerator>();
            builder?.Services.AddScoped<ICorrelationIdProvider, RequestHeaderCorrelationIdProvider>();
            builder?.Services.AddScoped<ILogService, LogService>();
            builder?.Services.AddScoped<IResponseWithCorrelation, ResponseWithCorrelation>();
        }
    }
}