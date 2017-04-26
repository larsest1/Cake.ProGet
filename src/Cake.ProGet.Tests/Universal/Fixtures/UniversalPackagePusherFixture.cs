using Cake.ProGet.Universal.Push;
using Cake.Testing;

namespace Cake.ProGet.Tests.Universal.Fixtures
{
    internal sealed class UniversalPackagePusherFixture : UPackFixture<UniversalPackagePushSettings>
    {
        public UniversalPackagePusherFixture()
        {
            this.FileSystem.CreateFile("./test/file.upack");

            this.Settings.Package = "./test/file.upack";
            this.Settings.Target = "http://proget.com/upack-feed";
        }

        protected override void RunTool()
        {
            var tool = new UniversalPackagePusher(this.FileSystem, this.Environment, this.ProcessRunner, this.Tools, this.Resolver);
            tool.Execute(this.Settings);
        }
    }
}