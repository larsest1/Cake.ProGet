using System;
using Cake.Core;
using Cake.ProGet.Tests.Universal.Fixtures;
using Cake.Testing;
using FluentAssertions;
using Xunit;

namespace Cake.ProGet.Tests.Universal
{
    public sealed class UPackToolResolverTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new UPackToolResolverFixture { FileSystem = null };

                // When
                var result = Record.Exception(() => fixture.Resolve());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new UPackToolResolverFixture { Environment = null };

                // When
                var result = Record.Exception(() => fixture.Resolve());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("environment");
            }

            [Fact]
            public void Should_Throw_If_Tool_Locator_Is_Null()
            {
                // Given
                var fixture = new UPackToolResolverFixture { Tools = null };

                // When
                var result = Record.Exception(() => fixture.Resolve());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("tools");
            }
        }

        public sealed class TheResolveToolPathMethod
        {
            [Fact]
            public void Should_Throw_If_UPack_Exe_Could_Not_Be_Found()
            {
                // Given
                var fixture = new UPackToolResolverFixture();

                // When
                var result = Record.Exception(() => fixture.Resolve());

                // Assert
                result.Should().BeOfType<CakeException>().Which.Message.Should().Be("Could not locate upack.exe.");
            }

            [Fact]
            public void Should_Be_Able_To_Resolve_Path_From_The_Tools_Directory()
            {
                // Given
                var fixture = new UPackToolResolverFixture();
                fixture.FileSystem.CreateFile("/Working/tools/upack.exe");

                // When
                var result = fixture.Resolve();

                // Then
                Assert.Equal("/Working/tools/upack.exe", result.FullPath);
            }

            [Fact]
            public void Should_Be_Able_To_Resolve_Path_Via_Environment_Path_Variable_On_Unix()
            {
                // Given
                var fixture = new UPackToolResolverFixture();
                fixture.Environment.SetEnvironmentVariable("PATH", "/temp:/stuff/programs:/programs");
                fixture.FileSystem.CreateFile("/stuff/programs/upack.exe");

                // When
                var result = fixture.Resolve();

                // Then
                Assert.Equal("/stuff/programs/upack.exe", result.FullPath);
            }

            [Fact]
            public void Should_Be_Able_To_Resolve_Path_Via_Environment_Path_Variable_On_Windows()
            {
                // Given
                var fixture = new UPackToolResolverFixture(FakeEnvironment.CreateWindowsEnvironment());
                fixture.Environment.SetEnvironmentVariable("PATH", "/temp;/stuff/programs;/programs");
                fixture.FileSystem.CreateFile("/stuff/programs/upack.exe");

                // When
                var result = fixture.Resolve();

                // Then
                Assert.Equal("/stuff/programs/upack.exe", result.FullPath);
            }

            [Fact]
            public void Should_Be_Able_To_Resolve_Path_Via_UPack_Environment_Variable()
            {
                // Given
                var fixture = new UPackToolResolverFixture();
                fixture.Environment.SetEnvironmentVariable("UPackInstall", "/programs");
                fixture.FileSystem.CreateFile("/programs/upack.exe");

                // When
                var result = fixture.Resolve();

                // Then
                Assert.Equal("/programs/upack.exe", result.FullPath);
            }
        }
    }
}
