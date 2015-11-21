using System;
using System.Diagnostics;

public class Rand
{
    //random object with seed init
    //and deterministic sequence
    public Rand(uint _seed)
    {
        num = _seed;
    }

    public uint next(uint max, uint min = 0)
    {
        num ^= num << 13;
        num ^= num >> 17;
        num ^= num << 5;
        return (num % (max + 1 - min)) + min;
    }


    uint num;

    static Random random = new Random();

    public static float Value()
    {
        return (float)random.NextDouble();
    }

    public static float Value(float maxValue)
    {
        return (float)(random.NextDouble() * maxValue);
    }

    public static float Value(float minValue, float maxValue)
    {
        return (float)(minValue + random.NextDouble() * (maxValue - minValue));
    }

    public static int IntValue()
    {
        return random.Next();
    }

    public static int IntValue(int maxValue)
    {
        return random.Next(maxValue);
    }

    public static int IntValue(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }
}

