using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = System.Numerics.Vector2;

namespace MyGame;

public class Tear: IGame
{
    public Vector2 Direction { get; set; }
    public float Range { get; set; }
    public Tear(Texture2D texture, TearData data) : base(texture, data.Position)
    {
        Speed = data.Speed+100;
        Rotation = data.Rotation;
        Direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
        Range = data.Range+100;
    }
}