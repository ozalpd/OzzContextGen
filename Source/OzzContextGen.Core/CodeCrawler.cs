namespace OzzContextGen.Core
{
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
        /// Recursively scans the specified directory and returns a list of all C# source files (.cs), excluding certain folders.
        /// </summary>
        /// <param name="path">The root directory to start scanning from.</param>
        /// <returns>A list of file paths for all C# source files found.</returns>
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
