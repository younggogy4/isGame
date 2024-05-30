using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class LukeData
{
    public Vector2 Position { get; private set; }
    public Rectangle Bounds { get; private set; }
    public Texture2D Texture { get; set; }
    public bool CanEnter { get; set; }

    public void ChangeBounds(Rectangle bounds)
    {
        Bounds = bounds;
    }

    public void ChangePosition(Vector2 position)
    {
        Position = position;
    }
}