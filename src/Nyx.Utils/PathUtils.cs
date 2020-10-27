using System.IO;
using System.Linq;

namespace Nyx.Utils
{
    public static class PathUtils
    {
        public static string GetFullPath(string path)
        {
            return Path.Combine(TryGetSolutionDirectoryInfo().FullName, path);
        }

        public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while ((directory != null) && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            return directory;
        }
    }
}