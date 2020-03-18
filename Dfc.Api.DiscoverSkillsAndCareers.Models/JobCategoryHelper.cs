using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DFC.Api.DiscoverSkillsAndCareers.Models
{
    [ExcludeFromCodeCoverage]
    public static class JobCategoryHelper
    {
        public static string GetCode(string input)
        {
            var code = string.Empty;
            var words = input?.Split(' ', '-', StringSplitOptions.None);
            if (words?.Length > 1)
            {
                code = words.Aggregate(code, (current, word) => current + word.Substring(0, 1).ToUpperInvariant());
            }
            else
            {
                code = input?.Substring(0, 5).ToUpperInvariant();
            }

            return code;
        }
    }
}