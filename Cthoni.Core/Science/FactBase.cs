using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Cthoni.Core.CommandLine;
using JetBrains.Annotations;

namespace Cthoni.Core.Science
{
    public class FactBase : IFactBase
    {
        private class Concept
        {
            public string Name { get; set; }
        }


        [NotNull] private readonly IDictionary<string, Concept> _concepts = new ConcurrentDictionary<string, Concept>();


        public void CreateClass([NotNull] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            var safeName = name.ToLowerInvariant();
            var concept = new Concept { Name = safeName };

            try
            {
                _concepts.Add(safeName, concept);
            }
            catch (ArgumentException)
            {
                throw new NotImplementedException("Already exists.");
            }
        }
    }
}
