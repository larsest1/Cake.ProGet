using Cake.ProGet.Universal.Install;
using FluentAssertions;
using Xunit;

namespace Cake.ProGet.Tests.Universal.Install
{
    public sealed class InstallSettingsTests
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

        [Theory]
        [InlineData("", "pass", false)]
        [InlineData("user", "", false)]
        [InlineData("user", "pass", true)]
        public void Both_Username_And_Password_Must_Be_Specified_To_Be_Considered_Valid(string username, string password, bool expected)
        {
            var settings = new UniversalPackageInstallSettings("package.id", "http://proget.com/upack/feed", "./folder/test.upack") { UserName = username, Password = password };
            settings.AreCredentialsValid().Should().Be(expected);
        }

        [Theory]
        [InlineData("1.2.3", true)]
        [InlineData(null, false)]
        [InlineData(" ", false)]
        [InlineData("", false)]
        [InlineData("\t", false)]
        public void HasVersion_Should_Return_Correctly(string version, bool expected)
        {
            var settings = new UniversalPackageInstallSettings("package.id", "http://proget.com/upack/feed", "./folder/test.upack") { Version = version };
            settings.HasVersion().Should().Be(expected);
        }
    }
}
