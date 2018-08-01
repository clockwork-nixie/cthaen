using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine.ParsePolicies
{
    public class SimpleParsePolicy : IParsePolicy
    {
        private static readonly char[] _separators = { ' ', '\n', '\r', '\t' };
        [NotNull] private readonly string _prefix;


        [UsedImplicitly]
        public SimpleParsePolicy([NotNull] string prefix = "$")
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentNullException(nameof(prefix));
            }
            _prefix = prefix;
        }


        public virtual IEnumerable<ParseToken> ParseInput(string sentence) => ParseSpecification(sentence);


        public virtual IEnumerable<ParseToken> ParseSpecification(string sentence) => sentence
            .Split(_separators, StringSplitOptions.RemoveEmptyEntries)
            .Where(t => t != null)
            .Select(t => new ParseToken { IsParameter = t.StartsWith(_prefix), Text = t.StartsWith(_prefix)? t.Substring(1): t });
    }
}
