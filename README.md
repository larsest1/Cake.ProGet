# Cake.ProGet

`Cake.ProGet` is a [Cake](http://cakebuild.net) [add-in](http://cakebuild.net/docs/fundamentals/preprocessor-directives) that exposes [Inedo ProGet](https://inedo.com/proget) functionality to your build scripts.

 * Provides aliases for the functionality found in `upack.exe` ([available here](https://github.com/Inedo/upack/releases/download/3.0.0/upack-net6.0.zip)) .
 * Provides aliases for the [Asset Directory](https://inedo.com/support/documentation/proget/core-concepts/asset-directories) functionality.

[![Build status](https://ci.appveyor.com/api/projects/status/2tst3twn2nu4qool?svg=true)](https://ci.appveyor.com/project/cakecontrib/cake-proget)


 ## Table of Contents

- [Cake.ProGet](#cakeproget)
  - [Table of Contents](#table-of-contents)
  - [Building](#building)
  - [Pre-Requisites](#pre-requisites)
  - [Examples](#examples)
    - [Universal Package](#universal-package)
    - [ProGet Assets](#proget-assets)
  - [Contributing](#contributing)
  - [License](#license)
  - [Code of Conduct](#code-of-conduct)

## Building
This package is built using [Cake.Recipe](https://github.com/cake-contrib/Cake.Recipe)
```
> .\build.ps1
```

## Pre-Requisites

The Cake.ProGet add-in is intended to be used in conjunction with your organization's ProGet asset repository.  The alias methods for the Universal Package functionality will require your build environment to have `upack.exe` available to it.

 There are several ways you can configure your build environment for the `upack.exe`:
  - Add the installation path to your `%PATH%` variable.
  - Create environment variable(s) `%UPackInstall%` with the correct installation path set.
  - Host a local (to your organization) nuget package with the UPack executable and resolve the tool using a `#tool` directive.  i.e., `#tool "nuget:?package=YourPackage"`

  The order of resolution will be (first to last): local tool resolution via `#tool`, then the `%UPackInstall%` variable, and finally `%PATH%` variable.  This gives you flexibility in resolving a more specific version in a given build or allowing a system-wide default.

## Examples

### Universal Package
A full example [can be found here](https://github.com/cake-contrib/cake.proget/blob/master/examples/upack)

```
#addin "nuget:?package=Cake.ProGet"

Task("Create-Package")
    .Description("Creates a universal package")
    .Does(() => {
        UPackPack(new UniversalPackagePackSettings(
            "./UPack.Example.uspec",
            "./content"
        ));
    });

Task("Publish-Package")
    .Description("Publishes a universal package")
    .IsDependentOn("Create-Package")
    .Does(() => {
        foreach(var item in GetFiles("./*.upack"))
        {
            UPackPush(new UniversalPackagePushSettings(item.FullPath, "http://your-proget-server/upack/packages/"));
        }
    });
```

### ProGet Assets
A full example [can be found here](https://github.com/cake-contrib/cake.proget/blob/master/examples/assets)

```
#addin "nuget:?package=Cake.ProGet"

var Version = "0.1.0";

Task("Create-Package")
    .Description("Creates a package as a build artifact")
    .IsDependentOn("Clean")
    .Does(() => {
        Zip("./content", "artifact.zip");
    });

Task("Publish-Package")
    .Description("Publishes a ProGet asset")
    .IsDependentOn("Create-Package")
    .Does(() => {
        // TODO: this endpoint would be formatted based on your specific hosting environment/naming scheme.
        var assetUri = string.Format("http://your-proget-server/endpoints/asset-dir/content/test-artifact-{0}.zip", Version);
        PushAsset("./artifact.zip", assetUri, new ProGetConfiguration());
    });
```

## Contributing

If you're thinking about contributing to Cake.ProGet, please make sure you've read the [contribution guidelines](https://github.com/cake-contrib/cake.proget/blob/master/CONTRIBUTING.md) before creating your first pull request.

* Fork the repository.
* Create a branch to work in.
* Make your feature addition or bug fix.
* Don't forget the unit tests.
* Send a pull request.

## License

Copyright © Apprenda Inc., and contributors.

Cake.ProGet is provided as-is under the MIT license. For more information see [LICENSE](https://github.com/cake-contrib/cake.proget/blob/master/LICENSE).

## Code of Conduct

This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior in our community.