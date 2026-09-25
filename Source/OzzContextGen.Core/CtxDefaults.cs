namespace OzzContextGen.Core;

/// <summary>
/// Centralized default values and fallback configurations for context generation profiles and scanners.
/// </summary>
public static class CtxDefaults
{
    /// <summary>
    /// Default folder names excluded during source scanning across all frontends.
    /// </summary>
    public static readonly string[] ExcludedFolders =
    [
        "bin", "obj", ".git", ".vs", ".vscode", "packages", "node_modules", "GeneratedCodes"
    ];

    /// <summary>
    /// Default system prompt used for LLM context generation.
    /// </summary>
    public const string DefaultSystemPrompt =
        "Act as a senior software developer. Analyze the provided project source code. Provide concise, accurate solutions following modern idioms and architectural best practices.";

    /// <summary>
    /// Curated sample system prompts for common LLM analysis scenarios.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, string> SampleSystemPrompts = new Dictionary<string, string>
    {
        ["General"] =
            "Act as a senior software developer. Analyze the provided project source code. Provide concise, accurate solutions following modern idioms and architectural best practices.",

        ["Security Review"] =
            "Act as a security auditor. Review the provided source code for vulnerabilities (such as OWASP Top 10), insecure dependencies, credential leaks, and improper input validation.",

        ["Refactoring & Architecture"] =
            "Act as a software architect. Identify SOLID violations, code smells, and tight coupling. Propose clean refactorings and maintainability improvements.",

        ["Code Review & QA"] =
            "Perform a thorough code review. Identify edge cases, potential runtime bugs, performance bottlenecks, and adherence to clean coding standards.",

        ["Documentation & Explanation"] =
            "Explain the core components, design decisions, and data flow of this codebase. Generate clear API summaries and usage examples where appropriate."
    };
}
