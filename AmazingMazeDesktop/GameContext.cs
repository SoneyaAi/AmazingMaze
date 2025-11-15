using AmazingMazeDesktop.WorldGeneration;
using AmazingMazeDesktop.WorldGeneration.Configs;
using AmazingMazeDesktop.WorldModel;

namespace AmazingMazeDesktop;

public class GameContext
{
    public Dungeon Dungeon => _dungeon;
    public Level CurrentLevel { get; private set; }
    
    private int _currentLevelIndex = 0;
    private Dungeon _dungeon;
    private ConfigsPackage _configs;

    public GameContext(ConfigsPackage configs)
    {
        _configs = configs;
        _dungeon = new DungeonGenerator().Generate(_configs);
        CurrentLevel = _dungeon.Levels[_currentLevelIndex];
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
                break;
            case < 0:
                if (_currentLevelIndex == 0)
                    return;
                break;
            default:
                break;
        }
    }
}