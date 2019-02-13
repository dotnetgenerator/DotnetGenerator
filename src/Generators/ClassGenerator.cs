
using dgen.Models;

namespace dgen.Generators {

    public class ClassGenerator : ITypeGenerator{

        const string PREFIX = "cs";

        public GeneratorResult Generate(string ns, string name){
            var res = new GeneratorResult();

            res.FileName = $"{name}.{PREFIX}";
            res.Content = createContent(ns, name);

            return res;
        }

        private string createContent(string ns, string name){
            var content = string.Empty;

            content += System.Environment.NewLine;
            content += $"namespace {ns}";
            content += System.Environment.NewLine;
            content += "{";
            content += $"\tpublic class {name}";
            content += "\t{";
            content += "\t}";
            content += "}";

            return content;
        }
    }
}