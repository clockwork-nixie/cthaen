using System;
using System.Reflection;
using Cthoni.Utilities;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    [UsedImplicitly]
    public class CommandLine : ICommandLine
    {
        private const string NOT_FOUND = "I'm sorry, Laura; I don't understand the question.";
        private const string GOODBYE = "See you!";
        private const string OK = "OK";
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
            Exception trace = null;

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
                                Console.WriteLine(NOT_FOUND);
                                break;

                            case ResponseType.Ok:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(OK);
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

                            case ResponseType.Trace:
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine(trace);
                                break;

                            default:
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine($"Unknown response type '{response.Type}' with text: {response.Text}");
                                break;
                        }
                        trace = null;
                    }
                    catch (Exception exception)
                    {
                        trace = exception;

                        while (exception is TargetInvocationException && exception.InnerException != null)
                        {
                            exception = exception.InnerException;
                        }
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(exception.Message);
                    }
                }
            }
            while (!isFinished);
        }
    }
}
