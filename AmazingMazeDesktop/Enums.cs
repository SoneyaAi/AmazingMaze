namespace AmazingMazeDesktop;

public enum MoveState
{
    Idle,
    Walk,
    Attack,
    Hurt,
    Die
}

public enum EnemyStateId
{
    Idle,
    Patrol,
    Chase,
    Search,
    Attack,
    Dead
}


public enum Facing
{
    South,
    West,
    East,
    North
}

public enum TriggerType
{
    OnEnter,
    OnExit
}

public enum TriggerAction
{
    LoadPreviousLevel,
    LoadNextLevel,
    OpenDoor,
    ShowMessage
}

public enum Tags
{
    Player,
    Projectile,
    Enemy,
    Spawner,
    Wall,
    Trigger
}