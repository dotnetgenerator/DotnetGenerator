
namespace dgen.Generators {
    public interface IGenerator
    {
        void GenerateFile(GeneratorType genType, string name);
    }
}