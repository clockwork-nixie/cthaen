using System;
using JetBrains.Annotations;

namespace Cthoni.Core.Science
{
    public interface ITopic
    {
        void AddRelation(IConcept descendant, IConcept ancestor, Relationship relationship);
        [NotNull] IConcept CreateConcept(string name);
        IConcept FindConcept(string name);
        [NotNull] IConcept FindConceptOrThrow(string name);
        void Initialise(string name, [NotNull] Action<string, ITopic> register);
        void Reset();
    }
}
