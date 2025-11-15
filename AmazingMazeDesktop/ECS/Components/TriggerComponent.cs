namespace AmazingMazeDesktop.ECS.Components;

public class TriggerComponent
{
        public bool IsArmed;
        public bool IsTriggered;
        public required TriggerType Type;     
        public required TriggerAction Action;
        public int ActivatingEntity;
}