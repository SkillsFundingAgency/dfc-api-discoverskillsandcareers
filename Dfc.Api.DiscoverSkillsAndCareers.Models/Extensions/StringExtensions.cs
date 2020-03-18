using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.DiscoverSkillsAndCareers.Models.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string source, string target) =>
            source?.Equals(target, StringComparison.InvariantCultureIgnoreCase) ?? false;
    }
}