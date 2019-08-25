using Cake.ProGet.Universal.Install;
using FluentAssertions;
using Xunit;

namespace Cake.ProGet.Tests.Universal.Install
{
    public sealed class UniversalPackageInstallSettingsTests
    {
        public sealed class TheCtor
        {
            [Theory]
            [InlineData("")]
            [InlineData(null)]
            public void Should_Throw_If_Package_Is_Null_Or_Empty(string package)
            {
                var ex = Record.Exception(() => new UniversalPackageInstallSettings(package, "http://proget.com/upack/feed", "./folder"));

                ExtraAssert.IsArgumentException(ex, "package", "Value cannot be null or empty.");
            }

            [Theory]
            [InlineData("")]
            [InlineData(null)]
            public void Should_Throw_If_Source_Is_Null(string source)
            {
                var ex = Record.Exception(() => new UniversalPackageInstallSettings("Test.Package", source, "./folder"));

                ExtraAssert.IsArgumentException(ex, "source", "Value cannot be null or empty.");
            }

            [Fact]
            public void Should_Throw_If_TargetDirectory_Is_Null()
            {
                var ex = Record.Exception(() => new UniversalPackageInstallSettings("Test.Package", "http://proget.com/upack/feed", null));

                ExtraAssert.IsArgumentNullException(ex, "targetDirectory");
            }
        }

        public sealed class TheHasCredentialsMethod
        {
            [Theory]
            [InlineData("", "pass", true)]
            [InlineData("user", "", true)]
            [InlineData("user", "pass", true)]
            public void Either_Username_And_Password_Must_Be_Specified_To_Be_Considered_Present(string username, string password, bool expected)
            {
                var settings = new UniversalPackageInstallSettings("package.id", "http://proget.com/upack/feed", "./folder/test.upack") { UserName = username, Password = password };
                settings.HasCredentials().Should().Be(expected);
            }
        }

        public sealed class TheAreCredentialsValidMethod
        {
            [Theory]
            [InlineData("", "pass", false)]
            [InlineData("user", "", false)]
            [InlineData("user", "pass", true)]
            public void Both_Username_And_Password_Must_Be_Specified_To_Be_Considered_Valid(string username, string password, bool expected)
            {
                var settings = new UniversalPackageInstallSettings("package.id", "http://proget.com/upack/feed", "./folder/test.upack") { UserName = username, Password = password };
                settings.AreCredentialsValid().Should().Be(expected);
            }
        }

        public sealed class TheHasVersionMethod
        {
            [Theory]
            [InlineData("", false)]
            [InlineData(" ", false)]
            [InlineData("\t", false)]
            [InlineData(null, false)]
            [InlineData("1.2.0", true)]
            public void HasVersionShouldBeTrueIfVersionIsNotNullOrWhitespace(string version, bool expected)
            {
                var settings = new UniversalPackageInstallSettings("package.id", "http://proget.com/upack/feed", "./folder/test.upack") { Version = version };
                settings.HasVersion().Should().Be(expected);
            }
        }

        [Fact]
        public void Overwrite_Should_Be_False_By_Default()
        {
            // Given, When
            var settings = new UniversalPackageInstallSettings("package.id", "http://proget.com/upack/feed", "./folder/test.upack");

            // Then
            Assert.False(settings.Overwrite);
        }

        [Fact]
        public void PreserveTimestamps_Should_Be_False_By_Default()
        {
            // Given, When
            var settings = new UniversalPackageInstallSettings("package.id", "http://proget.com/upack/feed", "./folder/test.upack");

            // Then
            Assert.False(settings.PreserveTimestamps);
        }
    }
}
