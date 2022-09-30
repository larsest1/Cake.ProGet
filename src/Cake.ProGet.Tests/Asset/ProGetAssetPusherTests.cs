using System.IO;
using System.Net;
using Cake.Core.IO;
using Cake.ProGet.Asset;
using Cake.Testing;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;
using Path = System.IO.Path;

namespace Cake.ProGet.Tests.Asset
{
    public sealed class ProGetAssetPusherTests
    {
        private readonly ProGetConfiguration _config;
        private readonly FakeLog _log;

        public ProGetAssetPusherTests()
        {
            _config = new ProGetConfiguration
            {
                ProGetUser = "testuser",
                ProGetPassword = "password"
            };

            _log = new FakeLog();
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Report_True_If_Asset_Exists(string assetUri)
        {
            using(var server = WireMockServer.Start())
            {
                server.Given(Request.Create().WithPath(assetUri).UsingHead())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK));

                var asset = new ProGetAssetPusher(_log, _config );
                var result = asset.DoesAssetExist($"http://localhost:{server.Ports[0]}{assetUri}");
                Assert.True(result);
            }
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Report_False_If_Asset_Does_Not_Exist(string assetUri)
        {
            using(var server = WireMockServer.Start())
            {
                server.Given(Request.Create().WithPath(assetUri).UsingHead())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.NotFound));

                var asset = new ProGetAssetPusher(_log, _config );
                var result = asset.DoesAssetExist($"http://localhost:{server.Ports[0]}{assetUri}");
                Assert.False(result);
            }
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Fail_Existence_Check_If_Creds_Are_Invalid(string assetUri)
        {
            using(var server = WireMockServer.Start())
            {
                server.Given(Request.Create().WithPath(assetUri).UsingHead())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.Unauthorized));

                var asset = new ProGetAssetPusher(_log, _config);
                var result = Record.Exception(() => asset.DoesAssetExist($"http://localhost:{server.Ports[0]}{assetUri}"));
                ExtraAssert.IsCakeException(result, "Authorization to ProGet server failed; Credentials were incorrect, or not supplied.");
            }
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Delete_Asset(string assetUri)
        {
            using(var server = WireMockServer.Start())
            {
                server.Given(Request.Create().WithPath(assetUri).UsingDelete())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK));

                var asset = new ProGetAssetPusher(_log, _config);
                var result = asset.DeleteAsset($"http://localhost:{server.Ports[0]}{assetUri}");
                Assert.True(result);
            }
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Fail_Delete_If_Creds_Are_Invalid(string assetUri)
        {
            using(var server = WireMockServer.Start())
            {
                server.Given(Request.Create().WithPath(assetUri).UsingDelete())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.Unauthorized));

                var asset = new ProGetAssetPusher(_log, _config);
                var result = Record.Exception(() => asset.DeleteAsset($"http://localhost:{server.Ports[0]}{assetUri}"));
                ExtraAssert.IsCakeException(result, "Authorization to ProGet server failed; Credentials were incorrect, or not supplied.");
            }
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Push_New_Asset_With_Put_Under_5MB(string assetUri)
        {
            using(var server = WireMockServer.Start())
            {
                server.Given(Request.Create().WithPath(assetUri).UsingPut())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK));

                var asset = new ProGetAssetPusher(_log, _config);
                var tempFile = new FilePath($"{Path.GetTempPath()}Should_Push_New_Asset_With_Put_Under_5MB");

                if (File.Exists(tempFile.FullPath))
                {
                    File.Delete(tempFile.FullPath);
                }

                using (var fileStream = new FileStream(tempFile.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    fileStream.SetLength(4194304);
                }
                var result = Record.Exception(() => asset.Publish(tempFile, $"http://localhost:{server.Ports[0]}{assetUri}"));

                Assert.Null(result);
            }
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Push_New_Asset_With_Multipart_Post_Over_5MB(string assetUri)
        {
            using(var server = WireMockServer.Start())
            {
                server.Given(Request.Create().WithPath(assetUri).UsingPost())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK));

                var asset = new ProGetAssetPusher(_log, _config);
                var tempFile = new FilePath($"{Path.GetTempPath()}{Path.GetRandomFileName()}");
                using (var fileStream = new FileStream(tempFile.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    fileStream.SetLength(6291456);
                }
                var result = Record.Exception(() => asset.Publish(tempFile, $"http://localhost:{server.Ports[0]}{assetUri}"));
                File.Delete(tempFile.FullPath);
                Assert.Null(result);
            }
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Throw_Exception_When_Asset_Push_Fails_As_Put(string assetUri)
        {
            using(var server = WireMockServer.Start())
            {
                server.Given(Request.Create().WithPath(assetUri).UsingPut())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.BadRequest));

                var asset = new ProGetAssetPusher(_log, _config);
                var tempFile = new FilePath($"{Path.GetTempPath()}Should_Throw_Exception_When_Asset_Push_Fails_As_Put.txt");

                if (File.Exists(tempFile.FullPath))
                {
                    File.Delete(tempFile.FullPath);
                }

                using (var fileStream = new FileStream(tempFile.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    fileStream.SetLength(4194304);
                }
                var result = Record.Exception(() => asset.Publish(tempFile, $"http://localhost:{server.Ports[0]}{assetUri}"));

                ExtraAssert.IsCakeException(result, "Upload failed. This request would have overwritten an existing package.");
            }
        }
    }
}
