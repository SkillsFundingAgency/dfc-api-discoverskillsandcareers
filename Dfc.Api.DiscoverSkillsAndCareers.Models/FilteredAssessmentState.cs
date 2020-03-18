﻿using DFC.Api.DiscoverSkillsAndCareers.Models.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class FilteredAssessmentState : AssessmentStateBase
    {
        [JsonProperty("jobCategories")]
        public List<JobCategoryState> JobCategoryStates { get; set; } = new List<JobCategoryState>();

        [JsonIgnore]
        public override int MaxQuestions => CurrentState?.Skills.Count ?? 0;

        [JsonProperty("recordedAnswers")]
        public List<Answer> RecordedAnswers { get; set; } = new List<Answer>();

        [JsonProperty("currentFilterAssessmentCode")]
        public string CurrentFilterAssessmentCode { get; set; }

        [JsonIgnore]
        public override string QuestionSetVersion => CurrentState?.QuestionSetVersion;

        [JsonIgnore]
        public override int CurrentQuestion
        {
            get => CurrentState?.CurrentQuestion ?? 0;
            set => CurrentState?.SetCurrentQuestion(value);
        }

        [JsonIgnore]
        public override bool IsComplete
        {
            get
            {
                var complete = CurrentState?.IsComplete(RecordedAnswers);
                if (complete.HasValue && complete.Value && !CompleteDt.HasValue)
                {
                    CompleteDt = DateTime.UtcNow;
                }

                return complete.GetValueOrDefault(false);
            }
        }

        [JsonIgnore]
        public string JobFamilyNameUrlSafe => CurrentState?.JobFamilyNameUrlSafe;

        [JsonIgnore]
        public string CurrentQuestionId => CurrentState?.CurrentQuestionId;

        [JsonIgnore]
        private JobCategoryState CurrentState => JobCategoryStates.SingleOrDefault(jc => jc.JobCategoryCode.EqualsIgnoreCase(CurrentFilterAssessmentCode));

        public void CreateOrResetCategoryState(string questionSetVersion, List<Question> questions, JobCategory category)
        {
            if (!TryGetJobCategoryState(category?.Code, out var cat))
            {
                var skills = new List<JobCategorySkill>();
                var index = 1;
                foreach (var skill in category?.Skills)
                {
                    var question = questions.FirstOrDefault(q => q.TraitCode.EqualsIgnoreCase(skill.ONetAttribute));

                    if (question == null)
                    {
                        continue;
                    }

                    skills.Add(new JobCategorySkill
                    {
                        Skill = skill.ONetAttribute,
                        QuestionId = question.QuestionId,
                        QuestionNumber = index,
                    });

                    index++;
                }

                cat = new JobCategoryState(category?.Code, category?.Name, questionSetVersion, skills);
                JobCategoryStates.Add(cat);
            }

            cat.QuestionSetVersion = questionSetVersion;
        }

        public void RemoveAnswersForCategory(string jobCategoryCode)
        {
            var category = JobCategoryStates.FirstOrDefault(jc => jc.JobCategoryCode.EqualsIgnoreCase(jobCategoryCode));
            if (category != null)
            {
                RecordedAnswers = RecordedAnswers.Where(a => !category.Skills.Any(s => s.Skill.EqualsIgnoreCase(a.TraitCode))).ToList();
            }
        }

        public override int MoveToNextQuestion()
        {
            var number = CurrentState?.Skills.FirstOrDefault(q => !RecordedAnswers.Any(a => a.TraitCode.EqualsIgnoreCase(q.Skill)))?.QuestionNumber;
            CurrentQuestion = number ?? (CurrentState?.Skills.FirstOrDefault()?.QuestionNumber ?? 0);

            return CurrentQuestion;
        }

        public bool TryGetJobCategoryState(string jobCategoryCode, out JobCategoryState state)
        {
            state = JobCategoryStates.FirstOrDefault(s => s.JobCategoryCode.EqualsIgnoreCase(jobCategoryCode));
            return state != null;
        }

        public List<FilterAnswer> GetAnswersForCategory(string jobCategoryCode)
        {
            if (!TryGetJobCategoryState(jobCategoryCode, out var state))
            {
                return new List<FilterAnswer>();
            }

            var results = new List<FilterAnswer>();

            foreach (var skill in state.Skills)
            {
                var answer = RecordedAnswers.FirstOrDefault(a => a.TraitCode.EqualsIgnoreCase(skill.Skill));

                if (answer != null)
                {
                    results.Add(new FilterAnswer(skill.QuestionNumber, answer));
                }
            }

            return results;
        }

        public void AddAnswer(Answer answer)
        {
            var newAnswerSet = RecordedAnswers.Where(x => x.QuestionId != answer.QuestionId)
                                .ToList();

            newAnswerSet.Add(answer);
            RecordedAnswers = newAnswerSet;
        }
    }
}