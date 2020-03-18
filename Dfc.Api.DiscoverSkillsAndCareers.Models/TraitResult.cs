using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class TraitResult
    {
        [JsonProperty("traitCode")]
        public string TraitCode { get; set; }

        [JsonProperty("traitName")]
        public string TraitName { get; set; }

        [JsonProperty("traitText")]
        public string TraitText { get; set; }

        [JsonProperty("totalScore")]
        public int TotalScore { get; set; }
    }
}