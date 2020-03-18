using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class FilterAssessmentResult
    {
        [JsonProperty("jobFamilyName")]
        public string JobFamilyName { get; set; }

        [JsonProperty("createdDt")]
        public DateTime CreatedDt { get; set; }

        [JsonProperty("questionSetVersion")]
        public string QuestionSetVersion { get; set; }

        [JsonProperty("recordedAnswerCount")]
        public int RecordedAnswerCount { get; set; }

        [JsonProperty("maxQuestions")]
        public int MaxQuestions { get; set; }

        [JsonProperty("recordedAnswers")]
        public List<Answer> RecordedAnswers { get; set; } = new List<Answer>();

        [JsonProperty("suggestedJobProfiles")]
        public List<string> SuggestedJobProfiles { get; set; } = new List<string>();

        [JsonProperty("whatYouToldUs")]
        public List<string> WhatYouToldUs { get; set; } = new List<string>();

        [JsonIgnore]
        public string JobFamilyNameUrlSafe => JobFamilyName?.Replace(" ", "-", StringComparison.OrdinalIgnoreCase);
    }
}