﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class JobCategoryResult
    {
        [JsonProperty("jobFamilyCode")]
        public string JobCategoryCode => JobCategoryHelper.GetCode(JobCategoryName);

        [JsonProperty("jobFamilyName")]
        public string JobCategoryName { get; set; }

        [JsonProperty("jobFamilyText")]
        public string JobCategoryText { get; set; }

        [JsonProperty("jobFamilyUrl")]
        public string Url { get; set; }

        [JsonProperty("traitsTotal")]
        public int TraitsTotal { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("normalizedTotal")]
        public decimal NormalizedTotal { get; set; }

        public List<TraitValue> TraitValues { get; set; } = new List<TraitValue>();

        [JsonProperty("filterAssessment")]
        public FilterAssessmentResult FilterAssessmentResult { get; set; }

        [JsonProperty("totalQuestions")]
        public int TotalQuestions { get; set; }

        [JsonProperty("resultsShown")]
        public bool ResultsShown { get; set; }

        [JsonIgnore]
        public string JobCategoryNameUrl => JobCategoryName?.Replace(" ", "-", StringComparison.OrdinalIgnoreCase);
    }
}