
using System.IO.Abstractions.TestingHelpers;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;
using dgen.Services;

using Xunit;
using FluentAssertions;
using System.Collections.Generic;

namespace test {

    public class ProjectDiscoveryTests {

        [Fact]
        public void CSPROJIsInSameDirectory()
        {
            string basePath = XFS.Path(@"C:\test");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);

            var sut = new ProjectDiscoveryService(fs);

            var res = sut.DiscoverProject();

            res.ProjPath.Should().Be(projPath);
            res.RootNamespace.Should().Be("test");
        }

        [Fact]
        public void CSPROJInAParentDirectory1(){
            string basePath = XFS.Path(@"C:\test\folder1");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);

            var sut = new ProjectDiscoveryService(fs);

            var res = sut.DiscoverProject();

            res.ProjPath.Should().Be(projPath);
            res.RootNamespace.Should().Be("test");
            res.Paths.Count.Should().Be(2);
        }

         [Fact]
        public void CSPROJInAParentDirectory2(){
            string basePath = XFS.Path(@"C:\test\folder1\folder2");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);

            var sut = new ProjectDiscoveryService(fs);

            var res = sut.DiscoverProject();

            res.ProjPath.Should().Be(projPath);
            res.RootNamespace.Should().Be("test");
            res.Paths.Count.Should().Be(3);
        }

         [Fact]
        public void CSPROJNotInADirectory(){
            string basePath = XFS.Path(@"C:\test\folder1\folder2");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
            }, basePath);

            var sut = new ProjectDiscoveryService(fs);

            var res = sut.DiscoverProject();

            res.ProjPath.Should().Be(string.Empty);
            res.RootNamespace.Should().Be(string.Empty);
            res.Paths.Count.Should().Be(4);
        }
    }
}