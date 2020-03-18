using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class JobCategoryText
    {
        [JsonProperty("languageCode")]
        public string LanguageCode { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}