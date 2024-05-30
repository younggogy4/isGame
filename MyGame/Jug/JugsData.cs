using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class JugsData
{
    public Texture2D Texture { get; set; }
    public Rectangle Bounds { get; private set; }
    public int CountSpawn { get; set; }
    public Vector2 Position { get; private set; }

    public JugsData()
    {
        CountSpawn = new Random().Next(0, 7);
    }

    public void ChangeBounds(Rectangle bounds)
    {
        Bounds = bounds;
    }

    public void ChangePosition(Vector2 position)
    {
        Position = position;
    }
}