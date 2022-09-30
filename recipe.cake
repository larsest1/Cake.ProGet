#load nuget:?package=Cake.Recipe&version=3.0.1

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Cake.ProGet",
                            repositoryOwner: "cake-contrib",
                            repositoryName: "Cake.ProGet",
                            shouldCalculateVersion: true);

((CakeTask)BuildParameters.Tasks.UploadCoverallsReportTask.Task).Actions.Clear();

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

BuildParameters.Tasks.CreateNuGetPackagesTask.IsDependentOn("Download-Upack");

Task("Download-Upack")
    .Does(() => {
        var upackZipFile = $"{BuildParameters.Paths.Directories.Build}/temp/upack-net6.0.zip";
        var upackDirectory = $"{BuildParameters.Paths.Directories.Build}/Upack";

        DownloadFile("https://github.com/Inedo/upack/releases/download/3.0.0/upack-net6.0.zip", upackZipFile);
        Unzip(upackZipFile, upackDirectory);
    });

Build.RunDotNetCore();
