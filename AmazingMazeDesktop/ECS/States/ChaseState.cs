using System.Numerics;
using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.ECS.StateContext;
using AmazingMazeDesktop.Interfaces;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace AmazingMazeDesktop.ECS.States;

public class ChaseState : IEnemyState
{                                                     
    public void Enter(Entity entity)
    {
        // animacja biegu, dźwięk itp.
    }

    public void Update(Entity entity, GameTime gameTime, EnemyStateContext enemyStateContext)
    {
        var stateMachine = entity.Get<StateComponent>();
        var transform = entity.Get<Transform2>();

        var dist = Vector2.Distance(transform.Position, enemyStateContext.PlayerPosition);//    enemy.DistanceToPlayer();

        // jeśli za daleko – zgubił gracza, przechodzimy do Search lub Patrol
        if (entity.Has<PathComponent>() && entity.Get<PathComponent>().Path.Count > stateMachine.AgroRange)
        {
            stateMachine.NextStateId = EnemyStateId.Idle;// enemy.ChangeState(EnemyStateId.Search);
            return;
        }

        
        // // jeśli blisko – atak
        // if (dist <= enemy.AttackRange)
        // {
        //     enemy.ChangeState(EnemyStateId.Attack);
        //     return;
        // }
        //
        // // ruch w stronę gracza
        // Vector2 dir = enemy.PlayerPosition - enemy.Position;
        // if (dir.LengthSquared() > 0.001f)
        // {
        //     dir.Normalize();
        //     float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //     enemy.Position += dir * enemy.Speed * dt;
        // }
    }

    public void Exit(Entity entity)
    {
        
    }
}