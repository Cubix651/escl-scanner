using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Escl;

namespace EsclScannerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run(host: args[0]).Wait();
        }

        private async Task Run(string host)
        {
            var scanner = new Scanner(host);
            var capabilities = await scanner.GetCapabilities();
            Console.WriteLine($"Scanner {capabilities.Model}");
            var status = await scanner.GetStatus();
            Console.WriteLine($"Version: {status.Version}");
            Console.WriteLine($"State: {status.State}");
            var options = new ScanOptions();
            options.DocumentFormatExt = SelectOption("Select document format",
                capabilities.DocumentFormatExtensions);
            options.OutputPath = SelectString("Select path", "scan.jpg");
            options.Resolution = SelectOption("Select resolution", capabilities.Resolutions);
            options.XOffset = SelectInt("Select x offset", 0);
            options.YOffset = SelectInt("Select y offset", 0);
            options.Width = SelectInt("Select width", capabilities.MaxWidth);
            options.Height = SelectInt("Select height", capabilities.MaxHeight);
            await scanner.Scan(options);
        }

        private T SelectOption<T>(string description, IList<T> options)
        {
            int index = 0;
            foreach(var option in options)
            {
                Console.WriteLine($"{index}. {option}");
                ++index;
            }
            Console.Write($"{description} (default 0): ");
            var choiceString = Console.ReadLine();
            if(int.TryParse(choiceString, out int choice))
                return options[choice];
            return options[0];
        }

        private string SelectString(string description, string defaultValue)
        {
            Console.Write($"{description} (default {defaultValue}): ");
            var choiceString = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(choiceString))
                return defaultValue;
            return choiceString;
        }

        private int SelectInt(string description, int defaultValue)
        {
            Console.Write($"{description} (default {defaultValue}): ");
            var choiceString = Console.ReadLine();
            if(int.TryParse(choiceString, out int choice))
                return choice;
            return defaultValue;
        }
    }
}
