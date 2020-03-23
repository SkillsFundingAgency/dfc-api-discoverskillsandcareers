using Dfc.Session;
using Dfc.Session.Models;
using DFC.Api.DiscoverSkillsAndCareers.Models;
using DFC.Api.DiscoverSkillsAndCareers.Repositories;
using FluentAssertions;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Dfc.Api.DiscoverSkillsAndCareers.IntegrationTests
{
    public class CreateNewAssessmentIntegrationTests
    {
        private readonly string apiBaseUrl;
        private readonly IUserSessionRepository userSessionRepository;
        private readonly ISessionClient sessionClient;

        public CreateNewAssessmentIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var cosmosDbConnection = configuration.GetSection("CosmosDbConnection").Get<CosmosDbConnection>();
            var sessionConfig = configuration.GetSection("SessionConfig").Get<SessionConfig>();
            var documentClient = new DocumentClient(new Uri(cosmosDbConnection.EndpointUrl), cosmosDbConnection.AccessKey);
            this.userSessionRepository = new UserSessionRepository(documentClient, cosmosDbConnection);

            var serviceProvider = new ServiceCollection().AddSessionServices(sessionConfig);
            serviceProvider.AddLogging();
            serviceProvider.AddHttpContextAccessor();

            var services = serviceProvider.BuildServiceProvider();

            sessionClient = services.GetService<ISessionClient>();

            apiBaseUrl = configuration.GetConnectionString("ApiBaseUrl");
        }

        [Fact]
        public void CreateNewShortAssessmentReturnsDfcUserSession()
        {
            // Arrange
            var client = new RestClient(apiBaseUrl);
            var req = new RestRequest("assessment/short", Method.POST);

            // Act
            var response = client.Execute(req);

            // Assert
            Assert.True(response.IsSuccessful && response.StatusCode == HttpStatusCode.OK);
            var userSession = JsonConvert.DeserializeObject<DfcUserSession>(response.Content);

            Assert.True(userSession.PartitionKey != null &&
                        userSession.SessionId != null &&
                        userSession.Salt != null &&
                        !string.IsNullOrWhiteSpace(userSession.CreatedDate.ToString(new DateTimeFormatInfo())));
        }

        [Fact]
        public async Task CreateNewSkillsAssessmentCreatedSuccessfully()
        {
            // Arrange
            var dfcSession = sessionClient.NewSession();
            var client = new RestClient(apiBaseUrl);
            var req = new RestRequest("assessment/skills", Method.POST)
            {
                Body = new RequestBody("application/json", "integrationTest", JsonConvert.SerializeObject(dfcSession)),
            };

            // Act
            var response = client.Execute(req);

            // Assert
            Assert.True(response.IsSuccessful && response.StatusCode == HttpStatusCode.Created);

            var createdUserSession = await userSessionRepository.GetAsync(s => s.UserSessionId == dfcSession.SessionId).ConfigureAwait(false);
            Assert.NotNull(createdUserSession);
            Assert.True(createdUserSession.PartitionKey == dfcSession.PartitionKey &&
                createdUserSession.Salt == dfcSession.Salt &&
                createdUserSession.UserSessionId == dfcSession.SessionId &&
                createdUserSession.StartedDt == dfcSession.CreatedDate);
        }

        [Fact]
        public async Task GetSessionReturnsDfcUserSession()
        {
            // Arrange
            var sessionId = $"sessionId{Guid.NewGuid()}";
            var startedDate = DateTime.UtcNow;

            var userSession = CreateUserSession(sessionId, startedDate);
            await userSessionRepository.CreateUserSession(userSession).ConfigureAwait(false);

            var expectedDfcUserSession = CreateDfcUserSession(sessionId, startedDate);
            var client = new RestClient(apiBaseUrl);
            var req = new RestRequest($"assessment/session/{sessionId}", Method.GET);

            // Act
            var response = client.Execute(req);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var deserialisedResult = JsonConvert.DeserializeObject<DfcUserSession>(response.Content);
            deserialisedResult.Should().BeEquivalentTo(expectedDfcUserSession);
        }

        private static DfcUserSession CreateDfcUserSession(string sessionId, DateTime startedDateTime)
        {
            return new DfcUserSession
            {
                SessionId = sessionId,
                Salt = "salt",
                CreatedDate = startedDateTime,
                PartitionKey = "integrationTest",
            };
        }

        private static UserSession CreateUserSession(string sessionId, DateTime startedDateTime)
        {
            return new UserSession
            {
                UserSessionId = sessionId,
                Salt = "salt",
                PartitionKey = "integrationTest",
                StartedDt = startedDateTime,
                AssessmentType = "intTest",
            };
        }
    }
}