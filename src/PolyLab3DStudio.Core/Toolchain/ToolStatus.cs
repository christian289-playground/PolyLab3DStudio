namespace PolyLab3DStudio.Core;

/// <summary>
/// One row of the 환경 점검 panel: whether an external tool the curriculum uses
/// was found on this PC, plus when it is needed and how to install it.
/// </summary>
/// <param name="Name">Display name, e.g. <c>Blender</c>.</param>
/// <param name="English">Mono sub-label shown next to the name.</param>
/// <param name="When">Which part of the curriculum needs it.</param>
/// <param name="Detail">What the probe found (version/path) or why it could not tell.</param>
/// <param name="Install">Copy-paste install command, or the site to download from.</param>
public sealed record ToolStatus(
    string Name,
    string English,
    ToolState State,
    string When,
    string Detail,
    string Install);
