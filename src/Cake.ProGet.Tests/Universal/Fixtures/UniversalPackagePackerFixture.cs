using Cake.ProGet.Universal.Pack;
using Cake.Testing;

namespace Cake.ProGet.Tests.Universal.Fixtures
{
    internal sealed class UniversalPackagePackerFixture : UPackFixture<UniversalPackagePackSettings>
    {
        public UniversalPackagePackerFixture()
        {
            this.FileSystem.CreateFile("./test.uspec");
            this.FileSystem.CreateDirectory("./test/pack-source");

            this.Settings.MetadataFilePath = "./test.uspec";
            this.Settings.SourceDirectory = "./test/pack-source";
        }

        protected override void RunTool()
        {
            var tool = new UniversalPackagePacker(this.FileSystem, this.Environment, this.ProcessRunner, this.Tools, this.Resolver);
            tool.Execute(this.Settings);
        }
    }
}