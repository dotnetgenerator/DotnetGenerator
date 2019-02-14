
using dgen.Generators;
using FluentAssertions;
using Xunit;

namespace test {

    public class TypedGeneratorTests {

        #region ClassGenerator

        [Fact]
        public void TestClassGen_Content(){
            var sut = new ClassGenerator();

            var res = sut.createContent("dgen.MyFolder", "MyClass");

            var content = string.Empty;

            content += System.Environment.NewLine;
            content += $"namespace dgen.MyFolder";
            content += System.Environment.NewLine;
            content += "{";
            content += System.Environment.NewLine;
            content += $"\tpublic class MyClass";
            content += System.Environment.NewLine;
            content += "\t{";
            content += System.Environment.NewLine;
            content += "\t}";
            content += System.Environment.NewLine;
            content += "}";

            res.Should().Be(content);
        }

        [Fact]
        public void TestClassGen_1(){
            var sut = new ClassGenerator();

            var res = sut.Generate("dgen.MyFolder", "MyClass");

            var content = string.Empty;

            content += System.Environment.NewLine;
            content += $"namespace dgen.MyFolder";
            content += System.Environment.NewLine;
            content += "{";
            content += System.Environment.NewLine;
            content += $"\tpublic class MyClass";
            content += System.Environment.NewLine;
            content += "\t{";
            content += System.Environment.NewLine;
            content += "\t}";
            content += System.Environment.NewLine;
            content += "}";

            res.Content.Should().Be(content);
            res.FileName.Should().Be("MyClass.cs");
        }

        #endregion
    }
}