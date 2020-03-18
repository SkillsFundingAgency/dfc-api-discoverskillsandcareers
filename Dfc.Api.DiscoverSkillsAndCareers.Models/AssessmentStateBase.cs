﻿using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public abstract class AssessmentStateBase
    {
        [JsonProperty("questionSetVersion")]
        public abstract string QuestionSetVersion { get; }

        [JsonProperty("currentQuestion")]
        public abstract int CurrentQuestion { get; set; }

        [JsonProperty("maxQuestions")]
        public abstract int MaxQuestions { get; }

        [JsonIgnore]
        public int PercentageComplete =>
            (int)(((CurrentQuestion - 1M) / (decimal)MaxQuestions) * 100M);

        [JsonIgnore]
        public abstract bool IsComplete { get; }

        [JsonProperty("completeDt")]
        public DateTime? CompleteDt { get; set; }

        public abstract int MoveToNextQuestion();
    }
}