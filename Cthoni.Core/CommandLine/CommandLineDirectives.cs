using System;
using System.Linq;
using Cthoni.Core.Context;
using Cthoni.Core.Science;
using Cthoni.Utilities;
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

            // Administration
            commandSet.Register("load $filename", filename => new Response(filename, ResponseType.Load));
            commandSet.Register("quit", () => new Response(ResponseType.Quit));
            commandSet.Register("reset", () => context.CurrentTopic.Reset());
            commandSet.Register("trace", () => ResponseType.Trace);

            // Topics and concepts
            commandSet.Register("create topic $name", name => { context.CreateTopic(name); });
            commandSet.Register("clear topic", () => context.SetTopic(string.Empty));
            commandSet.Register("set topic $name", name => context.SetTopic(name));

            commandSet.Register("define $name", name => context.CurrentTopic.CreateConcept(name));
            commandSet.Register("define $child as a type of $parent", (child, parent) =>
                context.CurrentTopic.AddRelation(
                    context.CurrentTopic.CreateConcept(child),
                    context.CurrentTopic.FindConceptOrThrow(parent),
                    Relationship.Parent));
            commandSet.Register("define $unit as a unit of $measurable", (unit, measurable) =>
                 context.CurrentTopic.AddRelation(
                    context.CurrentTopic.CreateConcept(unit),
                    context.CurrentTopic.FindConceptOrThrow(measurable),
                    Relationship.Unit));

            commandSet.Register("$child is a type of $parent", (child, parent) =>
                context.CurrentTopic.AddRelation(
                    context.CurrentTopic.FindConceptOrThrow(child),
                    context.CurrentTopic.FindConceptOrThrow(parent),
                    Relationship.Parent));

            commandSet.Register("is $child a type of $parent", (child, parent) => {
                var topic = context.CurrentTopic;
                var descendant = topic.FindConceptOrThrow(child);
                var ancestor = topic.FindConceptOrThrow(parent);

                return new Response(descendant.IsDescendantOf(ancestor)? "Yes": "No");
            });

            commandSet.Register("$unit is a unit of $measurable", (unit, measurable) =>
                 context.CurrentTopic.AddRelation(
                    context.CurrentTopic.FindConceptOrThrow(unit),
                    context.CurrentTopic.FindConceptOrThrow(measurable),
                    Relationship.Unit));
        }
    }
}
