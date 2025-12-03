using System.Collections.Generic;
using AmazingMazeDesktop.ECS.States;
using AmazingMazeDesktop.Interfaces;

namespace AmazingMazeDesktop.Components;

public class StateComponent
{



    public MoveState Current;
    public Facing Facing;
    public MoveState? Queued; // żądany następny stan, gdy Lock aktywny
    public bool LockUntilEnd; // np. Attack/Hurt/Die nieprzerywalne
    public float TimeInState; // do okien i cooldownów
    public EnemyStateId CurrentStateId;
    public EnemyStateId NextStateId = EnemyStateId.Idle;
    public IEnemyState CurrentState;
    public int AgroRange = 10;
}