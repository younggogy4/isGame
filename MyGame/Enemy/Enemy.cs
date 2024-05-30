using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace MyGame;

public class Enemy : IGame
{
    public Rectangle Bounds { get; private set; }
    public int Healths { get; private set; } = 6;
    public int TextureWidth { get; }
    public int TextureHeight { get; }

    public Enemy(Texture2D texture, EnemyData data) : base(texture, data.Position)
    {
        Position = data.Position;
        Speed = data.Speed;
        TextureWidth = texture.Width / 2;
        TextureHeight = texture.Height / 2;
        Bounds = (Rectangle)new EllipseF
            (new Point2(Position.X, Position.Y), TextureWidth, TextureHeight).BoundingRectangle;
        data.ChangeDamage(RoomManager.IndexLevels);
    }

    public void ReduceHealth(int damage)
    {
        Healths -= damage;
    }

    public void ChangeBounds(Rectangle bounds)
    {
        Bounds = bounds;
    }
}