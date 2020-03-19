using DFC.Api.DiscoverSkillsAndCareers.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DFC.Api.DiscoverSkillsAndCareers.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly CosmosDbConnection cosmosSettings;
        private readonly IDocumentClient client;

        public UserSessionRepository(IDocumentClient client, CosmosDbConnection cosmosSettings)
        {
            this.cosmosSettings = cosmosSettings;
            this.client = client;
        }

        public async Task CreateUserSession(UserSession userSession)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(cosmosSettings.DatabaseId, cosmosSettings.UserSessionCollectionId);
            await client.CreateDocumentAsync(uri, userSession).ConfigureAwait(false);
        }
    }
}