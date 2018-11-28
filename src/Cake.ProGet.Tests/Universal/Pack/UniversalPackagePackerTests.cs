using Cake.Core.IO;
using Cake.ProGet.Tests.Universal.Fixtures;
using Cake.Testing;
using Xunit;

namespace Cake.ProGet.Tests.Universal.Pack
{
    public sealed class UniversalPackagePackerTests
    {
        [Theory]
        [InlineData("/bin/upack/upack.exe", "/bin/upack/upack.exe")]
        [InlineData("./upack/upack.exe", "/Working/upack/upack.exe")]
        public void Should_Use_UPack_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
        {
            // Given
            var fixture = new UniversalPackagePackerFixture { Settings = { ToolPath = toolPath } };
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Path.FullPath);
        }

        [Fact]
        public void Should_Throw_If_UPack_Executable_Was_Not_Found()
        {
            // Given
            var fixture = new UniversalPackagePackerFixture();
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeException(result, "UPack: Could not locate executable.");
        }

        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new UniversalPackagePackerFixture();
            fixture.GivenProcessCannotStart();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeException(result, "UPack: Process was not started.");
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
        {
            // Given
            var fixture = new UniversalPackagePackerFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeException(result, "UPack: Process returned an error (exit code 1).");
        }

        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new UniversalPackagePackerFixture { Settings = null };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_Metadata_FilePath_Is_Null()
        {
            // Given
            var fixture = new UniversalPackagePackerFixture { Settings = { MetadataFilePath = null } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeException(result, "Required setting MetadataFilePath not specified.");
        }

        [Fact]
        public void Should_Throw_If_Source_Directory_Is_Null()
        {
            // Given
            var fixture = new UniversalPackagePackerFixture { Settings = { SourceDirectory = null } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeException(result, "Required setting SourceDirectory not specified.");
        }

        [Fact]
        public void Should_Throw_If_Metadata_FilePath_Does_Not_Exist()
        {
            // Given
            var fixture = new UniversalPackagePackerFixture { Settings = { MetadataFilePath = "./path/does/not/exist.uspec" } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeExceptionWithMessage(result, s => s.StartsWith("Metadata file does not exist at"));
        }

        [Fact]
        public void Should_Throw_If_Metadata_SourceDirectory_Does_Not_Exist()
        {
            // Given
            var fixture = new UniversalPackagePackerFixture { Settings = { SourceDirectory = "./path/does/not/exist" } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeExceptionWithMessage(result, s => s.StartsWith("Source directory does not exist at"));
        }

        [Fact]
        public void Should_Add_TargetDirectory_To_Arguments_If_Set()
        {
            // Given
            var fixture = new UniversalPackagePackerFixture
            {
                Settings = {
                    TargetDirectory = new DirectoryPath("./target-directory")
                }
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Contains("/Working/target-directory", result.Args);
        }
        
        [Fact]
        public void Should_Not_Add_TargetDirectory_To_Arguments_If_Null()
        {
            // Given
            var fixture = new UniversalPackagePackerFixture
            {
                Settings = {
                    TargetDirectory = null
                }
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.DoesNotContain("--targetDirectory", result.Args);
        }
    }
}
