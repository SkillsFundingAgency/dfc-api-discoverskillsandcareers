using Dfc.Session;
using Dfc.Session.Models;
using DFC.Api.DiscoverSkillsAndCareers.Common.Services;
using DFC.Api.DiscoverSkillsAndCareers.Functions;
using DFC.Api.DiscoverSkillsAndCareers.Models;
using DFC.Api.DiscoverSkillsAndCareers.Repositories;
using FakeItEasy;
using FluentAssertions;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.DiscoverSkillsAndCareers.UnitTests.NewAssessment
{
    public class GetUserSessionTests
    {
        private readonly HttpRequest httpRequest;
        private readonly NewAssessmentFunctions functionApp;
        private readonly IUserSessionRepository userSessionRepository;
        private readonly ISessionClient sessionClient;

        public GetUserSessionTests()
        {
            httpRequest = A.Fake<HttpRequest>();
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var questionSetRepository = A.Fake<IQuestionSetRepository>();
            userSessionRepository = A.Fake<IUserSessionRepository>();
            sessionClient = A.Fake<ISessionClient>();
            var correlationProvider = new RequestHeaderCorrelationIdProvider(httpContextAccessor);
            using var telemetryConfig = new TelemetryConfiguration();
            var telemetryClient = new TelemetryClient(telemetryConfig);
            var logger = new LogService(correlationProvider, telemetryClient);
            var correlationResponse = new ResponseWithCorrelation(correlationProvider, httpContextAccessor);

            functionApp = new NewAssessmentFunctions(logger, correlationResponse, questionSetRepository, userSessionRepository, sessionClient, correlationProvider);
        }

        [Fact]
        public async Task ReturnsNoContentResultWhenUserSessionDoesNotExist()
        {
            // Arrange
            A.CallTo(() => userSessionRepository.GetByIdAsync(A<string>.Ignored, A<string>.Ignored)).Returns((UserSession)null);

            // Act
            var result = await functionApp.GetUserSession(httpRequest, "DummySessionId").ConfigureAwait(false);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task ReturnsMappedDfcUserSessionWhenUserSessionExists()
        {
            // Arrange
            const string userSessionId = "userSessionId";
            const string partitionKey = "partitionKey";
            const string salt = "salt";
            var startedDateTime = DateTime.UtcNow;

            var userSession = new UserSession
            {
                UserSessionId = userSessionId,
                PartitionKey = partitionKey,
                Salt = salt,
                StartedDt = startedDateTime,
            };

            var expectedDfcUserSession = new DfcUserSession
            {
                Salt = salt,
                SessionId = userSessionId,
                CreatedDate = startedDateTime,
                PartitionKey = partitionKey,
            };

            A.CallTo(() => userSessionRepository.GetByIdAsync(A<string>.Ignored, A<string>.Ignored)).Returns(userSession);
            A.CallTo(() => sessionClient.GeneratePartitionKey(A<string>.Ignored)).Returns(partitionKey);

            // Act
            var result = await functionApp.GetUserSession(httpRequest, "DummySessionId").ConfigureAwait(false);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var deserialisedResult = JsonConvert.DeserializeObject<DfcUserSession>(okObjectResult.Value.ToString());

            deserialisedResult.Should().BeEquivalentTo(expectedDfcUserSession);
        }
    }
}