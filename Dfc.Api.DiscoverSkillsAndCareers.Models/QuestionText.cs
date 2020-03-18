using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class QuestionText
    {
        [JsonProperty("languageCode")]
        public string LanguageCode { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}