using JetBrains.Annotations;

namespace Cthoni.Core.Science
{
    public interface IFactBase
    {
        void AddRelation([NotNull] Concept descendant, [NotNull] Concept ancestor);
        [NotNull] Concept CreateConcept([NotNull] string name);
        Concept FindConcept([NotNull] string name);
    }
}
