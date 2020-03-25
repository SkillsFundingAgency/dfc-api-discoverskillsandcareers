using Dfc.Api.DiscoverSkillsAndCareers.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.DiscoverSkillsAndCareers.Repositories
{
    [ExcludeFromCodeCoverage]
    public class QuestionSetRepository : IQuestionSetRepository
    {
        private readonly CosmosDbConnection cosmosDbConnection;
        private readonly IDocumentClient client;

        public QuestionSetRepository(IDocumentClient client, CosmosDbConnection cosmosDbConnection)
        {
            this.client = client;
            this.cosmosDbConnection = cosmosDbConnection;

            if (this.cosmosDbConnection == null)
            {
                throw new ArgumentNullException(nameof(cosmosDbConnection));
            }
        }

        private Uri DocumentCollectionUri => UriFactory.CreateDocumentCollectionUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.QuestionSetsCollectionId);

        public async Task<QuestionSet> GetCurrentQuestionSet(string assessmentType)
        {
            var feedOptions = new RequestOptions { PartitionKey = new PartitionKey("latest-questionset") };

            var uri = UriFactory.CreateDocumentUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.QuestionSetsCollectionId, $"latest-{assessmentType}");
            QuestionSet qs = null;

            try
            {
                var result = await client.ReadDocumentAsync<QuestionSet>(uri, feedOptions).ConfigureAwait(false);
                qs = result.Document;
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    var latestQs = client.CreateDocumentQuery<QuestionSet>(DocumentCollectionUri, new FeedOptions() { EnableCrossPartitionQuery = true })
                                   .Where(x => x.AssessmentType == assessmentType && x.IsCurrent)
                                   .OrderByDescending(x => x.Version)
                                   .AsEnumerable()
                                   .FirstOrDefault();

                    await CreateUpdateLatestQuestionSet(latestQs).ConfigureAwait(false);

                    qs = latestQs;
                }
            }

            if (qs == null)
            {
                return qs;
            }

            qs.QuestionSetVersion = $"{assessmentType?.ToLowerInvariant()}-{qs.Title?.ToLowerInvariant()}-{qs.Version.ToString(new NumberFormatInfo())}";
            qs.PartitionKey = "ncs";
            return qs;
        }

        private async Task CreateUpdateLatestQuestionSet(QuestionSet questionSet)
        {
            if (questionSet?.IsCurrent == true)
            {
                questionSet.PartitionKey = "latest-questionset";
                questionSet.QuestionSetVersion = $"latest-{questionSet.AssessmentType}";

                await client.UpsertDocumentAsync(DocumentCollectionUri, questionSet).ConfigureAwait(false);
            }
        }
    }
}