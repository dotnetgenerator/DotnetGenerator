
using dgen.Models;

namespace dgen.Generators {

    public interface ITypeGenerator
    {
        GeneratorResult Generate(string ns, string name);
    }
}