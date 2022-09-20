using System.IO;
using System.Net;
using Cake.Core.IO;
using Cake.ProGet.Asset;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;
using Path = System.IO.Path;

namespace Cake.ProGet.Tests.Asset
{
    public sealed class ProGetAssetDownloaderTests
    {
        private readonly ProGetConfiguration _config;

        public ProGetAssetDownloaderTests()
        {
            _config = new ProGetConfiguration
            {
                ProGetUser = "testuser",
                ProGetPassword = "password"
            };
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Download_Asset(string assetUri)
        {
            using(var server = WireMockServer.Start())
            {
                var tempFile = Path.GetTempFileName();

                server.Given(Request.Create().WithPath(assetUri).UsingGet())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBodyFromFile(tempFile));

                var asset = new ProGetAssetDownloader(_config);
                var outputPath = Path.GetRandomFileName();
                asset.GetSingleAsset($"http://localhost:{server.Ports[0]}{assetUri}", outputPath);
                Assert.True(File.Exists(outputPath));
                File.Delete(outputPath);
            }
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Throw_If_Asset_Not_Found(string assetUri)
        {
            using(var server = WireMockServer.Start())
            {
                server.Given(Request.Create().WithPath(assetUri).UsingGet())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.NotFound));

                var asset = new ProGetAssetDownloader(_config);
                var result = Record.Exception(() => asset.GetSingleAsset($"http://localhost:{server.Ports[0]}{assetUri}", new FilePath(Path.GetTempPath())));
                ExtraAssert.IsCakeException(result, "The asset was not found.");
            }
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Throw_If_Unauthorized(string assetUri)
        {
            using(var server = WireMockServer.Start())
            {
                server.Given(Request.Create().WithPath(assetUri).UsingGet())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.Unauthorized));

                var asset = new ProGetAssetDownloader(_config);
                var result = Record.Exception(() =>
                    asset.GetSingleAsset($"http://localhost:{server.Ports[0]}{assetUri}", new FilePath(Path.GetTempPath())));
                ExtraAssert.IsCakeException(result, "Authorization to ProGet server failed; Credentials were incorrect, or not supplied.");
            }
        }
    }
}
