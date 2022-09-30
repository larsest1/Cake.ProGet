using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.ProGet.Universal;
using Cake.Testing;

namespace Cake.ProGet.Tests.Universal.Fixtures
{
    internal sealed class UPackToolResolverFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public FakeEnvironment Environment { get; set; }
        public ToolLocator Tools { get; set; }

        public UPackToolResolverFixture(FakeEnvironment environment = null)
        {
            this.Environment = environment ?? FakeEnvironment.CreateUnixEnvironment();
            this.FileSystem = new FakeFileSystem(this.Environment);
            this.Tools = new ToolLocator(
                this.Environment,
                new ToolRepository(this.Environment),
                new ToolResolutionStrategy(this.FileSystem, this.Environment, new Globber(this.FileSystem, this.Environment), new FakeConfiguration(), new FakeLog()));
        }

        public FilePath Resolve()
        {
            var resolver = new UPackToolResolver(this.FileSystem, this.Environment, this.Tools);
            return resolver.ResolvePath();
        }
    }
}
