using Newtonsoft.Json;
using System;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    public abstract class AssessmentStateBase
    {
        [JsonProperty("questionSetVersion")]
        public abstract string QuestionSetVersion { get; }

        [JsonProperty("currentQuestion")]
        public abstract int CurrentQuestion { get; set; }

        [JsonProperty("maxQuestions")]
        public abstract int MaxQuestions { get; }

        [JsonProperty("completeDt")]
        public DateTime? CompleteDt { get; set; }
    }
}