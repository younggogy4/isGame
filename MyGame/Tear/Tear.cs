using System;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = System.Numerics.Vector2;

namespace MyGame;

public class Tear : IGame
{
    public Vector2 Direction { get; }
    public float Range { get; private set; }

    public Tear(Texture2D texture, TearData data) : base(texture, data.Position)
    {
        Speed = data.Speed;
        Rotation = data.Rotation;
        Direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
        Range = data.Range+0.4f;
    }

    public void ChangeRange(float range)
    {
        Range -= range;
    }
}