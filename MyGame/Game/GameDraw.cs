using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace MyGame;

public class GameDraw
{
    private SpriteFont font = Globals.Content.Load<SpriteFont>("SpriteText");

    public void Draw(GameData gameData)
    {
        gameData.BackGround.Draw(gameData);
        foreach (var manager in gameData.Managers)
            manager.Draw(gameData);
        
        gameData.DrawHero.Draw(gameData);
        gameData.RecurringItems.Draw(gameData);
        if (gameData.EnemyManager.Enemies.Count <= 0 && gameData.BossManager.CanEnter)
            gameData.DropHeartDraw.Draw(gameData);
        Globals.SpriteBatch.DrawString(font, gameData.BombManager.Bombs.Count.ToString(), new Vector2(150, 200),
            Color.White);
        Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("bomb"), new Rectangle(100, 200, 40, 40), Color.White);
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && gameData.BossData.Health > 0)
        {
            Globals.SpriteBatch.DrawString(font, gameData.BossData.Health.ToString(),
                new Vector2(gameData.BossData.Position.X + 130,
                    gameData.BossData.Position.Y - 50), Color.DarkRed);
        }

        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 &&
            gameData.BossData.Health <= 0 && !gameData.RecurringItems.Taken)
        {
            Globals.SpriteBatch.DrawString(font, gameData.RecurringItems.Item.Characteristics.Description,
                new Vector2(gameData.RecurringItems.Item.Position.X + 100,
                    gameData.RecurringItems.Item.Position.Y - 400), Color.Black);
        }
    }
}