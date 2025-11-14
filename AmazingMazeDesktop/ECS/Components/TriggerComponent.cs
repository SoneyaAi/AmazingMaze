namespace AmazingMazeDesktop.ECS.Components;

public class TriggerComponent
{
        public bool IsArmed;
        public bool IsTriggered;
        public TriggerType Type;      // Enter, Exit, Once, Many, itp.
        public TriggerAction Action;  // np. LoadNextLevel, OpenDoor itp.
        public int ActivatingEntity;
}