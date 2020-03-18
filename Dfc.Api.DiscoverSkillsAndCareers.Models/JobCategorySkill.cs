using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class JobCategorySkill
    {
        public string Skill { get; set; }

        public string QuestionId { get; set; }

        public int QuestionNumber { get; set; } = 1;
    }
}