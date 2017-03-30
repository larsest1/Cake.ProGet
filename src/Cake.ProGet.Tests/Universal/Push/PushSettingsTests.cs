using Cake.ProGet.Universal.Push;
using FluentAssertions;
using Xunit;

namespace Cake.ProGet.Tests.Universal.Push
{
    public sealed class PushSettingsTests
    {
        [Theory]
        [InlineData("", "pass", true)]
        [InlineData("user", "", true)]
        [InlineData("user", "pass", true)]
        public void Either_Username_And_Password_Must_Be_Specified_To_Be_Considered_Present(string username, string password, bool expected)
        {
            var settings = new UniversalPackagePushSettings("./folder/test.upack", "http://proget.com/upack/feed") { UserName = username, Password = password };
            settings.HasCredentials().Should().Be(expected);
        }

        [Theory]
        [InlineData("", "pass", false)]
        [InlineData("user", "", false)]
        [InlineData("user", "pass", true)]
        public void Both_Username_And_Password_Must_Be_Specified_To_Be_Considered_Valid(string username, string password, bool expected)
        {
            var settings = new UniversalPackagePushSettings("./folder/test.upack", "http://proget.com/upack/feed") { UserName = username, Password = password };
            settings.AreCredentialsValid().Should().Be(expected);
        }
    }
}
