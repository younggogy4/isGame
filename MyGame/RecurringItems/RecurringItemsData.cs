using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class RecurringItemsData
{
    public Vector2 Position { get; private set; }
    public Texture2D Texture { get; set; }
    public Characteristics Characteristics { get; init; }
    public Rectangle Bounds { get; private set; }

    public void ChangeBounds(Rectangle bounds)
    {
        Bounds = bounds;
    }

    public void ChangePosition(Vector2 position)
    {
        Position = position;
    }
}