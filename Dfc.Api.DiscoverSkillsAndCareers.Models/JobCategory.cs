using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class JobCategory
    {
        [JsonProperty("id")]
        public string Code => JobCategoryHelper.GetCode(Name);

        [JsonProperty("jobFamilyName")]
        public string Name { get; set; }

        [JsonProperty("texts")]
        public List<JobCategoryText> Texts { get; set; } = new List<JobCategoryText>();

        [JsonProperty("traitCodes")]
        public List<string> Traits { get; set; } = new List<string>();

        [JsonIgnore]
        public decimal ResultMultiplier => 1m / Traits.Count;

        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }

        public List<JobProfileSkillMapping> Skills { get; set; } = new List<JobProfileSkillMapping>();
    }
}