using Cake.ProGet.Universal.Install;
using Cake.Testing;

namespace Cake.ProGet.Tests.Universal.Fixtures
{
    internal sealed class UniversalPackageInstallerFixture : UPackFixture<UniversalPackageInstallSettings>
    {
        public UniversalPackageInstallerFixture()
        {
            this.FileSystem.CreateDirectory("./target");

            this.Settings.TargetDirectory = "./target";
            this.Settings.Source = "http://proget.com/upack-feed";
            this.Settings.Package = "Test.Package";
        }

        protected override void RunTool()
        {
            var tool = new UniversalPackageInstaller(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Execute(Settings);
        }
    }
}
