using System;

namespace AmazingMazeDesktop;

public class GlobalRng
{
    public static int Seed { get; private set; }
    private static Random _rng;

    public static void Initialize(int seed)
    {
        Seed = seed;
        _rng = new Random(seed);
    }

    public static int Next(int max) => _rng.Next(max);
    public static int Next(int min, int max) => _rng.Next(min, max);
}