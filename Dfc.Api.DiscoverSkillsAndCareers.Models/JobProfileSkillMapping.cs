using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public class JobProfileSkillMapping
    {
        public string ONetAttribute { get; set; }

        public List<JobProfileMapping> JobProfiles { get; set; } = new List<JobProfileMapping>();
    }
}