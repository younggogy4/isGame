using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class DroppedBombData
{
    public Vector2 Position { get; private set; }
    public Texture2D Texture { get; set; }
    public bool Dropped { get; set; }
    public bool Taken { get; set; }
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