using DFC.Api.DiscoverSkillsAndCareers.Models.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class JobCategoryState
    {
        public JobCategoryState(string code, string name, string questionSetVersion, List<JobCategorySkill> skills, string currentQuestionId = null)
        {
            if (skills?.Count == 0)
            {
                throw new ArgumentException($"No skills were passed for job category {name}", nameof(skills));
            }

            JobCategoryCode = code;
            JobCategoryName = name;
            QuestionSetVersion = questionSetVersion;
            Skills = skills?.OrderBy(s => s.QuestionNumber).ToList();

            CurrentQuestion = !string.IsNullOrWhiteSpace(currentQuestionId) ? Skills.First(q => q.QuestionId == currentQuestionId).QuestionNumber : Skills.First().QuestionNumber;
        }

        [JsonProperty("questionSetVersion")]
        public string QuestionSetVersion { get; set; }

        [JsonProperty("questions")]
        public List<JobCategorySkill> Skills { get; set; }

        [JsonProperty("jobCategoryName")]
        public string JobCategoryName { get; set; }

        [JsonProperty("jobCategoryCode")]
        public string JobCategoryCode { get; set; }

        [JsonProperty("currentQuestion")]
        public int CurrentQuestion { get; private set; }

        [JsonIgnore]
        public string JobFamilyNameUrlSafe => JobCategoryName?.Replace(" ", "-", StringComparison.OrdinalIgnoreCase);

        [JsonIgnore]
        public string CurrentQuestionId => Skills.First(q => q.QuestionNumber == CurrentQuestion).QuestionId;

        public int UnansweredQuestions(List<Answer> answers) =>
            answers == null
                ? Skills.Count
                : Skills.Count(s => !answers.Any(a => a.TraitCode.EqualsIgnoreCase(s.Skill)));

        public bool IsComplete(List<Answer> answers) =>
            answers != null
            && Skills.All(s => answers.Any(a => s.Skill.EqualsIgnoreCase(a.TraitCode)));

        public void SetCurrentQuestion(int questionNumber)
        {
            CurrentQuestion = Skills.First(q => q.QuestionNumber == questionNumber).QuestionNumber;
        }
    }
}