
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using dgen.Exceptions;
using dgen.Models;

namespace dgen.Services {

    public class ProjectDiscoveryService : IProjectDiscoveryService {
        private readonly IFileSystem _fileSystem;

        public ProjectDiscoveryService (IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public ProjectDiscoveryResult DiscoverProject(string path)
        {
            var csproj = findSln(path);
            var newPath = path;
            var list = new List<string>{ path };

            while(String.IsNullOrEmpty(csproj)){
                //Im Parent Folder suchen
                if(isRoot(newPath)) return new ProjectDiscoveryResult { Paths = list };

                newPath = _fileSystem.Directory.GetParent(newPath).FullName;
                csproj = findSln(newPath);
                list.Add(newPath);
            }

            var rootNamespace = csproj.Split('.').First();
            return new ProjectDiscoveryResult { Paths = list, RootNamespace = rootNamespace, ProjPath = csproj };
        }

        private string findSln(string path){
            var csprojs = _fileSystem.Directory.GetFiles(path, "*.csproj");
            
            if (csprojs.Length == 1) return _fileSystem.Path.GetFullPath(csprojs[0]);
            if (csprojs.Length > 1) throw new CommandValidationException($"Es gibt mehrere *.csproj Dateien in diesem Ordner {path}!");
            return "";
        }

        private bool isRoot(string current){
            var root = _fileSystem.Directory.GetDirectoryRoot(current);
            return root == current ? true : false;
        }
    }
}