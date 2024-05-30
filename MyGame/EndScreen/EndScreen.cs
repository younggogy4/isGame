using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using SharpDX.Direct3D9;

namespace MyGame;

public class EndScreen : IManager
{
    private Texture2D texture;
    public bool EndGame;

    public void Init(GameData gameData, HeroUpdate hero)
    {
        texture = Globals.Content.Load<Texture2D>("EndScreen");
    }

    public void Draw(GameData gameData)
    {
        if (gameData.EndScreen.EndGame)
        {
            Globals.SpriteBatch.Draw(texture, Vector2.Zero, Color.White);

        }
    }

    public void Update(GameData gameData)
    {
    }
}