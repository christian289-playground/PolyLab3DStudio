using System.Runtime.InteropServices;

namespace PolyLab3DStudio.Core;

/// <summary>
/// Detects the external tools 코스 00 / 트랙 C rely on, for the 환경 점검 panel.
/// Deliberately filesystem- and PATH-only: launching winget or blender to ask for
/// a version would stall the UI on a cold start, so every probe here is a cheap
/// lookup that can be re-run from the refresh button as often as the user likes.
/// A <see cref="ToolState.Missing"/> result therefore means "not where we know how
/// to look" — a portable copy or a custom install path is reported as missing.
/// </summary>
public static class ToolProbe
{
    public static IReadOnlyList<ToolStatus> ProbeAll() =>
    [
        ProbeWinget(),
        ProbeDotnetSdk(),
        ProbeBlender(),
        ProbeStride(),
    ];

    private static ToolStatus ProbeWinget()
    {
        string? path = FindOnPath("winget.exe");
        return new ToolStatus(
            "winget",
            "WINDOWS PACKAGE MANAGER",
            path is null ? ToolState.Missing : ToolState.Found,
            "코스 00 · 다른 도구를 설치할 때",
            path is null
                ? "PATH에서 winget.exe를 찾지 못했어요. Windows 10에서는 없을 수 있습니다."
                : path,
            path is null
                ? "Microsoft Store에서 \"앱 설치 관리자\" 설치 (Microsoft.AppInstaller)"
                : "터미널에서 winget --version 으로 버전을 확인할 수 있어요.");
    }

    private static ToolStatus ProbeDotnetSdk()
    {
        string? newest = null;
        Version? newestVersion = null;
        foreach (string root in DotnetRoots())
        {
            foreach (string dir in SafeDirectories(Path.Combine(root, "sdk")))
            {
                string name = Path.GetFileName(dir);

                // Folder names are versions ("10.0.303") and may carry a preview suffix
                // ("10.0.100-preview.1"). Compare numerically — a plain string compare
                // would rank 8.0.419 above 10.0.303.
                _ = Version.TryParse(name.Split('-')[0], out Version? parsed);
                bool better = newest is null
                    || (parsed is not null && (newestVersion is null || parsed > newestVersion))
                    || (parsed is null && newestVersion is null && string.CompareOrdinal(name, newest) > 0);

                if (better)
                {
                    newest = name;
                    newestVersion = parsed ?? newestVersion;
                }
            }
        }

        // The app is running, so the .NET runtime is present by definition; only the
        // SDK (needed to build PolyLab from source) is genuinely in question.
        string runtime = RuntimeInformation.FrameworkDescription;
        return new ToolStatus(
            ".NET SDK",
            "DOTNET SDK",
            newest is null ? ToolState.Missing : ToolState.Found,
            "폴리랩을 소스에서 빌드할 때",
            newest is null
                ? $"SDK를 찾지 못했어요. 실행 중인 런타임은 {runtime} 입니다 — 앱을 쓰는 데는 문제없어요."
                : $"SDK {newest} · 실행 중인 런타임 {runtime}",
            newest is null
                ? "winget install -e --id Microsoft.DotNet.SDK.10"
                : "dotnet --info 로 전체 목록을 볼 수 있어요.");
    }

    private static ToolStatus ProbeBlender()
    {
        string? exe = FindOnPath("blender.exe");
        string? version = null;

        if (exe is null)
        {
            // winget/installer layout: <Program Files>\Blender Foundation\Blender <ver>\blender.exe
            foreach (string programFiles in ProgramFilesRoots())
            {
                foreach (string dir in SafeDirectories(Path.Combine(programFiles, "Blender Foundation")))
                {
                    string candidate = Path.Combine(dir, "blender.exe");
                    string folder = Path.GetFileName(dir);
                    if (SafeFileExists(candidate) && (version is null || string.CompareOrdinal(folder, version) > 0))
                    {
                        exe = candidate;
                        version = folder;
                    }
                }
            }
        }

        return new ToolStatus(
            "Blender",
            "BLENDER",
            exe is null ? ToolState.Missing : ToolState.Found,
            "트랙 C 전체",
            exe is null
                ? "설치된 Blender를 찾지 못했어요. 포터블(zip) 버전은 자동으로 찾을 수 없습니다."
                : exe,
            exe is null
                ? "winget install -e --id BlenderFoundation.Blender"
                : "blender.exe는 PATH에 등록되지 않아요. 스크립트 실행에는 위 전체 경로를 씁니다.");
    }

    private static ToolStatus ProbeStride()
    {
        string launcher = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Programs",
            "Stride",
            "Stride.Launcher.exe");

        bool found = SafeFileExists(launcher);
        return new ToolStatus(
            "Stride 런처",
            "STRIDE LAUNCHER",
            found ? ToolState.Found : ToolState.Missing,
            "트랙 C 마지막 레슨",
            found
                ? $"{launcher} — 엔진 버전이 설치되었는지는 런처를 열어 확인하세요."
                : "Stride 런처를 찾지 못했어요. winget으로는 설치할 수 없습니다.",
            found
                ? "런처에서 엔진 버전을 선택 설치하면 Game Studio가 열려요."
                : "stride3d.net/download 에서 StrideSetup.exe 를 받으세요.");
    }

    // ---------------- lookup helpers (never throw) ----------------

    private static IEnumerable<string> DotnetRoots()
    {
        string? explicitRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");
        if (!string.IsNullOrWhiteSpace(explicitRoot))
        {
            yield return explicitRoot;
        }

        foreach (string programFiles in ProgramFilesRoots())
        {
            yield return Path.Combine(programFiles, "dotnet");
        }
    }

    private static IEnumerable<string> ProgramFilesRoots()
    {
        string x64 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        if (!string.IsNullOrEmpty(x64))
        {
            yield return x64;
        }

        string x86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        if (!string.IsNullOrEmpty(x86) && !string.Equals(x86, x64, StringComparison.OrdinalIgnoreCase))
        {
            yield return x86;
        }
    }

    private static string? FindOnPath(string exeName)
    {
        string? path = Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        foreach (string dir in path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            string candidate;
            try
            {
                candidate = Path.Combine(dir, exeName);
            }
            catch (ArgumentException)
            {
                continue;
            }

            if (SafeFileExists(candidate))
            {
                return candidate;
            }
        }

        return null;
    }

    private static IEnumerable<string> SafeDirectories(string parent)
    {
        try
        {
            return Directory.Exists(parent) ? Directory.EnumerateDirectories(parent) : [];
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException)
        {
            return [];
        }
    }

    private static bool SafeFileExists(string path)
    {
        try
        {
            return File.Exists(path);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException)
        {
            return false;
        }
    }
}
