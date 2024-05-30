using Microsoft.Xna.Framework;

namespace MyGame;

public class BombData
{
    public Vector2 Position { get; private set; }
    public Rectangle BlastRadius { get; private set; }
    public int Count { get; set; }

    public void ChangePos(Vector2 newPos)
    {
        Position = newPos;
    }

    public void SetBlastRadius(Rectangle blastRadius)
    {
        BlastRadius = blastRadius;
    }
}