using Dfc.Api.DiscoverSkillsAndCareers.Models;
using Dfc.Session;
using Dfc.Session.Models;
using DFC.Api.DiscoverSkillsAndCareers.Common.Services;
using DFC.Api.DiscoverSkillsAndCareers.Functions;
using DFC.Api.DiscoverSkillsAndCareers.Models;
using DFC.Api.DiscoverSkillsAndCareers.Repositories;
using FakeItEasy;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.DiscoverSkillsAndCareers.UnitTests.NewAssessment
{
    public class CreateNewSkillsAssessmentTests
    {
        private readonly HttpRequest httpRequest;
        private readonly NewAssessmentFunctions functionApp;
        private readonly IQuestionSetRepository questionSetRepository;
        private readonly IUserSessionRepository userSessionRepository;
        private readonly ISessionClient sessionClient;

        public CreateNewSkillsAssessmentTests()
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
        public async Task ReturnsBadRequestWhenEmptyRequestBodySent()
        {
            // Arrange
            var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);

            streamWriter.Write(string.Empty);
            streamWriter.Flush();
            memoryStream.Position = 0;

            A.CallTo(() => httpRequest.Body).Returns(memoryStream);

            // Act
            var result = await functionApp.CreateNewSkillsAssessment(httpRequest).ConfigureAwait(false);
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task ReturnsBadRequestWhenSessionIsNotValid()
        {
            // Arrange
            var dfcUserSession = CreateDfcUserSession();
            var ms = new MemoryStream();
            using var sw = new StreamWriter(ms);

            sw.Write(JsonConvert.SerializeObject(dfcUserSession));
            sw.Flush();
            ms.Position = 0;

            A.CallTo(() => httpRequest.Body).Returns(ms);
            A.CallTo(() => sessionClient.ValidateUserSession(A<DfcUserSession>.Ignored)).Returns(false);

            // Act
            var result = await functionApp.CreateNewSkillsAssessment(httpRequest).ConfigureAwait(false);
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task ReturnsNoContentResultWhenQuestionSetInfoDoesNotExist()
        {
            // Arrange
            var dfcUserSession = CreateDfcUserSession();
            var ms = new MemoryStream();
            using var sw = new StreamWriter(ms);

            sw.Write(JsonConvert.SerializeObject(dfcUserSession));
            sw.Flush();
            ms.Position = 0;

            A.CallTo(() => httpRequest.Body).Returns(ms);
            A.CallTo(() => questionSetRepository.GetCurrentQuestionSet(A<string>.Ignored)).Returns((QuestionSet)null);
            A.CallTo(() => sessionClient.ValidateUserSession(A<DfcUserSession>.Ignored)).Returns(true);

            // Act
            var result = await functionApp.CreateNewSkillsAssessment(httpRequest).ConfigureAwait(false);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task ReturnsAlreadyReportedResultWhenUserSessionAlreadyExists()
        {
            // Arrange
            var dfcUserSession = CreateDfcUserSession();
            var questionSet = CreateQuestionSet();
            var userSession = CreateUserSession();
            var ms = new MemoryStream();
            using var sw = new StreamWriter(ms);

            sw.Write(JsonConvert.SerializeObject(dfcUserSession));
            sw.Flush();
            ms.Position = 0;

            A.CallTo(() => httpRequest.Body).Returns(ms);
            A.CallTo(() => questionSetRepository.GetCurrentQuestionSet(A<string>.Ignored)).Returns(questionSet);
            A.CallTo(() => userSessionRepository.GetAsync(A<Expression<Func<UserSession, bool>>>.Ignored)).Returns(userSession);
            A.CallTo(() => sessionClient.ValidateUserSession(A<DfcUserSession>.Ignored)).Returns(true);

            // Act
            var result = await functionApp.CreateNewSkillsAssessment(httpRequest).ConfigureAwait(false);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.AlreadyReported, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CreatesUserSessionInDbSuccessfully()
        {
            // Arrange
            var dfcUserSession = CreateDfcUserSession();
            var questionSet = CreateQuestionSet();
            var ms = new MemoryStream();
            using var sw = new StreamWriter(ms);

            sw.Write(JsonConvert.SerializeObject(dfcUserSession));
            sw.Flush();
            ms.Position = 0;

            A.CallTo(() => httpRequest.Body).Returns(ms);
            A.CallTo(() => questionSetRepository.GetCurrentQuestionSet(A<string>.Ignored)).Returns(questionSet);
            A.CallTo(() => userSessionRepository.GetAsync(A<Expression<Func<UserSession, bool>>>.Ignored)).Returns((UserSession)null);
            A.CallTo(() => sessionClient.ValidateUserSession(A<DfcUserSession>.Ignored)).Returns(true);

            // Act
            var result = await functionApp.CreateNewSkillsAssessment(httpRequest).ConfigureAwait(false);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, statusCodeResult.StatusCode);
            A.CallTo(() => userSessionRepository.CreateUserSession(A<UserSession>.Ignored)).MustHaveHappenedOnceExactly();
        }

        private static QuestionSet CreateQuestionSet()
        {
            return new QuestionSet
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
        }

        private static UserSession CreateUserSession()
        {
            return new UserSession
            {
                UserSessionId = "sessionId",
            };
        }

        private static DfcUserSession CreateDfcUserSession()
        {
            return new DfcUserSession
            {
                SessionId = "sessionId",
                Salt = "salt",
                CreatedDate = DateTime.UtcNow,
                PartitionKey = "partitionkey",
            };
        }
    }
}