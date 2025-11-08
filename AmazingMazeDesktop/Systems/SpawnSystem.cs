using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.Factory;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Timers;

namespace AmazingMazeDesktop.Systems;

public class SpawnSystem : EntityUpdateSystem
{
    private CountdownTimer _countdownTimer;
    private MathHelper.Random _random;
    private EntityFactory _entityFactory;
    private FastRandom rng = new();
    public Maze Maze;
    public SpawnSystem(EntityFactory factory) : base(Aspect.All(typeof(TagsComponent)))
    {
        _entityFactory = factory;
        _countdownTimer = new CountdownTimer(10);
        _countdownTimer.Start();
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        
    }

    public override void Update(GameTime gameTime)
    {
        _countdownTimer.Update(gameTime);
        if (_countdownTimer.State != TimerState.Completed) 
            return;
        
        while (true)
        {
            var x = rng.Next(0, Maze.MazeSchema.GetLength(1) -1);
            var y = rng.Next(0, Maze.MazeSchema.GetLength(0) -1);
            if (Maze.MazeSchema[y,x] != 0) 
                continue;
            
            _entityFactory.BuildEntity(new EnemyBuilderArgs()
            {
                Position = Conversions.CellToWorld(new Point(x,y)),
                Speed = 30
            });
            break;
        }

        _countdownTimer.Restart();
    }
}