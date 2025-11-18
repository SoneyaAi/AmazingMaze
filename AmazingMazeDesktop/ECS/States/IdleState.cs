using System.Diagnostics;
using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.ECS.StateContext;
using AmazingMazeDesktop.Interfaces;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;

namespace AmazingMazeDesktop.ECS.States;

public class IdleState : IEnemyState
{
    public void Enter(Entity entity)
    {
        
    }

    public void Update(Entity entity, GameTime gameTime, EnemyStateContext enemyStateContext)
    {
        var stateMachine = entity.Get<StateComponent>();
        var transform = entity.Get<Transform2>();

        var dist = Vector2.Distance(transform.Position, enemyStateContext.PlayerPosition);//    enemy.DistanceToPlayer();

        // jeśli za daleko – zgubił gracza, przechodzimy do Search lub Patrol
        if (entity.Has<PathComponent>() && entity.Get<PathComponent>().Path.Count < stateMachine.AgroRange)
        {
            stateMachine.NextStateId = EnemyStateId.Chase;// enemy.ChangeState(EnemyStateId.Search);
            return;
        }
    }

    public void Exit(Entity entity)
    {
    }
}