using JetBrains.Annotations;

namespace Cthoni.Core.Science
{
    public interface IFactBase
    {
        void AddRelation(Concept descendant, Concept ancestor);
        [NotNull] Concept CreateConcept(string name);
        Concept FindConcept(string name, bool isThrowOnNotFound = false);
        void Reset();
    }
}
