using System;
using Cthoni.Utilities;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    [UsedImplicitly]
    public class CommandLine : ICommandLine
    {
        private const string GOODBYE = "See you!";
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
            var isFinished = false;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            do
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

                        switch (response.Type)
                        {
                            case ResponseType.NotFound:
                            case ResponseType.Error:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(response.Text);
                                break;

                            case ResponseType.Quit:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(GOODBYE);
                                isFinished = true;
                                break;

                            case ResponseType.Text:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(response.Text);
                                break;

                            default:
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine($"Unknown response type '{response.Type}' with text: {response.Text}");
                                break;
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(exception);
                    }
                }
            }
            while (!isFinished);
        }
    }
}
