namespace PolyLab3DStudio.Controls;

public enum CommandRowKind
{
    /// <summary>A copyable terminal command — gets its own scroller and copy button.</summary>
    Command,

    /// <summary>A full-line <c>#</c> comment shown as a label above the commands it explains.</summary>
    Comment,

    /// <summary>A blank line from the source, kept so authored grouping survives.</summary>
    Spacer,
}
