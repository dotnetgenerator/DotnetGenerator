
using System.IO.Abstractions;
using System.Linq;
using dgen.Models;
using dgen.Services;
using McMaster.Extensions.CommandLineUtils;

namespace dgen.Generators
{

    public class BaseGenerator : IGenerator
    {
        private readonly IProjectDiscoveryService _projectDiscoveryService;
        private readonly IReporter _reporter;
        private string currentPath;
        private ProjectDiscoveryResult projectDiscovery;

        public BaseGenerator(IProjectDiscoveryService projectDiscoveryService, IReporter reporter)
        {
            _projectDiscoveryService = projectDiscoveryService;
            _reporter = reporter;
        }

        public virtual void GenerateFile(string name){
            projectDiscovery = _projectDiscoveryService.DiscoverProject("path");
            buildNamespace(name);
        }

        protected string buildNamespace(string name){
            string ns = string.Empty;

            if(name.Contains("/")){
                var items = name.Split('/');
                ns = projectDiscovery.RootNamespace + string.Join(".", items);
            } else {
                if(projectDiscovery.Paths.Count > 1){

                } else {
                    ns = projectDiscovery.RootNamespace;
                }
            }
            return ns;
        }

        // public static IGenerator GetGenerator(string Type)
        // {
        //     switch (Type)
        //     {
        //         case "c":
        //         case "class":
        //             return new ClassGenerator();
        //         default:
        //             return null;
        //     }
        // }
    }
}