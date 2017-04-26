using Cake.ProGet.Universal.Unpack;
using Xunit;

namespace Cake.ProGet.Tests.Universal.Pack
{
    public sealed class UniversalPackageUnpackSettingsTests
    {
        public sealed class TheCtor
        {
            [Fact]
            public void Should_Throw_If_Package_FilePath_Is_Null()
            {
                var ex = Record.Exception(() => new UniversalPackageUnpackSettings(null, "./folder"));

                Assert.IsArgumentNullException(ex, "package");
            }

            [Fact]
            public void Should_Throw_If_TargetDirectory_Is_Null()
            {
                var ex = Record.Exception(() => new UniversalPackageUnpackSettings("./path/to/file.upack", null));

                Assert.IsArgumentNullException(ex, "targetDirectory");
            }
        }

        [Fact]
        public void Overwrite_Should_Be_False_By_Default()
        {
            // Given, When
            var settings = new UniversalPackageUnpackSettings("./path/to/file.upack", "./path/to/destination");

            // Then
            Assert.False(settings.Overwrite);
        }
    }
}
