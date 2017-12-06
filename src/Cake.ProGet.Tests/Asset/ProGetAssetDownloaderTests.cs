using System;
using System.IO;
using System.Net;
using Cake.Core.IO;
using Cake.ProGet.Asset;
using HttpMock;
using Xunit;
using Path = System.IO.Path;

namespace Cake.ProGet.Tests.Asset
{
    public sealed class ProGetAssetDownloaderTests : IDisposable
    {
        private const string Host = "http://localhost:9191";
        private readonly ProGetConfiguration _config;
        private readonly IHttpServer _server;
        
        public ProGetAssetDownloaderTests()
        {
            _config = new ProGetConfiguration
            {
                ProGetUser = "testuser",
                ProGetPassword = "password"
            };

            _server = HttpMockRepository.At(Host).WithNewContext();
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Download_Asset(string assetUri)
        {
            var tempFile = Path.GetTempFileName();
            _server.Stub(x => x.Get(assetUri))
                .ReturnFile(tempFile)
                .OK();
            
            var asset = new ProGetAssetDownloader(_config);
            var outputPath = Path.GetRandomFileName();
            asset.GetSingleAsset($"{Host}{assetUri}", outputPath);
            Assert.True(File.Exists(outputPath));
            File.Delete(outputPath);
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Throw_If_Asset_Not_Found(string assetUri)
        {
            _server.Stub(x => x.Get(assetUri))
                .NotFound();
            
            var asset = new ProGetAssetDownloader(_config);
            var result = Record.Exception(() => asset.GetSingleAsset($"{Host}{assetUri}", new FilePath(Path.GetTempPath())));
            Assert.IsCakeException(result, "The asset was not found.");
        }

        [Theory]
        [InlineData("/endpoints/test/content/test.gif")]
        public void Should_Throw_If_Unauthorized(string assetUri)
        {
            _server.Stub(x => x.Get(assetUri))
                .WithStatus(HttpStatusCode.Unauthorized);
            
            var asset = new ProGetAssetDownloader(_config);
            var result = Record.Exception(() =>
                asset.GetSingleAsset($"{Host}{assetUri}", new FilePath(Path.GetTempPath())));
            Assert.IsCakeException(result, "Authorization to ProGet server failed; Credentials were incorrect, or not supplied.");
        }
        
        public void Dispose()
        {
            _server.Dispose();
        }
    }
}