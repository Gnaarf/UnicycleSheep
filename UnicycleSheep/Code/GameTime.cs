using System;
using System.Diagnostics;

public class GameTime
{
    private Stopwatch watch;
    /// <summary>
    /// Time passed since GameStart
    /// </summary>
    public TimeSpan TotalTime { get; private set; }
    /// <summary>
    /// Time passed since last Update
    /// </summary>
    public TimeSpan EllapsedTime { get; private set; }
    
    public GameTime()
    {
        watch = new Stopwatch();
        TotalTime = TimeSpan.FromSeconds(0);
        EllapsedTime = TimeSpan.FromSeconds(0);
    }
    public void Start()
    {
        watch.Start();
    }
    public void Stop()
    {
        watch.Reset();
        TotalTime = TimeSpan.FromSeconds(0);
        EllapsedTime = TimeSpan.FromSeconds(0);
    }
    public void Update()
    {
        EllapsedTime = watch.Elapsed - TotalTime;
        TotalTime = watch.Elapsed;
    }
}

