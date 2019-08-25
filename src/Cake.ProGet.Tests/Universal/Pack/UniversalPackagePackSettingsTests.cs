using Cake.ProGet.Universal.Pack;
using Xunit;

namespace Cake.ProGet.Tests.Universal.Pack
{
    public sealed class UniversalPackagePackSettingsTests
    {
        public sealed class TheCtor
        {
            [Fact]
            public void Should_Throw_If_Metadata_FilePath_Is_Null()
            {
                var ex = Record.Exception(() => new UniversalPackagePackSettings(null, "./folder"));

                ExtraAssert.IsArgumentNullException(ex, "metadataFilePath");
            }

            [Fact]
            public void Should_Throw_If_SourceDirectory_Is_Null()
            {
                var ex = Record.Exception(() => new UniversalPackagePackSettings("./path/to/file.uspec", null));

                ExtraAssert.IsArgumentNullException(ex, "sourceDirectory");
            }
        }
    }
}
