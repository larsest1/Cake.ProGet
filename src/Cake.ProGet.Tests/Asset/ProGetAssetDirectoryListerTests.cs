using System;
using System.Collections.Generic;
using Cake.ProGet.Asset;
using HttpMock;
using Xunit;

namespace Cake.ProGet.Tests.Asset
{
    public sealed class ProGetAssetDirectoryListerTests : IDisposable
    {
        private const string Host = "http://localhost:9191";
        private readonly ProGetConfiguration _config;
        private readonly IHttpServer _server;

        public ProGetAssetDirectoryListerTests()
        {
            _config = new ProGetConfiguration
            {
                ProGetUser = "testuser",
                ProGetPassword = "password"
            };

            _server = HttpMockRepository.At(Host).WithNewContext();
        }

        [Theory]
        [InlineData("/endpoints/test/content/dir/")]
        public void Should_Get_List_Of_Assets_Recursive(string assetDirectoryUri)
        {
            const string jsonList = @"[
            {
                ""name"": ""integration-tests"",
                ""type"": ""dir"",
                ""created"": ""2017-10-25T20:22:48.533Z"",
                ""modified"": ""2017-10-25T20:22:48.503Z""
            },
            {
                ""name"": ""test-resources-0.1.2.zip"",
                ""parent"": ""integration-tests"",
                ""size"": 383814131,
                ""type"": ""application/x-zip-compressed"",
                ""sha1"": ""868541f2c06a18ce9c159a709b18629c1a3b89d7"",
                ""created"": ""2017-10-31T21:59:20.81Z"",
                ""modified"": ""2017-10-31T21:59:20.81Z""
            }
            ]";
            
            _server.Stub(x => x.Get($"{assetDirectoryUri}"))
                .WithParams(new Dictionary<string, string>{{"recursive", "true"}})
                .Return(jsonList)
                .OK();
            
            var asset = new ProGetAssetDirectoryLister(_config);
            var result = asset.ListDirectory($"{Host}{assetDirectoryUri}", true);
            Assert.Equal(2, result.Count);
        }

        [Theory]
        [InlineData("/endpoints/test/content/dir/")]
        public void Should_Get_List_Of_Assets_NonRecursive(string assetDirectoryUri)
        {
            const string jsonList = @"[
            {
                ""name"": ""integration-tests"",
                ""type"": ""dir"",
                ""created"": ""2017-10-25T20:22:48.533Z"",
                ""modified"": ""2017-10-25T20:22:48.503Z""
            }
            ]";
            
            _server.Stub(x => x.Get(assetDirectoryUri))
                .WithParams(new Dictionary<string, string>{{"recursive", "false"}})
                .Return(jsonList)
                .OK();
            
            var asset = new ProGetAssetDirectoryLister(_config);
            var result = asset.ListDirectory($"{Host}{assetDirectoryUri}");
            Assert.Single(result);
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}