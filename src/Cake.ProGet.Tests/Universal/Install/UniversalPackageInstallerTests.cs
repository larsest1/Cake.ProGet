using System;
using Cake.ProGet.Tests.Universal.Fixtures;
using Cake.Testing;
using Xunit;

namespace Cake.ProGet.Tests.Universal.Install
{
    public sealed class UniversalPackageInstallerTests
    {
        [Theory]
        [InlineData("/bin/upack/upack.exe", "/bin/upack/upack.exe")]
        [InlineData("./upack/upack.exe", "/Working/upack/upack.exe")]
        public void Should_Use_UPack_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
        {
            // Given
            var fixture = new UniversalPackageInstallerFixture { Settings = { ToolPath = toolPath } };
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
            var fixture = new UniversalPackageInstallerFixture();
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            ExtraAssert.IsCakeException(result, "UPack: Could not locate executable.");
        }

        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new UniversalPackageInstallerFixture();
            fixture.GivenProcessCannotStart();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            ExtraAssert.IsCakeException(result, "UPack: Process was not started.");
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
        {
            // Given
            var fixture = new UniversalPackageInstallerFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            ExtraAssert.IsCakeException(result, "UPack: Process returned an error (exit code 1).");
        }

        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new UniversalPackageInstallerFixture { Settings = null };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            ExtraAssert.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_Package_Is_Null()
        {
            // Given
            var fixture = new UniversalPackageInstallerFixture { Settings = { Package = null } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            ExtraAssert.IsCakeException(result, "Required setting Package not specified.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Throw_If_Source_Is_Null_Or_Empty(string source)
        {
            // Given
            var fixture = new UniversalPackageInstallerFixture { Settings = { Source = source } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            ExtraAssert.IsCakeException(result, "Required setting Source not specified.");
        }

        [Fact]
        public void Should_Throw_If_Target_Directory_Is_Null()
        {
            // Given
            var fixture = new UniversalPackageInstallerFixture { Settings = { TargetDirectory = null } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            ExtraAssert.IsCakeException(result, "Required setting TargetDirectory not specified.");
        }

        [Fact]
        public void Should_Throw_If_Credentials_Are_Not_Valid()
        {
            // Given
            var fixture = new UniversalPackageInstallerFixture { Settings = { UserName = "user@foo.com", Password = "" } };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            ExtraAssert.IsCakeException(result, "Both username and password must be specified for authentication");
        }

        [Theory]
        [InlineData("1.0.0", "install Test.Package 1.0.0 --source=http://proget.com/upack-feed --target=\"/Working/target\"")]
        [InlineData(null, "install Test.Package --source=http://proget.com/upack-feed --target=\"/Working/target\"")]
        public void Should_Add_Version_To_Arguments_If_Set(string version, string expected)
        {
            // Given
            var fixture = new UniversalPackageInstallerFixture { Settings = { Version = version } };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Args);
        }

        [Theory]
        [InlineData(true, "install Test.Package --source=http://proget.com/upack-feed --target=\"/Working/target\" --overwrite")]
        [InlineData(false, "install Test.Package --source=http://proget.com/upack-feed --target=\"/Working/target\"")]
        public void Should_Add_Overwrite_To_Arguments_If_Set(bool overwrite, string expected)
        {
            // Given
            var fixture = new UniversalPackageInstallerFixture { Settings = { Overwrite = overwrite } };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Args);
        }

        [Theory]
        [InlineData(true, "install Test.Package --source=http://proget.com/upack-feed --target=\"/Working/target\" --preserve-timestamps")]
        [InlineData(false, "install Test.Package --source=http://proget.com/upack-feed --target=\"/Working/target\"")]
        public void Should_Add_PreserveTimestamps_To_Arguments_If_Set(bool preserveTimestamps, string expected)
        {
            // Given
            var fixture = new UniversalPackageInstallerFixture { Settings = { PreserveTimestamps = preserveTimestamps } };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Args);
        }

        [Theory]
        [InlineData("user", "pass", "install Test.Package --source=http://proget.com/upack-feed --target=\"/Working/target\" --user=user:pass")]
        [InlineData(null, null, "install Test.Package --source=http://proget.com/upack-feed --target=\"/Working/target\"")]
        public void Should_Add_Credentials_To_Arguments_If_Set(string user, string pass, string expected)
        {
            // Given
            var fixture = new UniversalPackageInstallerFixture { Settings = { UserName = user, Password = pass } };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Args);
        }
    }
}
