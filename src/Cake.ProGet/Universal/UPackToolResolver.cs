using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ProGet.Universal
{
    /// <summary>
    /// Represents a UPack tool resolver.
    /// </summary>
    public interface IUPackToolResolver
    {
        /// <summary>
        /// Resolves the path to upack.exe.
        /// </summary>
        /// <returns>The path to upack.exe.</returns>
        FilePath ResolvePath();
    }

    /// <summary>
    /// Contains UPack tool resolver functionality
    /// </summary>
    public sealed class UPackToolResolver : IUPackToolResolver
    {
        private const string UPackExe = "upack.exe";

        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IToolLocator _tools;

        private IFile _cachedPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="UPackToolResolver" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="tools">The tool locator</param>
        public UPackToolResolver(IFileSystem fileSystem, ICakeEnvironment environment, IToolLocator tools)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (tools == null)
            {
                throw new ArgumentNullException(nameof(tools));
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _tools = tools;
        }

        /// <summary>
        /// Resolves the path to upack.exe.
        /// </summary>
        /// <returns>The path to upack.exe.</returns>
        public FilePath ResolvePath()
        {
            // Check if path allready resolved
            if (_cachedPath != null && _cachedPath.Exists)
            {
                return _cachedPath.Path;
            }

            // Try to resolve it with the regular tool resolver.
            var toolsExe = _tools.Resolve(UPackExe);
            if (toolsExe != null)
            {
                var toolsFile = _fileSystem.GetFile(toolsExe);
                if (toolsFile.Exists)
                {
                    _cachedPath = toolsFile;
                    return _cachedPath.Path;
                }
            }

            // Check if path set to environment variable
            var upackInstallationFolder = _environment.GetEnvironmentVariable("UPackInstall");
            if (!string.IsNullOrWhiteSpace(upackInstallationFolder))
            {
                var envFile = _fileSystem.GetFile(System.IO.Path.Combine(upackInstallationFolder, UPackExe));
                if (envFile.Exists)
                {
                    _cachedPath = envFile;
                    return _cachedPath.Path;
                }
            }

            // Last resort try path
            var envPath = _environment.GetEnvironmentVariable("path");
            if (!string.IsNullOrWhiteSpace(envPath))
            {
                var pathFile = envPath
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(path => _fileSystem.GetDirectory(path))
                    .Where(path => path.Exists)
                    .Select(path => path.Path.CombineWithFilePath(UPackExe))
                    .Select(_fileSystem.GetFile)
                    .FirstOrDefault(file => file.Exists);

                if (pathFile != null)
                {
                    _cachedPath = pathFile;
                    return _cachedPath.Path;
                }
            }

            throw new CakeException("Could not locate upack.exe.");
        }
    }
}
