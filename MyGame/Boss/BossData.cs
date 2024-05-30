using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class BossData
{
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public Rectangle Bounds { get; private set; }
    public int Health { get; private set; } = 40 + (RoomManager.IndexLevels - 1) * 5;
    public Vector2 Direction { get; set; }
    public float Speed { get; private set; } = 3f;
    public int MaxHealth { get; private set; } = 40;
    public int Damage { get; private set; } = RoomManager.IndexLevels;

    public void SetBounds(Rectangle bounds)
    {
        Bounds = bounds;
    }

    public void ReduceHealths(int damage)
    {
        Health -= damage;
    }
}