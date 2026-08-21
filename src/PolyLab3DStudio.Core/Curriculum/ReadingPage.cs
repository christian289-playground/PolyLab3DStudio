namespace PolyLab3DStudio.Core;

/// <summary>
/// One page of a reading lesson.
/// </summary>
/// <param name="Code">Optional code/terminal text shown under the paragraphs.</param>
/// <param name="Commands">
/// True when <paramref name="Code"/> is a list of terminal commands rather than a
/// source snippet. The reading modal then renders one copyable row per command
/// instead of a single block, so a learner can copy exactly one line at a time.
/// </param>
public sealed record ReadingPage(
    string Title,
    IReadOnlyList<string> Paragraphs,
    string? Code = null,
    bool Commands = false);
