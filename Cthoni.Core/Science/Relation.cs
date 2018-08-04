using System;
using JetBrains.Annotations;

namespace Cthoni.Core.Science
{
    public class Relation
    {
        public Relation([NotNull] Concept from, [NotNull] Concept to)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (from == to)
            {
                throw new ArgumentException($"A concept may not be related to itself: {from.Name}");
            }

            From = from;
            To = to;
        }


        [NotNull] public Concept To { get; }


        [NotNull] public Concept From { get; }
    }
}
