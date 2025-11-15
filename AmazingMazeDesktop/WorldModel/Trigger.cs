using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.WorldModel;

public class Trigger
{
    public Point TileCoordinates { get; set; }
    public TriggerType TriggerType { get; set; }
    public TriggerAction Action { get; set; }
}