using System.Collections.Generic;
using JetBrains.Annotations;

namespace Cthoni.Core.Science
{
    public class Concept
    {
        public Concept([NotNull] string name)
        {
            Name = name;
            Relations = new List<Relation>();
        }


        [NotNull] public string Name { get; }
        [NotNull] public IList<Relation> Relations { get; }
    }
}