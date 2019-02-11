using System.IO.Abstractions;

namespace test.mocks {

    public class FileSystemMock : IFileSystem
    {
        public FileBase File => throw new System.NotImplementedException();

        public DirectoryBase Directory => new DirectoryBaseMock(this);

        public IFileInfoFactory FileInfo => throw new System.NotImplementedException();

        public IFileStreamFactory FileStream => throw new System.NotImplementedException();

        public PathBase Path => throw new System.NotImplementedException();

        public IDirectoryInfoFactory DirectoryInfo => throw new System.NotImplementedException();

        public IDriveInfoFactory DriveInfo => throw new System.NotImplementedException();

        public IFileSystemWatcherFactory FileSystemWatcher => throw new System.NotImplementedException();
    }
}