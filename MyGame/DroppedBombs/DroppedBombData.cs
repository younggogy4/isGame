using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class DroppedBombData
{
    public Vector2 Position;
    public Texture2D Texture;
    public bool Dropped;
    public bool Taken;
    public Rectangle Bounds;
}