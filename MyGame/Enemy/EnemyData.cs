using Microsoft.Xna.Framework;

namespace MyGame;

public class EnemyData
{
    public Vector2 Position { get; private set; }
    public int Speed { get; private set; }
    public int Damage { get; private set; }

    public void ChangePosition(Vector2 position)
    {
        Position = position;
    }

    public void ChangeSpeed(int speed)
    {
        Speed = speed;
    }

    public void ChangeDamage(int damage)
    {
        Damage = damage;
    }
}