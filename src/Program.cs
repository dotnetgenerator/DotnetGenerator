using System;
using System.ComponentModel.DataAnnotations;
using System.IO.Abstractions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using dgen.Generators;
using dgen.Services;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("test")]
namespace dgen
{

    [Command(
        Name = "dgen",
        FullName = "A .NET Core global to generate csharp files")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    class Program : CommandBase
    {
        private readonly IFileSystem _fileSystem;
        private readonly IReporter _reporter;
        private readonly IProjectDiscoveryService _projectDiscoveryService;
        private readonly IGenerator _generator;

        [Argument(0, Description = "Describes what to generate c/class for example...")]
        [Required]
        public string Type { get; set; }

        [Argument(1, Description = "Filename to generate")]
        [Required]
        public string Name { get; set; }

        static int Main(string[] args)
        {
            using (var services = new ServiceCollection()
                .AddSingleton<IConsole, PhysicalConsole>()
                .AddSingleton<IReporter>(provider => new ConsoleReporter(provider.GetService<IConsole>()))
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<IProjectDiscoveryService, ProjectDiscoveryService>()
                .AddSingleton<IGenerator, BaseGenerator>()
                .BuildServiceProvider())
            {

                var app = new CommandLineApplication<Program>
                {
                    ThrowOnUnexpectedArgument = false
                };
                app.Conventions.UseDefaultConventions().UseConstructorInjection(services);

                return app.Execute(args);
            }

            //Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        public Program(IFileSystem fileSystem,
                    IReporter reporter,
                    IProjectDiscoveryService projectDiscoveryService,
                    IGenerator generator)
        {
            _fileSystem = fileSystem;
            _reporter = reporter;
            _projectDiscoveryService = projectDiscoveryService;
            _generator = generator;
        }

        public async Task<int> OnExecute(CommandLineApplication app, IConsole console){
            try
            {
                if(!checkArguments(app)) return 1;

                var path = _fileSystem.Directory.GetCurrentDirectory();
                _reporter.Output($"Aktueller Ordner: {path}");

                // var csproj = _projectDiscoveryService.DiscoverProject("C:\\dev\\js\\waas\\demoapp");
                // if (string.IsNullOrEmpty(csproj.ProjPath)) {
                //     _reporter.Output("Es wurde keine csproj-Datei gefunden werden!");
                //     return 0;
                // }

                _generator.GenerateFile(GeneratorType.CLASS, Name);

                return 0;
            }
            catch (Exceptions.CommandValidationException e)
            {
                _reporter.Error(e.Message);
                return 1;
            }
        }

        private bool checkArguments(CommandLineApplication app){
            var argumentsOk = false;

                if(!argumentsOk && ( Type == "c" || Type == "class" )){
                    argumentsOk = true;
                }

                if(!argumentsOk){
                    app.ShowHelp();
                    return false;
                }

                return true;
        }

        public static string GetVersion() => typeof(Program)
            .Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;
    }
}
