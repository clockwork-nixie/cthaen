using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cthoni.Utilities;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    [UsedImplicitly]
    public class CommandLineConsole : ICommandLineConsole
    {
        private const string ABORT_LOAD = "Aborting load.";
        private const string GOODBYE = "See you!";
        private const string LOADING = "Loading file...";
        private const string NOT_FOUND = "I'm sorry, Laura; I don't understand the question.";
        private const string PROMPT = ">> ";
        private const string SUFFIX = ".cth";

        [NotNull] private readonly ICommandLineProcessor _processor;
        

        public CommandLineConsole([NotNull] IFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            _processor = factory.GetInstance<ICommandLineProcessor>();
        }


        public void Run()
        {
            var loading = new Queue<string>();
            var isFinished = false;
            var isSkipBlankLine = false;
            Exception trace = null;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            do
            {
                if (isSkipBlankLine)
                {
                    isSkipBlankLine = false;
                }
                else
                {
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(_processor.Context.CurrentTopic);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(PROMPT);
                Console.ForegroundColor = ConsoleColor.Green;

                string command;

                if (loading.Count == 0)
                {
                    command = Console.ReadLine();
                }
                else
                {
                    command = loading.Dequeue();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(command);
                }
                Console.WriteLine();

                if (!string.IsNullOrWhiteSpace(command))
                {
                    try
                    {
                        var response = _processor.Process(command);

                        switch (response.Type)
                        {
                            case ResponseType.Load:
                                var path = Path.Combine(
                                    AppContext.BaseDirectory ?? string.Empty, 
                                    "Scripts",
                                    response.Text);

                                if (!path.EndsWith(SUFFIX))
                                {
                                    path = path + SUFFIX;
                                }

                                foreach (var line in File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)))
                                {
                                    loading.Enqueue(line);
                                }

                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(LOADING);
                                break;

                            case ResponseType.NotFound:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(NOT_FOUND);
                                break;

                            case ResponseType.Ok:
                                isSkipBlankLine = true;
                                break;

                            case ResponseType.Quit:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(GOODBYE);
                                Console.WriteLine();
                                Console.ResetColor();
                                isFinished = true;
                                break;

                            case ResponseType.Text:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(response.Text);
                                break;

                            case ResponseType.Trace:
                                Console.ForegroundColor = ConsoleColor.Magenta;
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

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(exception.Message);

                        if (loading.Any())
                        {
                            loading.Clear();
                            Console.WriteLine();
                            Console.WriteLine(ABORT_LOAD);
                        }
                    }
                }
            }
            while (!isFinished);
        }
    }
}
