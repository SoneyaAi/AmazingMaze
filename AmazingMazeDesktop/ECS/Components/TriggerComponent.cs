namespace AmazingMazeDesktop.ECS.Components;

public class TriggerComponent
{
        public bool IsArmed = false;
        public bool IsTriggered = false;
        public required TriggerType Type;     
        public required TriggerAction Action;
        public int ActivatingEntity;
}