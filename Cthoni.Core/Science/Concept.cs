using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Cthoni.Core.Science
{
    public class Concept
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


        public bool IsDescendantOf([NotNull] Concept ancestor)
        {
            var cache = new HashSet<Concept>();
            var stack = new Stack<Concept>();
            var candidate = this;
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

                    foreach (var target in candidate.Relations.Select(r => r.To))
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