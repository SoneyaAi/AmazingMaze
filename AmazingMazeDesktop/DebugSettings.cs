using System.Data.SqlTypes;

namespace AmazingMazeDesktop;

public static class DebugSettings
{
    public static bool EnableDebug { get; set; } = false;
    public static bool ShowColliders { get; set; } = true;
    public static bool ShowPaths { get; set; } = true;
    public static bool ShowWaypoint { get; set; } = true;
}