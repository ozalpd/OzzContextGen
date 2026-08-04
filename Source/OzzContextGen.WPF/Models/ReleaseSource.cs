using OzzMarkdown.Core.Models;


namespace OzzContextGen.WPF.Models;

internal class ReleaseSource : IReleaseSource
{
    public string CurrentVersion => AppVersion.Version;

    public string RepositoryName => "OzzContextGen";

    public string RepositoryApiUrl => "https://api.github.com/repos/ozalpd/OzzContextGen";

    public string RepositoryUrl => "https://github.com/ozalpd/OzzContextGen";
}