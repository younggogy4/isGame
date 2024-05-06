using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace MyGame;

public class Enemy : IGame
{
    public Rectangle Bounds { get; set; }
    public int Helths { get; set; } = 1;
    public int TextureWidth { get;}
    public int TextureHeight { get;}
    
    public Enemy(Texture2D texture, EnemyData data) : base(texture, data.Position)
    {
        Position = data.Position;
        Speed = data.Speed;
        TextureWidth = texture.Width / 2;
        TextureHeight = texture.Height / 2;
        Bounds = (Rectangle)new EllipseF
            (new Point2(Position.X, Position.Y), TextureWidth, TextureHeight).BoundingRectangle;
    }

}