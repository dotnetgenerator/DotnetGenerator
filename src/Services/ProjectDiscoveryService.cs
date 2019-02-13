
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.CompilerServices;
using dgen.Exceptions;
using dgen.Models;

[assembly: InternalsVisibleTo("test")]
namespace dgen.Services {

    public class ProjectDiscoveryService : IProjectDiscoveryService {
        private readonly IFileSystem _fileSystem;

        public ProjectDiscoveryService (IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public ProjectDiscoveryResult DiscoverProject(string path = "")
        {

            var newPath = !string.IsNullOrEmpty(path) ? path : _fileSystem.Directory.GetCurrentDirectory();
            var csproj = findSln(newPath);
            var list = new List<string>{ newPath };

            while(String.IsNullOrEmpty(csproj)){
                //Im Parent Folder suchen
                if(isRoot(newPath)) return new ProjectDiscoveryResult { Paths = list };

                newPath = _fileSystem.Directory.GetParent(newPath).FullName;
                csproj = findSln(newPath);
                list.Add(newPath);
            }

            var fi = _fileSystem.FileInfo.FromFileName(csproj);
            var rootNamespace = fi.Name.Split('.').First();
            return new ProjectDiscoveryResult { Paths = list, RootNamespace = rootNamespace, ProjPath = csproj };
        }

        internal protected string findSln(string path){
            var csprojs = _fileSystem.Directory.GetFiles(path, "*.csproj");

            if (csprojs.Length == 1) return _fileSystem.Path.GetFullPath(csprojs[0]);

            if (csprojs.Length > 1) throw new CommandValidationException($"Es gibt mehrere *.csproj Dateien in diesem Ordner {path}!");
            return "";
        }

        internal protected bool isRoot(string current){
            var root = _fileSystem.Directory.GetDirectoryRoot(current);
            return root == current ? true : false;
        }
    }
}