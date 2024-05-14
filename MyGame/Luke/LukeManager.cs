using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = System.Numerics.Vector2;

namespace MyGame;

public class LukeManager
{
    public void Update(GameData gameData)
    {
        if (RoomManager.IndexRooms != RoomManager.CountRooms - 1 || gameData.EnemyManager.Enemies.Count > 0 ||
            RoomManager.IndexLevels > 3 || !gameData.BossManager.CanEnter) return;
        gameData.LukeData.CanEnter = true;
        gameData.LukeData.Bounds = new Rectangle((int)gameData.LukeData.Position.X, 
            (int)gameData.LukeData.Position.Y, 
            gameData.LukeData.Texture.Width, 
            gameData.LukeData.Texture.Height);
    }

    public void Init(GameData gameData)
    {
        gameData.LukeData.Texture = Globals.Content.Load<Texture2D>("luke");
        gameData.LukeData.Position= new Vector2(gameData.Width / 2, gameData.Height / 2);
    }

    public void Draw(GameData gameData)
    {
        if (gameData.LukeData.CanEnter)
        {
            Globals.SpriteBatch.Draw(gameData.LukeData.Texture, gameData.LukeData.Position, Color.White);
        }
    }
}