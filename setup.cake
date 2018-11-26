#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&version=0.3.0-unstable0427

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context, 
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Cake.ProGet",
                            repositoryOwner: "cake-contrib",
                            repositoryName: "Cake.ProGet");

((CakeTask)BuildParameters.Tasks.UploadCoverallsReportTask.Task).Actions.Clear();

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                            dupFinderExcludePattern: new string[] { BuildParameters.RootDirectoryPath + "/src/Cake.ProGet.Tests/*.cs" },
                            testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* -[FluentAssertions*]* ",
                            testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs",
                            dupFinderDiscardCost: 150,
                            dupFinderThrowExceptionOnFindingDuplicates: false);

BuildParameters.Tasks.CreateNuGetPackagesTask.IsDependentOn("Download-Upack");

Task("Download-Upack")
	.Does(() => {
		DownloadFile(
			"https://github.com/Inedo/upack/releases/download/upack-2.2.2.2/upack.exe",
			BuildParameters.Paths.Directories.Build.GetFilePath("upack.exe"));
	});
Build.Run();
