using PolyLab3DStudio.Core;

namespace PolyLab3DStudio.ViewModels;

/// <summary>One row of the 환경 점검 panel on the settings screen.</summary>
public sealed class ToolStatusRowViewModel(ToolStatus status)
{
    public string Name { get; } = status.Name;

    public string English { get; } = status.English;

    public string When { get; } = status.When;

    public string Detail { get; } = status.Detail;

    /// <summary>Install command when missing, or a follow-up tip when found.</summary>
    public string Install { get; } = status.Install;

    public bool IsFound { get; } = status.State == ToolState.Found;

    public string StateLabel { get; } = status.State == ToolState.Found ? "확인됨" : "찾지 못함";
}
