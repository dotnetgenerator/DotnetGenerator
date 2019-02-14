
using System.Runtime.CompilerServices;
using dgen.Models;

[assembly: InternalsVisibleTo("test")]
namespace dgen.Generators {

    public class ClassGenerator : ITypeGenerator{

        const string PREFIX = "cs";

        public GeneratorResult Generate(string ns, string name){
            var res = new GeneratorResult();

            res.FileName = $"{name}.{PREFIX}";
            res.Content = createContent(ns, name);

            return res;
        }

        internal protected string createContent(string ns, string name){
            var content = string.Empty;

            content += System.Environment.NewLine;
            content += $"namespace {ns}";
            content += System.Environment.NewLine;
            content += "{";
            content += System.Environment.NewLine;
            content += $"\tpublic class {name}";
            content += System.Environment.NewLine;
            content += "\t{";
            content += System.Environment.NewLine;
            content += "\t}";
            content += System.Environment.NewLine;
            content += "}";

            return content;
        }
    }
}