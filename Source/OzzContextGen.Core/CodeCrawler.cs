namespace OzzContextGen.Core
{
    /// <summary>
    /// The CodeCrawler class is responsible for recursively scanning a specified directory and collecting all files that match
    /// certain file suffixes (e.g., ".cs" for C# source files). It also allows for the exclusion of specific folders (like "bin",
    /// "obj", ".git", etc.) to avoid unnecessary files during the scan. The class provides a method to retrieve the absolute
    /// paths of all matching source files found in the directory and its subdirectories.
    /// </summary>
    public class CodeCrawler
    {
        public CodeCrawler() : this(".cs") { }

        public CodeCrawler(params string[] suffixes)
        {
            Suffixes = new(suffixes, StringComparer.OrdinalIgnoreCase);

            ExcludedFolders = new(StringComparer.OrdinalIgnoreCase)
            {
                "bin", "obj", ".git", ".vs", "packages", "node_modules"
            };
        }

        /// <summary>
        /// The file suffixes to look for when crawling the directory. By default, it includes ".cs" for C# source files, but it can be customized to include other file types if needed.
        /// </summary>
        public HashSet<string> Suffixes { get; }

        // Build result organization: Exclude common build and version control folders to avoid unnecessary files
        public HashSet<string> ExcludedFolders { get; }

        /// <summary>
        /// Recursively scans the specified directory and returns all files whose extension
        /// matches one of the configured <see cref="Suffixes"/>, excluding <see cref="ExcludedFolders"/>.
        /// </summary>
        /// <param name="path">The root directory to start scanning from.</param>
        /// <returns>A list of absolute file paths for all matching source files found.</returns>
        public IEnumerable<string> GetCodeFiles(string path)
        {
            var files = new List<string>();
            try
            {
                foreach (var suffix in Suffixes)
                {
                    files.AddRange(Directory.GetFiles(path, $"*{suffix}"));
                }

                foreach (var directory in Directory.GetDirectories(path))
                {
                    string folderName = Path.GetFileName(directory);
                    if (!ExcludedFolders.Contains(folderName))
                    {
                        files.AddRange(GetCodeFiles(directory));
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // For now, we just skip directories we can't access.
            }

            return files;
        }
    }
}
