
using McMaster.Extensions.CommandLineUtils;

namespace test.mocks {

    public class ReporterMock : IReporter
    {
        public void Error(string message)
        {
            throw new System.NotImplementedException();
        }

        public void Output(string message)
        {
            throw new System.NotImplementedException();
        }

        public void Verbose(string message)
        {
            throw new System.NotImplementedException();
        }

        public void Warn(string message)
        {
            throw new System.NotImplementedException();
        }
    }

}