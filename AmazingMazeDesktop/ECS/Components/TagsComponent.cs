using System.Collections.Generic;

namespace AmazingMazeDesktop.Components;

public enum Tags
{
    Player,
    Projectile,
    Enemy,
    Spawner,
    Wall
}

public class TagsComponent
{
    public List<Tags> TagsList { get; set; } = new();
    public bool HasTag(Tags tag) => TagsList.Contains(tag);
}