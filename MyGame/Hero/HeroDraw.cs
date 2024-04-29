using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class HeroDraw
{
    public static Texture2D Texture { get;} = Globals.Content.Load<Texture2D>("isaac");
    
    public void Draw(GameData gameData)
    {
        if (gameData.HeroUpdate.Healths>0) Globals.SpriteBatch.Draw(Texture, gameData.HeroUpdate.Position, Color.White);
    }
}