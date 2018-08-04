using System.Collections.Generic;
using JetBrains.Annotations;

namespace Cthoni.Core.Context
{
    public interface IParsePolicy
    {
        [NotNull, ItemNotNull] IEnumerable<ParseToken> ParseInput([NotNull] string sentence);
        [NotNull, ItemNotNull] IEnumerable<ParseToken> ParseSpecification([NotNull] string sentence);
    }
}
