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
        private readonly CosmosDbConnection cosmosSettings;
        private readonly IDocumentClient client;

        public UserSessionRepository(IDocumentClient client, CosmosDbConnection cosmosSettings)
        {
            this.cosmosSettings = cosmosSettings;
            this.client = client;
        }

        public async Task<UserSession> GetUserSession(string primaryKey)
        {
            primaryKey = primaryKey?.Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase);
            var pos = primaryKey?.IndexOf('-', StringComparison.OrdinalIgnoreCase);
            if (pos <= 0)
            {
                return null;
            }

            var partitionKey = primaryKey?.Substring(0, pos.GetValueOrDefault());
            var userSessionId = primaryKey?.Substring(pos.GetValueOrDefault() + 1, primaryKey.Length - (pos.GetValueOrDefault() + 1));
            return await GetUserSession(userSessionId, partitionKey).ConfigureAwait(false);
        }

        public async Task CreateUserSession(UserSession userSession)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(cosmosSettings.DatabaseId, cosmosSettings.UserSessionCollectionId);
            await client.CreateDocumentAsync(uri, userSession).ConfigureAwait(false);
        }

        public async Task UpdateUserSession(UserSession userSession)
        {
            if (userSession != null)
            {
                userSession.LastUpdatedDt = DateTime.UtcNow;
                var uri = UriFactory.CreateDocumentUri(cosmosSettings.DatabaseId, cosmosSettings.UserSessionCollectionId, userSession.UserSessionId);
                var requestOptions = new RequestOptions { PartitionKey = new PartitionKey(userSession.PartitionKey) };
                await client.ReplaceDocumentAsync(uri, userSession, requestOptions).ConfigureAwait(false);
            }
        }

        private async Task<UserSession> GetUserSession(string userSessionId, string partitionKey)
        {
            try
            {
                var uri = UriFactory.CreateDocumentUri(cosmosSettings.DatabaseId, cosmosSettings.UserSessionCollectionId, userSessionId);
                var requestOptions = new RequestOptions { PartitionKey = new PartitionKey(partitionKey) };
                return await client.ReadDocumentAsync<UserSession>(uri, requestOptions).ConfigureAwait(false);
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