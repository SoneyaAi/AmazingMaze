namespace AmazingMazeDesktop.Components;

public class StateComponent
{
    public State Current;
    public Facing Facing;
    public State? Queued;     // żądany następny stan, gdy Lock aktywny
    public bool LockUntilEnd;     // np. Attack/Hurt/Die nieprzerywalne
    public float TimeInState;     // do okien i cooldownów
}