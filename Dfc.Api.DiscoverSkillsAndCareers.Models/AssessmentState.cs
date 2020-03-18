using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class AssessmentState : AssessmentStateBase
    {
        private int currentQuestion;

        public AssessmentState(string questionSetVersion, int maxQuestions)
        {
            QuestionSetVersion = questionSetVersion;
            MaxQuestions = maxQuestions;
        }

        [JsonProperty("questionSetVersion")]
        public override string QuestionSetVersion { get; }

        [JsonProperty("currentQuestion")]
        public override int CurrentQuestion
        {
            get => currentQuestion;
            set => currentQuestion = Math.Min(value, MaxQuestions);
        }

        [JsonProperty("maxQuestions")]
        public override int MaxQuestions { get; }

        [JsonIgnore]
        public override bool IsComplete
        {
            get
            {
                var complete = RecordedAnswers.Count >= MaxQuestions;

                if (complete && !CompleteDt.HasValue)
                {
                    CompleteDt = DateTime.UtcNow;
                }

                return complete;
            }
        }

        [JsonProperty("recordedAnswers")]
        public List<Answer> RecordedAnswers { get; set; } = new List<Answer>();

        public override int MoveToNextQuestion()
        {
            if (RecordedAnswers.Count >= CurrentQuestion)
            {
                CurrentQuestion = FindNextQuestion();
                return CurrentQuestion;
            }

            CurrentQuestion = FindNextUnansweredQuestion();
            return CurrentQuestion;
        }

        public void AddAnswer(Answer answer)
        {
            var newAnswerSet = RecordedAnswers.Where(x => x.QuestionId != answer.QuestionId).ToList();

            newAnswerSet.Add(answer);
            RecordedAnswers = newAnswerSet;
        }

        private int FindNextUnansweredQuestion()
        {
            for (var i = 1; i <= MaxQuestions; i++)
            {
                if (RecordedAnswers.All(x => x.QuestionNumber != i))
                {
                    return i;
                }
            }

            return MaxQuestions;
        }

        private int FindNextQuestion()
        {
            return Math.Min(CurrentQuestion + 1, MaxQuestions);
        }
    }
}