using System;
using Cthoni.Core.Context;
using Cthoni.Utilities;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    [UsedImplicitly]
    public class CommandLineProcessor : ICommandLineProcessor
    {
        [NotNull] private readonly ICommandSet _commandSet;


        public CommandLineProcessor([NotNull] IFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            _commandSet = factory.GetInstance<ICommandSet>();

            var builder = factory.GetInstance<ICommandLineDirectives>();
            var context = factory.GetInstance<IContext>();

            builder.Populate(_commandSet, context);
        }


        public Response Process(string command)
        {
            Response response;

            try
            {
                response = _commandSet.Process(command);
            }
            catch (BaseException exception)
            {
                response = new Response(exception.Message ?? "Unknown error.", ResponseType.Error);
            }
            return response;
        }
    }
}
