using DFC.Api.DiscoverSkillsAndCareers.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class Answer
    {
        [JsonProperty("questionId")]
        public string QuestionId { get; set; }

        [JsonProperty("questionNumber")]
        public int QuestionNumber { get; set; }

        [JsonProperty("questionText")]
        public string QuestionText { get; set; }

        [JsonProperty("traitCode")]
        public string TraitCode { get; set; }

        [JsonProperty("selectedOption")]
        public AnswerOption SelectedOption { get; set; }

        [JsonProperty("answeredDt")]
        public DateTime AnsweredDt { get; set; }

        [JsonProperty("isNegative")]
        public bool IsNegative { get; set; }

        [JsonProperty("questionSetVersion")]
        public string QuestionSetVersion { get; set; }
    }
}