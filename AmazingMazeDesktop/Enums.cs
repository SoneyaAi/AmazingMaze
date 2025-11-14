namespace AmazingMazeDesktop;

public enum State
{
    Idle,
    Walk,
    Attack,
    Hurt,
    Die
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