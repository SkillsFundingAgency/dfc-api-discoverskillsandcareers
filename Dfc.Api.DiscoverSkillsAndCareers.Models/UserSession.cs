﻿using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class UserSession
    {
        [JsonIgnore]
        public string PrimaryKey => $"{PartitionKey}-{UserSessionId}";

        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }

        [JsonProperty("id")]
        public string UserSessionId { get; set; }

        [JsonProperty("languageCode")]
        public string LanguageCode { get; set; }

        [JsonProperty("salt")]
        public string Salt { get; set; }

        [JsonProperty("assessmentState")]
        public AssessmentState AssessmentState { get; set; }

        [JsonProperty("filteredAssessmentState")]
        public FilteredAssessmentState FilteredAssessmentState { get; set; }

        [JsonProperty("resultData")]
        public ResultData ResultData { get; set; }

        [JsonProperty("startedDt")]
        public DateTime StartedDt { get; set; }

        [JsonProperty("assessmentType")]
        public string AssessmentType { get; set; }

        [JsonProperty("lastUpdatedDt")]
        public DateTime LastUpdatedDt { get; set; }

        [JsonProperty("completeDt")]
        public DateTime? CompleteDt => CurrentState.CompleteDt;

        [JsonIgnore]
        public bool IsComplete => CurrentState.IsComplete;

        [JsonIgnore]
        public int MaxQuestions => CurrentState.MaxQuestions;

        [JsonIgnore]
        public AssessmentStateBase CurrentState =>
            (FilteredAssessmentState == null
             || string.IsNullOrWhiteSpace(FilteredAssessmentState.CurrentFilterAssessmentCode))
             || !AssessmentState.IsComplete ? (AssessmentStateBase)AssessmentState : (AssessmentStateBase)FilteredAssessmentState;

        [JsonIgnore]
        public int CurrentQuestion => CurrentState.CurrentQuestion;

        [JsonIgnore]
        public string CurrentQuestionSetVersion => CurrentState.QuestionSetVersion;

        [JsonIgnore]
        public bool IsFilterAssessment => CurrentState is FilteredAssessmentState;
    }
}