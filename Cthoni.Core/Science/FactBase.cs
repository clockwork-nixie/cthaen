using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Cthoni.Core.CommandLine;
using Cthoni.Utilities;
using JetBrains.Annotations;

namespace Cthoni.Core.Science
{
    [UsedImplicitly]
    public class FactBase : IFactBase
    {
        [NotNull] private readonly IDictionary<string, Concept> _concepts = new ConcurrentDictionary<string, Concept>();
        [NotNull] private readonly ConcurrentDictionary<string, Relation> _relations = new ConcurrentDictionary<string, Relation>();


        public void AddRelation(Concept descendant, Concept ancestor)
        {
            if (descendant == null)
            {
                throw new ArgumentNullException(nameof(descendant));
            }

            if (ancestor == null)
            {
                throw new ArgumentNullException(nameof(ancestor));
            }
            var relation = new Relation(descendant, ancestor);
            var key = (string.Compare(descendant.Name, ancestor.Name, StringComparison.InvariantCultureIgnoreCase) > 0)?
                $"{ancestor.Name}\0${descendant.Name}":
                $"{descendant.Name}\0${ancestor.Name}";
            
            // Needs lock and backout
            if (_relations.TryAdd(key, relation))
            {
                descendant.Relations.Add(relation);
                ancestor.Relations.Add(relation);
            }
            else
            {
                throw new StateException($"Relation between {descendant.Name} and {ancestor.Name} already exists.");
            }
        }


        public Concept CreateConcept(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var safeName = name.ToLowerInvariant();
            var concept = new Concept(safeName);

            try
            {
                _concepts.Add(safeName, concept);
            }
            catch (ArgumentException)
            {
                throw new StateException("Concept already exists.");
            }
            return concept;
        }


        public Concept FindConcept(string name, bool isThrowOnNotFound)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var safeName = name.ToLowerInvariant();
            Concept concept;

            _concepts.TryGetValue(safeName, out concept);

            if (isThrowOnNotFound && concept == null)
            {
                throw new StateException($"Concept does not exist: {name}");
            }
            return concept;
        }


        public void Reset()
        {
            _concepts.Clear();
            _relations.Clear();
        }
    }
}
