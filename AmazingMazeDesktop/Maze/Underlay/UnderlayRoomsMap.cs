namespace AmazingMazeDesktop.Maze.Underlay;

/// <summary>
/// Placing rooms with theirs IDs on grid
/// </summary>
public class UnderlayRoomsMap(int width, int height)
{
    public int this[int y, int x] => _map[y, x];
    
    private readonly int[,] _map = new int[height, width];

    public void Generate(int roomsCount)
    {
        for (var i = 0; i < roomsCount; i++)
        {
            var roomId = i + 2; // +2 as 0 are empty cells and 1 is for walls.
            PlaceSquare(roomId);
        }
    }

    private void PlaceSquare(int roomId)
    {
        var rows = _map.GetLength(0);
        var columns = _map.GetLength(1);

        var k = GlobalRng.Next(2) == 0 ? 2 : 3;

        while (true)
        {
            var row = GlobalRng.Next(0, rows - k + 1);
            var column = GlobalRng.Next(0, columns - k + 1);

            if (!CanPlace(row, column, k))
                continue;

            Fill(row, column, k, roomId);
            return;
        }
    }

    private bool CanPlace(int row, int column, int k)
    {
        for (var i = 0; i < k; i++)
        for (var j = 0; j < k; j++)
            if (_map[row + i, column + j] != 0)
                return false;
        return true;
    }

    private void Fill(int row, int column, int k, int val)
    {
        for (var i = 0; i < k; i++)
        for (var j = 0; j < k; j++)
            _map[row + i, column + j] = val;
    }
}