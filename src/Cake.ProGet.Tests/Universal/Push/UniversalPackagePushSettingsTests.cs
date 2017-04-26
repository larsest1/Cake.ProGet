using Cake.ProGet.Universal.Push;
using FluentAssertions;
using Xunit;

namespace Cake.ProGet.Tests.Universal.Push
{
    public sealed class UniversalPackagePushSettingsTests
    {
        public sealed class TheCtor
        {
            [Fact]
            public void Should_Throw_If_Package_FilePath_Is_Null()
            {
                var ex = Record.Exception(() => new UniversalPackagePushSettings(null, "./folder"));

                Assert.IsArgumentNullException(ex, "packageFilePath");
            }

            [Fact]
            public void Should_Throw_If_TargetDirectory_Is_Null()
            {
                var ex = Record.Exception(() => new UniversalPackagePushSettings("./path/to/file.upack", null));

                Assert.IsArgumentException(ex, "target", "Value cannot be null or empty.");
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
                var settings = new UniversalPackagePushSettings("./folder/test.upack", "http://proget.com/upack/feed") { UserName = username, Password = password };
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
                var settings = new UniversalPackagePushSettings("./folder/test.upack", "http://proget.com/upack/feed") { UserName = username, Password = password };
                settings.AreCredentialsValid().Should().Be(expected);
            }
        }
        
    }
}
