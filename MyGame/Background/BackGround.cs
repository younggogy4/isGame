using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class BackGround
{
    private Texture2D texture;

    public BackGround()
    {
        texture = Globals.Content.Load<Texture2D>("room");
    }

    public void Draw(GameData gameData)
    {
        Globals.SpriteBatch.Draw(texture,
            new Rectangle(0, 0, gameData.Width, 
                gameData.Height), Color.White);
    }
}