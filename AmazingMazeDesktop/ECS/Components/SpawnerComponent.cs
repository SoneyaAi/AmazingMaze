using MonoGame.Extended.Timers;

namespace AmazingMazeDesktop.Components;

public class SpawnerComponent(int spawnTime)
{
    public enum SpawnerType
    {
        Player,
        Enemy
    }
    
    public SpawnerType Type = SpawnerType.Player;
    public CountdownTimer CountdownTimer = new CountdownTimer(spawnTime);
}