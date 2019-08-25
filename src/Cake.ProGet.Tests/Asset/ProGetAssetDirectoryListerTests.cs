using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Cake.ProGet.Asset;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace Cake.ProGet.Tests.Asset
{
    public sealed class ProGetAssetDirectoryListerTests
    {
        private readonly ProGetConfiguration _config;

        public ProGetAssetDirectoryListerTests()
        {
            _config = new ProGetConfiguration
            {
                ProGetUser = "testuser",
                ProGetPassword = "password"
            };
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

            using(var server = FluentMockServer.Start())
            {
                server.Given(Request.Create().WithPath(assetDirectoryUri).UsingGet())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBodyAsJson(jsonList));
                var asset = new ProGetAssetDirectoryLister(_config);
                var result = asset.ListDirectory($"http://localhost:{server.Ports[0]}{assetDirectoryUri}", true);
                Assert.Equal(2, result.Count);
            }
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

            using(var server = FluentMockServer.Start())
            {
                server.Given(Request.Create().WithPath(assetDirectoryUri).UsingGet())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBodyAsJson(jsonList));

                var asset = new ProGetAssetDirectoryLister(_config);
                var result = asset.ListDirectory($"http://localhost:{server.Ports[0]}{assetDirectoryUri}");
                Assert.Single(result);
            }
        }
    }
}
