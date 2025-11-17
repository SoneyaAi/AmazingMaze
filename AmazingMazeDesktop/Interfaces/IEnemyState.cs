using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;

namespace AmazingMazeDesktop.Interfaces;

public interface IEnemyState
{
    void Enter(Entity entity);
    void Update(Entity entity, GameTime gameTime);
    void Exit(Entity entity);
}