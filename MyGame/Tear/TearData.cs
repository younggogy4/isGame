using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class TearData
{
    public Vector2 Position { get; private set; }
    public float Rotation { get; private set; }
    public float Range { get; private set; }
    public int Speed { get; private set; }


    public void ChangeRotation(float rotation)
    {
        Rotation = rotation;
    }

    public void ChangeRange(float range)
    {
        Range = range;
    }

    public void ChangePosition(Vector2 position)
    {
        Position = position;
    }

    public void ChangeSpeed(int speed)
    {
        Speed = speed;
    }
}