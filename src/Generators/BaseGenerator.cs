
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using dgen.Exceptions;
using dgen.Models;
using dgen.Services;
using dgen.Extensions;
using McMaster.Extensions.CommandLineUtils;
using System.IO;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("test")]
namespace dgen.Generators
{
    public enum GeneratorType
    {
        CLASS
    }

    public class BaseGenerator : IGenerator
    {
        private readonly IProjectDiscoveryService _projectDiscoveryService;
        private readonly IReporter _reporter;
        private readonly IFileSystem _fileSystem;
        private string currentPath;
        private ProjectDiscoveryResult projectDiscovery;

        public BaseGenerator(IFileSystem fileSystem, IProjectDiscoveryService projectDiscoveryService, IReporter reporter)
        {
            _projectDiscoveryService = projectDiscoveryService;
            projectDiscovery = _projectDiscoveryService.DiscoverProject();
            _reporter = reporter;
            _fileSystem = fileSystem;
        }

        public async Task GenerateFileAsync(GeneratorType genType, string name)
        {
            var ns = buildNamespace(name);
            var path = getPath(name);
            var fname = getFileName(name);

            if (!IsValidFilename(fname)) throw new InvalidFileName($"The given name ({fname}) is not a valid filename!");

            var generator = getTypedGenerator(genType);
            var res = generator.Generate(ns, fname);

            generateFolders(path);
            await createFile(path, res);
        }

        internal protected string buildNamespace(string name)
        {
            string ns = string.Empty;
            var rootns = projectDiscovery.RootNamespace;
            var nsItems = new List<string>();

            if (projectDiscovery.Paths.Count > 1)
            {
                var items = projectDiscovery.Paths.Select(x => x.Split(System.IO.Path.DirectorySeparatorChar).Last()).ToList();
                items.Remove(items.Last());
                items.Reverse();
                nsItems.AddRange(items);
            }

            if (name.Contains(System.IO.Path.DirectorySeparatorChar))
            {
                var items = name.Split(System.IO.Path.DirectorySeparatorChar).ToList();

                //Gibts folder go back zeichen (..)?
                var cnt = 0;
                var goBacks = items.Where(x => x.Contains(".")).ToList();

                if (goBacks.Count > nsItems.Count) throw new NoParentClassException("Can't go up in hierarchy as this would be higher then the csproj file!");

                while (cnt < goBacks.Count)
                {
                    items.Remove(items.First());
                    //Wenn es GoBacks gibt, dann müssen wir auch die Ordnerliste anpassen
                    nsItems.Remove(nsItems.Last());
                    cnt++;
                }

                items.Remove(items.Last());

                nsItems.AddRange(items);
            }

            if (nsItems.Count != 0)
            {
                if(!string.IsNullOrEmpty(rootns)) nsItems.Insert(0, rootns); //Nur den RootNamespace einfügen, wenn er nicht leer ist
                ns = string.Join(".", nsItems);
            }
            else
            {
                ns = rootns; //Wenn es keine Namespace items gibt, dann ist der RootNamespace der Namespace
                ns = String.IsNullOrEmpty(ns) ? name : ns; //Wenn der Namespace immer noch leer ist, den Klassennamen nehmen
            }

            return ns;
        }

        internal protected string getPath(string name)
        {
            var path = string.Empty;
            var items = new List<string>();
            var paths = new List<string>(projectDiscovery.Paths);

            if (name.Contains(System.IO.Path.DirectorySeparatorChar))
            {
                items = name.Split(System.IO.Path.DirectorySeparatorChar).ToList();

                //Gibts folder go back zeichen (..)?
                var cnt = 0;
                var goBacks = items.Where(x => x.Contains(".")).ToList();
                if (goBacks.Count > paths.Count) throw new NoParentClassException("Can't go up in hierarchy as this would be higher then the csproj file!");
                while (cnt < goBacks.Count)
                {
                    items.Remove(items.First());
                    paths.Remove(paths.First());
                    cnt++;
                }

                items.Remove(items.Last());
            }

            var currentPath = paths.First();
            items.Insert(0, currentPath);

            path = string.Join(System.IO.Path.DirectorySeparatorChar, items);

            return path;
        }

        internal protected string getFileName(string name)
        {
            if (name.Contains(System.IO.Path.DirectorySeparatorChar))
            {
                var items = name.Split(System.IO.Path.DirectorySeparatorChar).ToList();
                return items.Last();
            }
            else return name;
        }

        internal protected bool IsValidFilename(string testName)
        {
            Regex containsABadCharacter = new Regex("["
                  + Regex.Escape(new string(System.IO.Path.GetInvalidPathChars())) + "]");
            if (containsABadCharacter.IsMatch(testName)) { return false; };

            // other checks for UNC, drive-path format, etc

            return true;
        }

        internal protected ITypeGenerator getTypedGenerator(GeneratorType type)
        {
            var qualifiedName = $"dgen.Generators.{type.ToString().FirstCharToUpper()}Generator";
            var t = Type.GetType(qualifiedName);
            return (ITypeGenerator)Activator.CreateInstance(t);
        }

        internal protected void generateFolders(string path)
        {
            if (!_fileSystem.Directory.Exists(path))
            {
                _fileSystem.Directory.CreateDirectory(path);
            }
        }

        internal protected async Task createFile(string path, GeneratorResult generatorResult)
        {
            var fullName = _fileSystem.Path.Combine(path, generatorResult.FileName);

            if (_fileSystem.File.Exists(fullName)) throw new FileAlreadyExistsException($"The file {fullName} already exist!");

            var buffer = Encoding.ASCII.GetBytes(generatorResult.Content);
            using (var sw = _fileSystem.FileStream.Create(fullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, buffer.Length, true))
            {
                await sw.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}