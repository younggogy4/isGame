
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace MyGame;

public class GameDraw
{
    private SpriteFont font = Globals.Content.Load<SpriteFont>("SpriteText");
    public void Draw(GameData gameData)
    {
        gameData.PuddleBossManager.Draw(gameData);
        gameData.BombManager.Draw(gameData);
        gameData.HeartManager.Draw();
        gameData.TearsManager.Draw();
        gameData.DrawHero.Draw(gameData);
        gameData.BackGround.Draw(gameData);
        gameData.JugManager.Draw();
        gameData.EnemyManager.Draw();
        if (gameData.EnemyManager.Enemies.Count <= 0 && gameData.BossManager.CanEnter) gameData.DropHeartDraw.Draw(gameData);
        gameData.DoorManager.Draw(gameData);
        gameData.LukeManager.Draw(gameData);
        if (gameData.EndScreen.EndGame) gameData.EndScreen.Draw();
        gameData.BossManager.Draw();
        gameData.TearBossManager.Draw();
        gameData.DroppedBombManager.Draw(gameData);
        gameData.RecurringItemsManager.Draw();
        Globals.SpriteBatch.DrawString(font, gameData.BombManager.Bombs.Count.ToString(), new Vector2(150, 200), Color.White);
        Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("bomb"), new Rectangle(100, 200, 40, 40), Color.White);
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && gameData.BossManager.Bosses.Count > 0)
        {
            Globals.SpriteBatch.DrawString(font, gameData.BossManager.Bosses[0].Health.ToString(), 
                new Vector2(gameData.BossManager.Bosses[0].Position.X+130, 
                    gameData.BossManager.Bosses[0].Position.Y-50), Color.DarkRed);
        }

        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && 
            gameData.BossManager.Bosses.Count == 0 && !gameData.RecurringItemsManager.Taken)
        {
            Globals.SpriteBatch.DrawString(font, gameData.RecurringItemsManager.Item.Characteristics.Description,
                new Vector2(gameData.RecurringItemsManager.Item.Position.X-400, gameData.RecurringItemsManager.Item.Position.Y-100), Color.White);
        }
        
    }
}