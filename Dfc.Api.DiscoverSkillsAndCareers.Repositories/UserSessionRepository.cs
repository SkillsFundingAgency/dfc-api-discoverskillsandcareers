using DFC.Api.DiscoverSkillsAndCareers.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
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

        public async Task<UserSession> GetByIdAsync(string sessionId, string partitionKey)
        {
            try
            {
                var uri = UriFactory.CreateDocumentUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.UserSessionCollectionId, sessionId);
                var requestOptions = new RequestOptions { PartitionKey = new PartitionKey(partitionKey) };
                var document = await client.ReadDocumentAsync<UserSession>(uri, requestOptions).ConfigureAwait(false);
                return document;
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }
    }
}