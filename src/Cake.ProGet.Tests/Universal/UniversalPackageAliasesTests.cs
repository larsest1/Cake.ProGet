using Cake.ProGet.Universal;
using Cake.ProGet.Universal.Install;
using Cake.ProGet.Universal.Pack;
using Cake.ProGet.Universal.Push;
using Cake.ProGet.Universal.Unpack;
using Xunit;

namespace Cake.ProGet.Tests.Universal
{
    public sealed class UniversalPackageAliasesTests
    {
        public sealed class ThePackMethod
        {
            [Fact]
            public void Should_Throw_If_Cake_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => UniversalPackageAliases.UPackPack(null, new UniversalPackagePackSettings()));

                // Then
                ExtraAssert.IsArgumentNullException(result, "context");
            }
        }

        public sealed class ThePushMethod
        {
            [Fact]
            public void Should_Throw_If_Cake_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => UniversalPackageAliases.UPackPush(null, new UniversalPackagePushSettings()));

                // Then
                ExtraAssert.IsArgumentNullException(result, "context");
            }
        }

        public sealed class TheInstallMethod
        {
            [Fact]
            public void Should_Throw_If_Cake_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => UniversalPackageAliases.UPackInstall(null, new UniversalPackageInstallSettings()));

                // Then
                ExtraAssert.IsArgumentNullException(result, "context");
            }
        }

        public sealed class TheUnpackMethod
        {
            [Fact]
            public void Should_Throw_If_Cake_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => UniversalPackageAliases.UPackUnpack(null, new UniversalPackageUnpackSettings()));

                // Then
                ExtraAssert.IsArgumentNullException(result, "context");
            }
        }
    }
}
