
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sorter.Core.Services;

namespace Generator
{
    class Program
    {
        private class Arguments
        {
            public string InputFileName { get; set; }
            public string OutputFileName { get; set; }
            public int BatchSizeInMegabytes { get; set; }
        }

        private static Arguments ParseArguments(string[] args)
        {
            var switchMappings = new Dictionary<string, string>()
           {
               { "-inputFileName", "inputFileName" },
               { "-outputFileName", "outputFileName" },
               { "-batchSizeInMegabytes", "batchSizeInMegabytes" }
           };
            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args, switchMappings);
            var config = builder.Build();

            if (string.IsNullOrEmpty(config["inputFileName"]))
            {
                throw new ArgumentException("Обязательный параметр '-inputFileName' не установлен. Работа программы невозможна.");
            }

            if (string.IsNullOrEmpty(config["outputFileName"]))
            {
                throw new ArgumentException("Обязательный параметр '-outputFileName' не установлен. Работа программы невозможна.");
            }

            if (string.IsNullOrEmpty(config["batchSizeInMegabytes"]) || !int.TryParse(config["batchSizeInMegabytes"], out int batchSizeInMegabytes))
            {
                Console.WriteLine("Параметр -batchSizeInMegabytes' не установлен. Установлено значение по умолчанию = 100 Мб");
                batchSizeInMegabytes = 100;
            }

            return new Arguments { InputFileName = config["inputFileName"], OutputFileName = config["outputFileName"], BatchSizeInMegabytes = batchSizeInMegabytes };
        }

        static int Main(string[] args)
        {
            var arguments = ParseArguments(args);


            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("Generator.Program", LogLevel.Debug)
                    .AddFilter("Sorter.Core.Services.Processor", LogLevel.Debug)
                    .AddConsole();
            });

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddScoped<ITreeGenerator, TreeGenerator>()
                .AddScoped<ITreeDataSaver, TreeDataSaver>()
                .AddScoped<ISmallFileTree, SmallFileTree>()
                .AddScoped<IProcessor, Processor>()
                .AddScoped<ILoggerFactory>(x => loggerFactory)
                .AddSingleton(typeof(ILogger<>), typeof(Logger<>))
                .BuildServiceProvider();

            var processor = serviceProvider.GetService<IProcessor>();
            processor.Process(arguments.InputFileName, arguments.OutputFileName, arguments.BatchSizeInMegabytes);


            return 0;
        }
    }
}