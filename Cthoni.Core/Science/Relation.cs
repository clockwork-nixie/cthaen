using System;
using JetBrains.Annotations;

namespace Cthoni.Core.Science
{
    public class Relation
    {
        public Relation([NotNull] IConcept from, [NotNull] IConcept to, Relationship relationship)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (relationship == Relationship.Unknown)
            {
                throw new ArgumentException($"Concepts cannot be related using the unknown relationship.");
            }

            if (from == to)
            {
                throw new ArgumentException($"A concept may not be related to itself: {from.Name}");
            }

            From = from;
            Relationship = relationship;
            To = to;
        }


        [NotNull] public IConcept From { get; }
        public Relationship Relationship { get; }
        [NotNull] public IConcept To { get; }
    }
}
