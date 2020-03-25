using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Repositories
{
    [ExcludeFromCodeCoverage]
    public class CosmosDbConnection
    {
        public string AccessKey { get; set; }

        public string EndpointUrl { get; set; }

        public string DatabaseId { get; set; }

        public string QuestionSetsCollectionId { get; set; }

        public string UserSessionCollectionId { get; set; }

        public string PartitionKey { get; set; }
    }
}