using System;

namespace DFC.Api.DiscoverSkillsAndCareers.Models.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string source, string target) =>
            source?.Equals(target, StringComparison.InvariantCultureIgnoreCase) ?? false;
    }
}