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
                var result = Record.Exception(() => UniversalPackageAliases.Pack(null, new UniversalPackagePackSettings()));
                
                // Then
                Assert.IsArgumentNullException(result, "context");
            }
        }

        public sealed class ThePushMethod
        {
            [Fact]
            public void Should_Throw_If_Cake_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => UniversalPackageAliases.Push(null, new UniversalPackagePushSettings()));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }
        }

        public sealed class TheInstallMethod
        {
            [Fact]
            public void Should_Throw_If_Cake_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => UniversalPackageAliases.Install(null, new UniversalPackageInstallSettings()));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }
        }

        public sealed class TheUnpackMethod
        {
            [Fact]
            public void Should_Throw_If_Cake_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => UniversalPackageAliases.Unpack(null, new UniversalPackageUnpackSettings()));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }
        }
    }
}
