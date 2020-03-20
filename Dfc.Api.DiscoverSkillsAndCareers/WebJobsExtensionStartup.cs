using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle;
using DFC.Api.DiscoverSkillsAndCareers;
using DFC.Api.DiscoverSkillsAndCareers.Common.Services;
using DFC.Api.DiscoverSkillsAndCareers.Repositories;
using DFC.Functions.DI.Standard;
using Dfc.Session;
using Dfc.Session.Models;
using DFC.Swagger.Standard;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var cosmosDbConnection = configuration.GetSection("CosmosDbConnection").Get<CosmosDbConnection>();
            var sessionConfig = configuration.GetSection("SessionConfig").Get<SessionConfig>();

            builder.AddDependencyInjection();
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
            builder?.Services.AddSessionServices(sessionConfig);
            builder?.Services.AddApplicationInsightsTelemetry();
            builder?.Services.AddAutoMapper(typeof(WebJobsExtensionStartup).Assembly);
            builder?.Services.AddSingleton(cosmosDbConnection);
            builder?.Services.AddSingleton(sessionConfig);
            builder?.Services.AddSingleton<ILogger, Logger<WebJobsExtensionStartup>>();
            builder?.Services.AddTransient<ISwaggerDocumentGenerator, SwaggerDocumentGenerator>();
            builder?.Services.AddScoped<ICorrelationIdProvider, RequestHeaderCorrelationIdProvider>();
            builder?.Services.AddScoped<ILogService, LogService>();
            builder?.Services.AddScoped<IResponseWithCorrelation, ResponseWithCorrelation>();
            builder?.Services.AddScoped<IQuestionSetRepository, QuestionSetRepository>();
            builder?.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
            builder?.Services.AddSingleton<IDocumentClient>(new DocumentClient(new Uri(cosmosDbConnection.EndpointUrl), cosmosDbConnection.AccessKey));
        }
    }
}