namespace OzzContextGen.Core.Models;

/// <summary>
/// Result of a heuristic token estimation: the estimated token count plus the
/// character-class breakdown the estimate was computed from.
/// </summary>
public record TokenEstimate(int EstimatedTokens, int AsciiChars, int ExtendedLatinChars, int CjkChars, int OtherChars);
