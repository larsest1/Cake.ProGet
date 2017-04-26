using Cake.ProGet.Universal.Unpack;
using Cake.Testing;

namespace Cake.ProGet.Tests.Universal.Fixtures
{
    internal sealed class UniversalPackageUnpackerFixture : UPackFixture<UniversalPackageUnpackSettings>
    {
        public UniversalPackageUnpackerFixture()
        {
            this.FileSystem.CreateFile("./test/test-package.upack");
            this.FileSystem.CreateDirectory("./test/target-dir");

            this.Settings.Package = "./test/test-package.upack";
            this.Settings.TargetDirectory = "./test/target-dir";
        }

        protected override void RunTool()
        {
            var tool = new UniversalPackageUnpacker(this.FileSystem, this.Environment, this.ProcessRunner, this.Tools, this.Resolver);
            tool.Execute(this.Settings);
        }
    }
}