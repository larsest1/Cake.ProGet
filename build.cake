#tool "nuget:?package=xunit.runner.console"
#tool "docfx.msbuild"

#load "./build/parameters.cake"

var Parameters = BuildParameters.Load(Context, BuildSystem);

Task("Clean")
	.Does(() => {
        Information("Cleaning working directory...");
		CleanDirectories("./src/**/bin");
        CleanDirectories("./src/**/obj");
		CleanDirectories("./tests/**/bin");
		CleanDirectories("./tests/**/obj");
	});

Task("Clean-All")
    .IsDependentOn("Clean");

Task("Restore-NuGet-Packages")
    .Does(() => {
        NuGetRestore(
            Parameters.SolutionFile,
            new NuGetRestoreSettings {
                Source = new List<string>{ "https://www.nuget.org/api/v2" }
            }
        );
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() => {
        MSBuild(Parameters.SolutionFile, settings =>
            settings.SetConfiguration(Parameters.Configuration)
        );
    });

Task("Clean-Build")
    .IsDependentOn("Clean-All")
    .IsDependentOn("Build");

Task("Fast-Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() => {
        MSBuild(Parameters.SolutionFile, settings =>
            settings
            .SetConfiguration(Parameters.Configuration)
            .WithProperty("SourceAnalysisTreatErrorsAsWarnings", "True")
        );
    });

 Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() => {
        XUnit2("./tests/**/bin/**/*.Tests.dll");
    });

Task("Default")
    .Does(() => {
        RunTarget("Build");
    });

RunTarget(Parameters.Target);