using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class ResultData
    {
        [JsonProperty("traits")]
        public List<TraitResult> Traits { get; set; }

        [JsonProperty("jobFamilies")]
        public List<JobCategoryResult> JobCategories { get; set; }

        [JsonProperty("traitsscores")]
        public List<TraitResult> TraitScores { get; set; }
    }
}