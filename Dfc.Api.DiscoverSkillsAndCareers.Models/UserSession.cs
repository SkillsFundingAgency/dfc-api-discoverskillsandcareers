using Newtonsoft.Json;
using System;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    public class UserSession
    {
        [JsonIgnore]
        public string PrimaryKey => $"{PartitionKey}-{UserSessionId}";

        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }

        [JsonProperty("id")]
        public string UserSessionId { get; set; }

        [JsonProperty("languageCode")]
        public string LanguageCode { get; set; }

        [JsonProperty("salt")]
        public string Salt { get; set; }

        [JsonProperty("assessmentState")]
        public AssessmentState AssessmentState { get; set; }

        [JsonProperty("filteredAssessmentState")]
        public object FilteredAssessmentState { get; set; }

        [JsonProperty("resultData")]
        public object ResultData { get; set; }

        [JsonProperty("startedDt")]
        public DateTime StartedDt { get; set; }

        [JsonProperty("assessmentType")]
        public string AssessmentType { get; set; }

        [JsonProperty("lastUpdatedDt")]
        public DateTime LastUpdatedDt { get; set; }

        [JsonProperty("completeDt")]
        public DateTime? CompleteDt { get; set; }
    }
}