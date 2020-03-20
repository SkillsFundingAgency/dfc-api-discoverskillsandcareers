using Dfc.Api.DiscoverSkillsAndCareers.Models;
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
    public class CreateNewShortAssessmentTests
    {
        private readonly HttpRequest httpRequest;
        private readonly ISessionClient sessionClient;
        private readonly IQuestionSetRepository questionSetRepository;
        private readonly IUserSessionRepository userSessionRepository;
        private readonly NewAssessmentFunctions functionApp;

        public CreateNewShortAssessmentTests()
        {
            httpRequest = A.Fake<HttpRequest>();
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            questionSetRepository = A.Fake<IQuestionSetRepository>();
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
        public async Task ReturnsNoContentResultWhenQuestionSetInfoDoesNotExist()
        {
            // Arrange
            A.CallTo(() => questionSetRepository.GetCurrentQuestionSet(A<string>.Ignored)).Returns((QuestionSet)null);

            // Act
            var result = await functionApp.CreateNewShortAssessment(httpRequest).ConfigureAwait(false);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task ReturnsCreatedDfcUserSession()
        {
            // Arrange
            var dfcSession = new DfcUserSession
            {
                PartitionKey = "partitionKey",
                CreatedDate = DateTime.UtcNow,
                SessionId = "sessionId",
                Salt = "ncs",
            };
            A.CallTo(() => sessionClient.NewSession()).Returns(dfcSession);

            var questionSet = new QuestionSet
            {
                QuestionSetVersion = "qsVersion",
                AssessmentType = "short",
                MaxQuestions = 5,
                PartitionKey = "partitionKey",
                Description = "short description",
                IsCurrent = true,
                LastUpdated = DateTimeOffset.UtcNow,
                QuestionSetKey = "qsKey",
                Title = "qstitle",
                Version = 1,
            };

            A.CallTo(() => questionSetRepository.GetCurrentQuestionSet(A<string>.Ignored)).Returns(questionSet);

            // Act
            var result = await functionApp.CreateNewShortAssessment(httpRequest).ConfigureAwait(false);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var deserialisedResult = JsonConvert.DeserializeObject<DfcUserSession>(okObjectResult.Value.ToString());

            // Assert
            deserialisedResult.Should().BeEquivalentTo(dfcSession);
            A.CallTo(() => userSessionRepository.CreateUserSession(A<UserSession>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}