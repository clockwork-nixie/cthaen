using System;
using JetBrains.Annotations;

namespace Cthoni.Core.Science
{
    public interface ITopic
    {
        void AddRelation(Concept descendant, Concept ancestor);
        [NotNull] Concept CreateConcept(string name);
        Concept FindConcept(string name);
        [NotNull] Concept FindConceptOrThrow(string name);
        void Initialise(string name, [NotNull] Action<string, ITopic> register);
        void Reset();
    }
}
