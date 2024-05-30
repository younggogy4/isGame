using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class PuddleBossData
{
    public Vector2 Position { get; private set; }
    public Rectangle Bounds { get; private set; }
    public Texture2D Texture { get; set; }
    public bool Spill { get; set; }
    public int Damage { get; private set; }

    public void ChangeBounds(Rectangle bounds)
    {
        Bounds = bounds;
    }

    public void ChangePosition(Vector2 position)
    {
        Position = position;
    }

    public void ChangeDamage(int damage)
    {
        Damage = damage;
    }
}