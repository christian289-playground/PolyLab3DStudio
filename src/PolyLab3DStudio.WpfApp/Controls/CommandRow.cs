namespace PolyLab3DStudio.Controls;

/// <summary>
/// One line of a terminal command block, as rendered by <see cref="CommandList"/>.
/// Presentation-only, so it lives beside the control instead of in the ViewModels
/// project: the curriculum stores a command block as one string, and splitting it
/// into rows is purely how the reading modal chooses to draw it.
/// </summary>
public sealed class CommandRow
{
    private CommandRow(CommandRowKind kind, string text, string? trailingComment)
    {
        Kind = kind;
        Text = text;
        TrailingComment = trailingComment;
    }

    public CommandRowKind Kind { get; }

    /// <summary>The command itself, with any trailing comment split off.</summary>
    public string Text { get; }

    /// <summary>Explanatory comment that followed the command on the same line.</summary>
    public string? TrailingComment { get; }

    public bool IsCommand => Kind == CommandRowKind.Command;

    public bool IsComment => Kind == CommandRowKind.Comment;

    public bool IsSpacer => Kind == CommandRowKind.Spacer;

    public bool HasTrailingComment => !string.IsNullOrEmpty(TrailingComment);

    /// <summary>
    /// Splits a command block into rows. Full-line <c>#</c> comments become labels,
    /// blank lines become spacers that keep the authored grouping, and everything else
    /// becomes an individually copyable command. A comment that trails a command
    /// (separated by two or more spaces) is peeled off so copying yields a command the
    /// terminal accepts verbatim.
    /// </summary>
    public static IReadOnlyList<CommandRow> Parse(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return [];
        }

        var rows = new List<CommandRow>();
        foreach (string raw in code.Replace("\r\n", "\n").Split('\n'))
        {
            string line = raw.TrimEnd();
            if (line.Trim().Length == 0)
            {
                // Collapse runs of blank lines; a single spacer is enough.
                if (rows.Count > 0 && !rows[^1].IsSpacer)
                {
                    rows.Add(new CommandRow(CommandRowKind.Spacer, "", null));
                }

                continue;
            }

            string trimmed = line.TrimStart();
            if (trimmed.StartsWith('#'))
            {
                rows.Add(new CommandRow(CommandRowKind.Comment, trimmed, null));
                continue;
            }

            (string command, string? comment) = SplitTrailingComment(trimmed);
            rows.Add(new CommandRow(CommandRowKind.Command, command, comment));
        }

        // A trailing spacer would add dead space under the last row.
        if (rows.Count > 0 && rows[^1].IsSpacer)
        {
            rows.RemoveAt(rows.Count - 1);
        }

        return rows;
    }

    private static (string Command, string? Comment) SplitTrailingComment(string line)
    {
        // Only treat '#' as a comment when it is set off by whitespace, so a '#' inside
        // a path or an argument stays part of the command.
        for (int i = 1; i < line.Length; i++)
        {
            if (line[i] != '#' || line[i - 1] != ' ')
            {
                continue;
            }

            string command = line[..i].TrimEnd();
            if (command.Length == 0)
            {
                break;
            }

            return (command, line[i..].Trim());
        }

        return (line, null);
    }
}
