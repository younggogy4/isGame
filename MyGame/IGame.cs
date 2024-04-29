using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class IGame
{
    public Texture2D Texture { get; }
    public Vector2 Origin { get; }
    public Vector2 Position { get; set; }
    public int Speed;
    public float Rotation;

    protected IGame(Texture2D texture, Vector2 position)
    {
        this.Texture = texture;
        Position = position;
        Speed = 300;
        Origin = new(texture.Width / 2, texture.Height / 2);
    }
}