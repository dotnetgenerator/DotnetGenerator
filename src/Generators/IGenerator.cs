
using System.Threading.Tasks;

namespace dgen.Generators {
    public interface IGenerator
    {
        Task GenerateFileAsync(GeneratorType genType, string name);
    }
}