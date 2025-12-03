using AmazingMazeDesktop.ECS.StateContext;
using AmazingMazeDesktop.Interfaces;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;

namespace AmazingMazeDesktop.ECS.States;

public class PatrolEnemyState(IPlayerTracker playerTracker) : IEnemyState
{
    public void Enter(Entity entity)
    {
        
    }

    public void Update(Entity entity, GameTime gameTime, EnemyStateContext enemyStateContext)
    {
       
    }

    public void Exit(Entity entity)
    {
        
    }
}