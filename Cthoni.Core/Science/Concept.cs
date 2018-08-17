using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Cthoni.Core.Science
{
    public class Concept : IConcept
    {
        public Concept([NotNull] string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Name = name;
            Relations = new List<Relation>();
        }


        [NotNull] public string Name { get; }
        [NotNull] public IList<Relation> Relations { get; }


        public bool IsDescendantOf([NotNull] IConcept ancestor)
        {
            var cache = new HashSet<IConcept>();
            var stack = new Stack<IConcept>();
            IConcept candidate = this;
            var result = false;

            while (candidate != null)
            {
                if (!cache.Contains(candidate))
                {
                    if (candidate == ancestor)
                    {
                        result = true;
                        break;
                    }
                    cache.Add(candidate);

                    foreach (var target in candidate.Relations.Where(r => r.Relationship == Relationship.Parent).Select(r => r.To))
                    {
                        stack.Push(target);
                    }
                }
                
                candidate = stack.Count == 0? null: stack.Pop();
            }
            return result;
        }
    }
}