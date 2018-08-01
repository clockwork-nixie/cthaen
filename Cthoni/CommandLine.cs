using System;
using Cthoni.Core;
using Cthoni.Core.CommandLine;
using Cthoni.Core.DependencyInjection;
using JetBrains.Annotations;

namespace Cthoni
{
    [UsedImplicitly]
    public class CommandLine : ICommandLine
    {
        private const string PROMPT = ">> ";

        [NotNull] private readonly ICommandLineProcessor _processor;
        

        public CommandLine([NotNull] IFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            _processor = factory.GetInstance<ICommandLineProcessor>();
        }


        public void Run()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                Console.Write(PROMPT);
                Console.ForegroundColor = ConsoleColor.Green;

                var command = Console.ReadLine();

                Console.WriteLine();

                if (!string.IsNullOrWhiteSpace(command))
                {
                    try
                    {
                        var response = _processor.Process(command);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(response);
                    }
                    catch (CommandLineException exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(exception.Message);
                    }
                    catch (Exception exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(exception);
                    }
                }
            }
        }
    }
}
