using System;
using Cthoni.Core.Context;
using Cthoni.Utilities;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    [UsedImplicitly]
    public class CommandLineProcessor : ICommandLineProcessor
    {
        [NotNull] private readonly IContext _context;


        public CommandLineProcessor([NotNull] IFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            _context = factory.GetInstance<IContext>();

            var directives = _context.Directives;
            var facts = _context.Facts;

            directives.Register("trace", () => ResponseType.Trace);
            directives.Register("$concept is a concept", concept => facts.CreateConcept(concept));
            directives.Register("$child is a $parent", (child, parent) => {
                if (child == null)
                {
                    throw new ArgumentNullException(nameof(child));
                }
                if (parent == null)
                {
                    throw new ArgumentNullException(nameof(parent));
                }
                var ancestor = facts.FindConcept(parent);

                if (ancestor == null)
                {
                    throw new CommandLineException("");
                }
                var concept = facts.FindConcept(child) ?? facts.CreateConcept(child);

                facts.AddRelation(concept, ancestor);
            });
            directives.Register("quit", () => new Response(ResponseType.Quit));
        }


        public Response Process(string command)
        {
            Response response;

            try
            {
                response = _context.Directives.Process(command);
            }
            catch (CommandLineException exception)
            {
                response = new Response(exception.Message ?? "Unknown error.", ResponseType.Error);
            }
            return response;
        }
    }
}
