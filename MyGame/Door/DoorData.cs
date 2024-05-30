using Microsoft.Xna.Framework;

namespace MyGame;

public class DoorData
{
    public Vector2 Position { get; private set; }
    public Rectangle Bounds { get; private set; }

    public void ChangePosition(Vector2 position)
    {
        Position = position;
    }

    public void ChangeBounds(Rectangle bounds)
    {
        Bounds = bounds;
    }
}