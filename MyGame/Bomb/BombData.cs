using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class BombData
{
    public Vector2 Position;
    public Rectangle BlastRadius;
    public bool CanExplosion;
    public int Count;
}