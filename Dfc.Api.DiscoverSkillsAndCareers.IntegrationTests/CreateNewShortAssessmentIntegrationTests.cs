using Dfc.Session.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Globalization;
using System.Net;
using Xunit;

namespace Dfc.Api.DiscoverSkillsAndCareers.IntegrationTests
{
    public class CreateNewShortAssessmentIntegrationTests
    {
        private readonly string apiBaseUrl;

        public CreateNewShortAssessmentIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

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
    }
}