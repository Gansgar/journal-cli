using System;
using System.Text;
using JetBrains.Annotations;
using JournalCli.Library;
using JournalCli.Library.Infrastructure;
using JournalCli.Library.Parameters;

namespace JournalCli.GenericCli
{
    public abstract class CommandBase
    {
        private const string Error = "Journal location was not provided and no default location exists. One or the other is required";
        private readonly UserSettings _settings;
        private readonly IEncryptedStore<UserSettings> _encryptedStore;
        private readonly ILocationParameter _locationParameter;

        protected CommandBase(ILocationParameter locationParameter)
        {
            _locationParameter = locationParameter;
            _encryptedStore = EncryptedStoreFactory.Create<UserSettings>();
            _settings = UserSettings.Load(_encryptedStore);

            if (string.IsNullOrWhiteSpace(_locationParameter.Location))
            {
                if (string.IsNullOrEmpty(_settings.DefaultJournalRoot))
                    throw new InvalidOperationException(Error);

                _locationParameter.Location = _settings.DefaultJournalRoot;
            }
        }

        [UsedImplicitly]
        public abstract int Run();

        protected int Choice(string header, params string[] choices)
        {
            int index;
            while (true)
            {
                Console.ForegroundColor = Console.BackgroundColor;
                Console.BackgroundColor = Console.ForegroundColor;

                Console.WriteLine(header);

                var builder = new StringBuilder();
                for (var i = 0; i < choices.Length; i++) builder.Append($"{choices[i]} [{i}]  ");

                Console.Write(builder.ToString().Trim());
                Console.ResetColor();
                Console.Write(" ");
                var result = Console.ReadKey();
                Console.WriteLine();

                if (int.TryParse(result.KeyChar.ToString(), out index) && index < choices.Length)
                    break;

                Console.WriteLine("Please enter a number associated with your desired choice.");
            }

            return index;
        }

        protected void WriteWarning(string warning)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"WARNING: {warning}");
            Console.ResetColor();
        }

        protected bool YesOrNo(string question)
        {
            ConsoleKeyInfo result;
            while (true)
            {
                Console.ForegroundColor = Console.BackgroundColor;
                Console.BackgroundColor = Console.ForegroundColor;
                Console.Write($"{question} (y/n)");
                Console.ResetColor();
                Console.Write(" ");
                result = Console.ReadKey();
                Console.WriteLine();

                if (result.Key == ConsoleKey.Y || result.Key == ConsoleKey.N)
                    break;

                Console.WriteLine("Please type either 'y' or 'n'.");
            }

            return result.Key == ConsoleKey.Y;
        }

        private void ShowSplashScreen(string message)
        {
            var logo = $@"
       __                             ___________ 
      / /___  __  ___________  ____ _/ / ____/ (_)
 __  / / __ \/ / / / ___/ __ \/ __ `/ / /   / / / 
/ /_/ / /_/ / /_/ / /  / / / / /_/ / / /___/ / /  
\____/\____/\__,_/_/  /_/ /_/\__,_/_/\____/_/_/   
{message}                                                  
";
            Console.WriteLine(logo);
        }
    }
}