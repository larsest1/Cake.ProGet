using Cake.ProGet.Tests.Universal.Fixtures;
using Cake.Testing;
using Xunit;

namespace Cake.ProGet.Tests.Universal.Unpack
{
    public sealed class UniversalPackageUnpackerTests
    {
        [Theory]
        [InlineData("/bin/upack/upack.exe", "/bin/upack/upack.exe")]
        [InlineData("./upack/upack.exe", "/Working/upack/upack.exe")]
        public void Should_Use_UPack_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
        {
            // Given
            var fixture = new UniversalPackageUnpackerFixture { Settings = { ToolPath = toolPath } };
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
            var fixture = new UniversalPackageUnpackerFixture();
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
            var fixture = new UniversalPackageUnpackerFixture();
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
            var fixture = new UniversalPackageUnpackerFixture();
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
            var fixture = new UniversalPackageUnpackerFixture { Settings = null };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_Package_Is_Null()
        {
            // Given
            var fixture = new UniversalPackageUnpackerFixture { Settings = { Package = null } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeException(result, "Required setting Package not specified.");
        }

        [Fact]
        public void Should_Throw_If_TargetDirectory_Is_Null()
        {
            // Given
            var fixture = new UniversalPackageUnpackerFixture { Settings = { TargetDirectory = null } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeException(result, "Required setting TargetDirectory not specified.");
        }

        [Fact]
        public void Should_Throw_If_Package_File_Does_Not_Exist()
        {
            // Given
            var fixture = new UniversalPackageUnpackerFixture { Settings = { Package = "./path/does/not/exist.upack" } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeExceptionWithMessage(result, s => s.StartsWith("Universal package file does not exist at"));
        }

        [Theory]
        [InlineData(true, "unpack \"/Working/test/test-package.upack\" \"/Working/test/target-dir\" --overwrite")]
        [InlineData(false, "unpack \"/Working/test/test-package.upack\" \"/Working/test/target-dir\"")]
        public void Should_Add_Overwrite_To_Arguments_If_Set(bool overwrite, string expected)
        {
            // Given
            var fixture = new UniversalPackageUnpackerFixture { Settings = { Overwrite = overwrite } };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Args);
        }
    }
}
