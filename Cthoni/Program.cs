﻿using System;
using Cthoni.Core.Interfaces;
using Cthoni.Interfaces;
using JetBrains.Annotations;
using NLog;


namespace Cthoni
{

    public static class Program
    {
        // ReSharper disable once AssignNullToNotNullAttribute
        [NotNull] private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();


        [NotNull] public static IFactory Factory { get; set; } = DependencyInjection.CreateFactory();


        public static void Main()
        {
            try
            {
                Factory.GetInstance<ICommandLine>().Run();
            }
            catch (Exception exception)
            {
                _logger.Fatal(exception, "Unhandled top-level exception.");
            }
        }
    }
}
