using Microsoft.Xna.Framework;

namespace MyGame;

public class TearBossData
{
    public Vector2 Position { get; private set; }
    public float Rotation { get; private set; }
    public float Range { get; private set; }
    public int Speed { get; private set; }
    public Rectangle Bounds { get; private set; }
    public int Damage { get; private set; }

    public void ChangeDamage(int damage)
    {
        Damage = damage;
    }

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

    public void ChangeBounds(Rectangle bounds)
    {
        Bounds = bounds;
    }
}