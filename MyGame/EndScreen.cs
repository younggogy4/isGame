using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using SharpDX.Direct3D9;

namespace MyGame;

public class EndScreen
{
    private Texture2D texture;
    public bool EndGame;

    public void Init()
    {
        texture = Globals.Content.Load<Texture2D>("EndScreen");
    }

    public void Draw()
    {
        Globals.SpriteBatch.Draw(texture, Vector2.Zero, Color.White);
    }
}