using Cake.ProGet.Tests.Universal.Fixtures;
using Cake.Testing;
using Xunit;

namespace Cake.ProGet.Tests.Universal.Push
{
    public sealed class UniversalPackagePusherTests
    {
        [Fact]
        public void Should_Throw_If_Resolver_Is_Null()
        {
            // Given
            var fixture = new UniversalPackagePusherFixture();
            fixture.Resolver = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsArgumentNullException(result, "resolver");
        }

        [Theory]
        [InlineData("/bin/upack/upack.exe", "/bin/upack/upack.exe")]
        [InlineData("./upack/upack.exe", "/Working/upack/upack.exe")]
        public void Should_Use_UPack_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
        {
            // Given
            var fixture = new UniversalPackagePusherFixture { Settings = { ToolPath = toolPath } };
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
            var fixture = new UniversalPackagePusherFixture();
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
            var fixture = new UniversalPackagePusherFixture();
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
            var fixture = new UniversalPackagePusherFixture();
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
            var fixture = new UniversalPackagePusherFixture { Settings = null };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_Package_Is_Null()
        {
            // Given
            var fixture = new UniversalPackagePusherFixture { Settings = { Package = null } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeException(result, "Required setting Package not specified.");
        }

        [Fact]
        public void Should_Throw_If_Target_Is_Null()
        {
            // Given
            var fixture = new UniversalPackagePusherFixture { Settings = { Target = null } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeException(result, "Required setting Target not specified.");
        }

        [Fact]
        public void Should_Throw_If_Package_File_Does_Not_Exist()
        {
            // Given
            var fixture = new UniversalPackagePusherFixture { Settings = { Package = "./path/does/not/exist.upack" } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeExceptionWithMessage(result, s => s.StartsWith("Universal package file does not exist at"));
        }

        [Fact]
        public void Should_Throw_If_Credentials_Are_Not_Valid()
        {
            // Given
            var fixture = new UniversalPackagePusherFixture { Settings = { UserName = "user@foo.com", Password = "" } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsCakeException(result, "Both username and password must be specified for authentication");
        }

        [Theory]
        [InlineData("user", "pass", "push \"/Working/test/file.upack\" \"http://proget.com/upack-feed\" --user=user:pass")]
        [InlineData(null, null, "push \"/Working/test/file.upack\" \"http://proget.com/upack-feed\"")]
        public void Should_Add_Credentials_To_Arguments_If_Set(string user, string pass, string expected)
        {
            // Given
            var fixture = new UniversalPackagePusherFixture { Settings = { UserName = user, Password = pass } };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Args);
        }
    }
}
