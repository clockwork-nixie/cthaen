using System;
using Cthoni.Core;
using Cthoni.Core.Interfaces;
using Cthoni.Interfaces;
using JetBrains.Annotations;

namespace Cthoni
{
    public class CommandLine : ICommandLine
    {
        private const string PROMPT = ">> ";

        [NotNull] private readonly IFactory _factory;
        

        public CommandLine([NotNull] IFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            _factory = factory;
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

                if (!string.IsNullOrWhiteSpace(command))
                {
                    try
                    {
                        // PROCESS
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
