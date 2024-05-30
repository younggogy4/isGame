using System;
using Microsoft.Xna.Framework;
using MyGame;

public class MyTimer
{
    private TimeSpan elapsedTime;

    public MyTimer()
    {
        Reset();
    }

    public void Reset()
    {
        elapsedTime = TimeSpan.Zero;
    }

    public void Update()
    {
        elapsedTime += Globals.Time;
    }

    public float GetTime => (float)elapsedTime.TotalSeconds;
}