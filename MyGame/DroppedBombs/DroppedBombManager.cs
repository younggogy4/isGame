using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class DroppedBombManager
{
    public void Init(GameData gameData)
    {
        var bomb = gameData.DroppedBombData;
        bomb.Texture = Globals.Content.Load<Texture2D>("bomb");
        bomb.Bounds = new Rectangle((int)bomb.Position.X, (int)bomb.Position.Y, 50, 50);
    }

    public void Update(GameData gameData)
    {
        if (gameData.HeroUpdate.Bounds.Intersects(gameData.DroppedBombData.Bounds) && !gameData.DroppedBombData.Taken)
        {
            gameData.DroppedBombData.Taken = true;
            gameData.BombData.Count++;
            gameData.BombManager.Bombs.Add(new BombData());
        }
    }

    public void Draw(GameData gameData)
    {
        if (gameData.DroppedBombData.Dropped && !gameData.DroppedBombData.Taken)
        {
            var bomb = gameData.DroppedBombData;
            Globals.SpriteBatch.Draw(bomb.Texture, 
                new Rectangle((int)bomb.Position.X, (int)bomb.Position.Y, 50, 50),
                Color.White);
        }

    }
}