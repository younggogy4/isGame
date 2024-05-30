using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class Heart
{
    public int CountHeart { get; set; } = 3;
    private Texture2D fullHeart { get; }
    private Texture2D halfHeart { get; }
    private Texture2D emptyHeart { get; }
    public Vector2 Position { get; }

    public Heart(Texture2D fullHeart, Texture2D halfHeart, Texture2D emptyHeart, Vector2 position)
    {
        this.fullHeart = fullHeart;
        this.halfHeart = halfHeart;
        Position = position;
        this.emptyHeart = emptyHeart;
    }

    public void Draw()
    {
        if (CountHeart == 3) Globals.SpriteBatch.Draw(fullHeart, Position, Color.White);
        if (CountHeart == 2) Globals.SpriteBatch.Draw(halfHeart, Position, Color.White);
        if (CountHeart == 1) Globals.SpriteBatch.Draw(emptyHeart, Position, Color.White);
    }
}