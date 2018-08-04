using System;
using Cthoni.Core.Context;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    [UsedImplicitly]
    public class CommandLineDirectives : ICommandLineDirectives
    {
        public void Populate(ICommandSet commandSet, IContext context)
        {
            if (commandSet == null)
            {
                throw new ArgumentNullException(nameof(commandSet));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var facts = context.Facts;

            // Administration
            commandSet.Register("load $filename", filename => new Response(filename, ResponseType.Load));
            commandSet.Register("quit", () => new Response(ResponseType.Quit));
            commandSet.Register("reset", () => facts.Reset());
            commandSet.Register("trace", () => ResponseType.Trace);
            
            // Facts
            commandSet.Register("$concept is a concept", concept => facts.CreateConcept(concept));

            commandSet.Register("$child is a type of $parent", (child, parent) => {
                var ancestor = facts.FindConcept(parent, true);
                var descendant = facts.FindConcept(child) ?? facts.CreateConcept(child);

                facts.AddRelation(descendant, ancestor);
            });
        }
    }
}
