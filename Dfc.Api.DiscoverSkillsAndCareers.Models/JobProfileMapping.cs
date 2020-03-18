using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class JobProfileMapping
    {
        public string JobProfile { get; set; }

        public bool Included { get; set; }
    }
}