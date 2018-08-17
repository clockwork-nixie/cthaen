using System.Collections.Generic;
using JetBrains.Annotations;

namespace Cthoni.Core.Science
{
    public interface IConcept
    {
        [NotNull] string Name { get; }
        [NotNull] IList<Relation> Relations { get; }


        bool IsDescendantOf([NotNull] IConcept ancestor);
    }
}