using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class FilterAnswer
    {
        public FilterAnswer(int index, Answer answer)
        {
            Index = index;
            Answer = answer;
        }

        public int Index { get; }

        public Answer Answer { get; set; }
    }
}