using System.Collections.Generic;

namespace AmazingMazeDesktop.Components;



public class TagsComponent
{
    public List<Tags> TagsList { get; set; } = new();
    public bool HasTag(Tags tag) => TagsList.Contains(tag);
}