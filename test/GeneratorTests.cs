using dgen.Generators;
using dgen.Services;
using test.mocks;

namespace test {

    public class GeneratorTests {

        private readonly IGenerator _gen;
        public GeneratorTests()
        {
            _gen = new BaseGenerator(new ProjectDiscoveryService(new FileSystemMock()), new ReporterMock());


        }
    }

}