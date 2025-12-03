using System.Collections.Generic;
using System.Diagnostics;
using AmazingMazeDesktop.ECS;
using AmazingMazeDesktop.Services;
using AmazingMazeDesktop.WorldGeneration;
using AmazingMazeDesktop.WorldGeneration.Configs;
using AmazingMazeDesktop.WorldModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AmazingMazeDesktop;

public class GameContext
{
    public GameServices Services { get; }
    public Dungeon Dungeon => _dungeon;
    public int PlayerEntityId;
    public Level CurrentLevel { get; private set; }
    public ECSWorld World => _levelEcsWorldDictionary[CurrentLevel];
    public int _currentLevelIndex = 0;
    private Dungeon _dungeon;
    private ConfigsPackage _configs;
    private OrthographicCamera _camera;
    private GraphicsDevice _graphicsDevice;
    Dictionary<Level, ECSWorld> _levelEcsWorldDictionary = new Dictionary<Level, ECSWorld>();

    public GameContext(ConfigsPackage configs, OrthographicCamera camera, GraphicsDevice graphicsDevice)
    {
        Services = new GameServices()
        {
            PathfinderService = new AstarPathfinderService(this),
            CoordinatesTranslator = new CoordinatesTranslatorService(this),
            PlayerTracker = new PlayerTracker(this)
        };
        _configs = configs;
        _dungeon = new DungeonGenerator().Generate(_configs);
        CurrentLevel = _dungeon.Levels[_currentLevelIndex];
        var ecsWorld = new ECSWorld();
        _camera = camera;
        _graphicsDevice = graphicsDevice;
        ecsWorld.Initialize(this, camera, graphicsDevice);
        _levelEcsWorldDictionary.Add(CurrentLevel, ecsWorld);
    }

    public void ChangeLevelBy(int levelIndexChange)
    {
        switch (levelIndexChange)
        {
            case 0:
                return;
            case > 0:
                if (_dungeon.Levels.Count < _currentLevelIndex + levelIndexChange + 1)
                    _dungeon.Levels.Add(new LevelGenerator().Generate(_configs));

                _currentLevelIndex += levelIndexChange;
                CurrentLevel = _dungeon.Levels[_currentLevelIndex];
                var ecsWorld = new ECSWorld();
                ecsWorld.Initialize(this, _camera, _graphicsDevice);
                _levelEcsWorldDictionary.TryAdd(CurrentLevel, ecsWorld);
                break;
            case < 0:
                if (_currentLevelIndex == 0)
                    return;
                _currentLevelIndex += levelIndexChange;
                CurrentLevel = _dungeon.Levels[_currentLevelIndex];

                break;
            default:
                break;
        }
    }

    public void Update(GameTime gameTime)
    {
        World.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        World.Draw(gameTime);
    }
}