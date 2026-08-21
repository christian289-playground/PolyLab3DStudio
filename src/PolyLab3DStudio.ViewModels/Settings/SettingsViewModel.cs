using PolyLab3DStudio.Core;

namespace PolyLab3DStudio.ViewModels;

/// <summary>The settings screen; values live on the shell and persist immediately.</summary>
public sealed partial class SettingsViewModel : ObservableObject
{
    public SettingsViewModel(ShellViewModel shell)
    {
        Shell = shell;
        RefreshTools();
    }

    public ShellViewModel Shell { get; }

    /// <summary>환경 점검 rows: the external tools 코스 00 / 트랙 C need.</summary>
    public ObservableCollection<ToolStatusRowViewModel> Tools { get; } = [];

    [ObservableProperty] private string _toolsCheckedAt = "";

    [ObservableProperty] private string _toolsSummary = "";

    /// <summary>Re-runs the probes so the panel reflects a tool installed while the app was open.</summary>
    [RelayCommand]
    public void RefreshTools()
    {
        Tools.Clear();
        int found = 0;
        foreach (ToolStatus status in ToolProbe.ProbeAll())
        {
            var row = new ToolStatusRowViewModel(status);
            if (row.IsFound)
            {
                found++;
            }

            Tools.Add(row);
        }

        ToolsCheckedAt = $"{DateTime.Now:HH:mm:ss} 확인";
        ToolsSummary = $"{found}/{Tools.Count} 확인됨";
    }

    [RelayCommand]
    private void Back() => Shell.SettingsBack();

    [RelayCommand]
    private void ResetProgress() => Shell.ResetProgress();
}
