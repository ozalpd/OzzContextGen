using OzzContextGen.Core.Models;

namespace OzzContextGen.Core;

/// <summary>
/// Heuristic token estimator. Classifies characters into buckets with different
/// chars-per-token divisors so the estimate self-adjusts to non-English content.
/// </summary>
public static class TokenEstimator
{
    private const double AsciiDivisor = 4.0;
    private const double ExtendedLatinDivisor = 2.5;
    private const double CjkDivisor = 1.5;
    private const double OtherDivisor = 2.0;

    public static int EstimateTokens(string text) => Estimate(text).EstimatedTokens;

    public static TokenEstimate Estimate(string text)
    {
        if (string.IsNullOrEmpty(text))
            return new TokenEstimate(0, 0, 0, 0, 0);

        int ascii = 0, extendedLatin = 0, cjk = 0, other = 0;

        foreach (char c in text)
        {
            if (char.IsSurrogate(c))
                continue; // count supplementary-plane chars via their leading surrogate pair as Other

            if (c < 128)
                ascii++;
            else if (c < 0x250) // Latin-1 Supplement, Latin Extended-A/B
                extendedLatin++;
            else if (IsCjk(c))
                cjk++;
            else
                other++;
        }

        int tokens = (int)Math.Ceiling(
            ascii / AsciiDivisor +
            extendedLatin / ExtendedLatinDivisor +
            cjk / CjkDivisor +
            other / OtherDivisor);

        return new TokenEstimate(tokens, ascii, extendedLatin, cjk, other);
    }

    private static bool IsCjk(char c) =>
        (c >= '一' && c <= '鿿') ||   // CJK Unified Ideographs
        (c >= '぀' && c <= 'ヿ') ||   // Hiragana + Katakana
        (c >= '가' && c <= '힣');     // Hangul Syllables
}
