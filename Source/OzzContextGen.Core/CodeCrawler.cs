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
        public static readonly string[] DefaultExcludedFolders = CtxDefaults.ExcludedFolders;

        public CodeCrawler() : this(".cs") { }

        public CodeCrawler(params string[] suffixes) : this((IEnumerable<string>)suffixes, null) { }

        public CodeCrawler(IEnumerable<string> suffixes, IEnumerable<string>? excludedFolders)
        {
            Suffixes = new(suffixes, StringComparer.OrdinalIgnoreCase);

            ExcludedFolders = excludedFolders != null
                ? new(excludedFolders, StringComparer.OrdinalIgnoreCase)
                : new(DefaultExcludedFolders, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// The file suffixes to look for when crawling the directory. By default, it includes ".cs" for C# source files, but it can be customized to include other file types if needed.
        /// </summary>
        public HashSet<string> Suffixes { get; }

        // Build result organization: Exclude common build and version control folders to avoid unnecessary files
        public HashSet<string> ExcludedFolders { get; }

        private static readonly HashSet<string> PrioritySuffixSet = new(CtxDefaults.PrioritySuffixes, StringComparer.OrdinalIgnoreCase);
        private static readonly HashSet<string> PriorityFileNameSet = new(CtxDefaults.PriorityFileNames, StringComparer.OrdinalIgnoreCase);

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
                var tmpList = new List<string>();
                foreach (var suffix in Suffixes)
                {
                    tmpList.AddRange(Directory.GetFiles(path, $"*{suffix}"));
                }

                // 1. Specific priority file names (e.g., package.json, tsconfig.json)
                files.AddRange(tmpList.Where(f => PriorityFileNameSet.Contains(Path.GetFileName(f)))
                                      .OrderBy(f => Array.IndexOf(CtxDefaults.PriorityFileNames, Path.GetFileName(f).ToLowerInvariant()))
                                      .ThenBy(f => f));

                // 2. Priority suffixes (e.g., solution, project, asmdef) excluding any already added above
                files.AddRange(tmpList.Where(f => !PriorityFileNameSet.Contains(Path.GetFileName(f))
                                               && PrioritySuffixSet.Contains(Path.GetExtension(f)))
                                      .OrderBy(f => Array.IndexOf(CtxDefaults.PrioritySuffixes, Path.GetExtension(f).ToLowerInvariant()))
                                      .ThenBy(f => f));

                // 3. All remaining source files sorted alphabetically
                files.AddRange(tmpList.Where(f => !PriorityFileNameSet.Contains(Path.GetFileName(f))
                                               && !PrioritySuffixSet.Contains(Path.GetExtension(f)))
                                      .OrderBy(f => f));

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
