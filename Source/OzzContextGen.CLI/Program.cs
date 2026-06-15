using OzzContextGen.Core;
using OzzContextGen.i18n;

namespace OzzContextGen.CLI
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            // Kullanıcı hiçbir şey yazmadıysa veya yardım istediye kılavuzu göster
            if (args.Length == 0 || args[0] == "--help" || args[0] == "-h")
            {
                ShowHelp();
                return 0;
            }

            string source = string.Empty;
            string output = string.Empty;
            string config = string.Empty;
            bool noHistory = false;

            // 1. Parametreleri Döngüyle Ayrıştırma (Parsing)
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "--source":
                        case "-s":
                            source = args[++i];
                            break;
                        case "--output":
                        case "-o":
                            output = args[++i];
                            break;
                        case "--config":
                        case "-c":
                            config = args[++i];
                            break;
                        case "--nohistory":
                        case "-n":
                            noHistory = true;
                            break;
                        default:
                            throw new ArgumentException($"{LocalizedStrings.UnknownParameter}: {args[i]}");
                    }
                }

                // Zorunlu parametre kontrolleri
                if (string.IsNullOrEmpty(source)) throw new ArgumentException($"{LocalizedStrings.MissingParameter}: --source (-s)");
                if (string.IsNullOrEmpty(output)) throw new ArgumentException($"{LocalizedStrings.MissingParameter}: --output (-o)");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[X] {LocalizedStrings.ParameterError}: {ex.Message}\n");
                Console.ResetColor();
                ShowHelp();
                return 1;
            }

            // 2. Ana İş Mantığının Çalıştırılması
            try
            {
                Console.WriteLine($"{LocalizedStrings.Starting}\n");

                var packer = new PackerEngine();

                Console.WriteLine($"[1/2] {LocalizedStrings.ScanningSource}: {source}");

                // Core motorumuzu tetikliyoruz
                string markdownResult = await packer.PackSourceCodeAsync(source, message =>
                {
                    Console.WriteLine($" > {message}");
                });

                // Markdown dosyasını diske yaz
                Console.WriteLine($"\n[2/2] {LocalizedStrings.GeneratingMarkdown}: {output}");
                string? outputDir = Path.GetDirectoryName(output);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
                await File.WriteAllTextAsync(output, markdownResult, System.Text.Encoding.UTF8);

                if (!noHistory && !string.IsNullOrEmpty(config))
                {
                    Console.WriteLine($"\n[+] {LocalizedStrings.SavingProfile}: {config}");
                    // StateService entegrasyonu buraya gelecek
                }

                Console.WriteLine($"\n[✓] {LocalizedStrings.OperationCompleted}!");
                return 0;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[X] {LocalizedStrings.SystemError}: {ex.Message}");
                Console.ResetColor();
                return 1;
            }
        }

        /// <summary>
        /// Ezber sevmeyenler için otomatik şık yardım menüsü
        /// </summary>
        static void ShowHelp()
        {
            Console.WriteLine("==========================================================================");
            Console.WriteLine($" {LocalizedStrings.HelpTitle}");
            Console.WriteLine("==========================================================================");
            Console.WriteLine($"{LocalizedStrings.Usage}:");
            Console.WriteLine($"  {LocalizedStrings.UsageExample01}\n");
            Console.WriteLine($"{LocalizedStrings.RequiredParameters}:");
            Console.WriteLine($"  -s, --source        {LocalizedStrings.SourceDescription}.");
            Console.WriteLine($"  -o, --output        {LocalizedStrings.OutputDescription}.\n");
            Console.WriteLine($"{LocalizedStrings.OptionalParameters}:");
            Console.WriteLine($"  -c, --config        {LocalizedStrings.ConfigDescription}.");
            Console.WriteLine($"  -n, --nohistory     {LocalizedStrings.NoHistoryDescription}.");
            Console.WriteLine($"  -h, --help          {LocalizedStrings.HelpDescription}.\n");
            Console.WriteLine($"{LocalizedStrings.ExampleUsage}:");
            Console.WriteLine("  OzzContextGen -s \"C:\\Projects\\MyApp\" -o \"C:\\LLM\\context.md\" -c \"myapp.ctxgen\"");
            Console.WriteLine("==========================================================================");
        }
    }
}
