using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.ProGet.Universal;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.ProGet.Tests.Universal.Fixtures
{
    internal abstract class UPackFixture<TSettings> : UPackFixture<TSettings, ToolFixtureResult>
        where TSettings : ToolSettings, new()
    {
        protected override ToolFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new ToolFixtureResult(path, process);
        }
    }

    internal abstract class UPackFixture<TSettings, TFixtureResult> : ToolFixture<TSettings, TFixtureResult>
        where TSettings : ToolSettings, new()
        where TFixtureResult : ToolFixtureResult
    {
        public IUPackToolResolver Resolver { get; set; }
        public ICakeLog Log { get; set; }

        protected UPackFixture()
            : base("upack.exe")
        {
            Resolver = Substitute.For<IUPackToolResolver>();
            Log = Substitute.For<ICakeLog>();
        }
    }
}