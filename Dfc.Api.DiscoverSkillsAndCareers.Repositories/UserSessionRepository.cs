using DFC.Api.DiscoverSkillsAndCareers.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DFC.Api.DiscoverSkillsAndCareers.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly CosmosDbConnection cosmosDbConnection;
        private readonly IDocumentClient client;

        public UserSessionRepository(IDocumentClient client, CosmosDbConnection cosmosDbConnection)
        {
            this.cosmosDbConnection = cosmosDbConnection;
            this.client = client;
        }

        private Uri DocumentCollectionUri => UriFactory.CreateDocumentCollectionUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.UserSessionCollectionId);

        public async Task CreateUserSession(UserSession userSession)
        {
            await client.CreateDocumentAsync(DocumentCollectionUri, userSession).ConfigureAwait(false);
        }

        public async Task<UserSession> GetAsync(Expression<Func<UserSession, bool>> where)
        {
            var query = client.CreateDocumentQuery<UserSession>(DocumentCollectionUri, new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                .Where(where)
                .AsDocumentQuery();

            if (query == null)
            {
                return default;
            }

            var models = await query.ExecuteNextAsync<UserSession>().ConfigureAwait(false);

            if (models != null && models.Count > 0)
            {
                return models.FirstOrDefault();
            }

            return default;
        }
    }
}