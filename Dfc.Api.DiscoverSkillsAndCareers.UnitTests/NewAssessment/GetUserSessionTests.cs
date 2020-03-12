using DFC.Api.DiscoverSkillsAndCareers.Common.Services;
using DFC.Api.DiscoverSkillsAndCareers.Functions;
using FakeItEasy;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.Api.DiscoverSkillsAndCareers.UnitTests.NewAssessment
{
    public class GetUserSessionTests
    {
        private readonly HttpRequest httpRequest;
        private readonly NewAssessmentFunctions functionApp;

        public GetUserSessionTests()
        {
            httpRequest = A.Fake<HttpRequest>();
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var correlationProvider = new RequestHeaderCorrelationIdProvider(httpContextAccessor);
            using var telemetryConfig = new TelemetryConfiguration();
            var telemetryClient = new TelemetryClient(telemetryConfig);
            var logger = new LogService(correlationProvider, telemetryClient);
            var correlationResponse = new ResponseWithCorrelation(correlationProvider, httpContextAccessor);

            functionApp = new NewAssessmentFunctions(logger, correlationResponse);
        }

        [Fact]
        public void DummyTest()
        {
            // Act
            var result = functionApp.GetUserSession(httpRequest, "DummySessionId");
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            // Assert
            Assert.Equal((int)HttpStatusCode.Created, statusCodeResult.StatusCode);
        }
    }
}