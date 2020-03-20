using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CommandLine;

namespace JournalCli.GenericCli
{
    internal class Program
    {
        private static int Main(string[] args)
        {
#if DEBUG
            if (Debugger.IsAttached)
                return RunDebugLoop();
#endif
            return RunProgram(args);
        }

#if DEBUG
        private static int RunDebugLoop()
        {
            Console.WriteLine("Enter command:");

            while (true)
            {
                var args = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var result = RunProgram(args);
                Console.WriteLine("Last exit code: " + result);
                Console.WriteLine();
                Console.WriteLine("Enter command:");
            }
        }
#endif
        private static int RunProgram(IEnumerable<string> args)
        {
            try
            {
                var optionTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(JournalOptionsBase));
                var parserResult = Parser.Default.ParseArguments(args, optionTypes.ToArray());

                var commandType = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(JournalCommandBase))
                    .Single(t => t.GetConstructor(new[] { parserResult.TypeInfo.Current }) != null);

                return parserResult.MapResult(options =>
                {
                    var command = (dynamic)Activator.CreateInstance(commandType, options);
                    return command.Run();
                }, errs => 1);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {e.Message}");
                Console.ResetColor();
                return 1;
            }
        }
    }
}
