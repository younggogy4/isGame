using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class TearBoss: IGame
{
    public Vector2 Direction { get; set; }
    public float Range { get; set; }
    public TearBoss(Texture2D texture, TearBossData data) : base(texture, data.Position)
    {
        Speed = data.Speed;
        Rotation = data.Rotation;
        Direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
        Range = data.Range+2.4f;
    }
}