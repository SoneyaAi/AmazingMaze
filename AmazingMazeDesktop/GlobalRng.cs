using System;

namespace AmazingMazeDesktop;

public class GlobalRng
{
    public static int Seed { get; private set; }
    public static Random Random;

    public static void Initialize(int seed)
    {
        Seed = seed;
        Random = new Random(seed);
    }

    public static int Next(int max) => Random.Next(max);
    public static int Next(int min, int max) => Random.Next(min, max);
}