using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    public class AssessmentState : AssessmentStateBase
    {
        public AssessmentState(string questionSetVersion, int maxQuestions)
        {
            QuestionSetVersion = questionSetVersion;
            MaxQuestions = maxQuestions;
        }

        [JsonProperty("questionSetVersion")]
        public override string QuestionSetVersion { get; }

        [JsonProperty("currentQuestion")]
        public override int CurrentQuestion { get; set; }

        [JsonProperty("maxQuestions")]
        public override int MaxQuestions { get; }

        [JsonProperty("recordedAnswers")]
        public List<object> RecordedAnswers { get; set; } = new List<object>();
    }
}