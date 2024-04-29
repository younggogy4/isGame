using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class GameDraw
{
    public void Draw(GameData gameData)
    {
        gameData.HeartManager.Draw();
        gameData.TearsManager.Draw();
        gameData.DrawHero.Draw(gameData);
        gameData.BackGround.Draw(gameData);
        gameData.JugManager.Draw();
        gameData.EnemyManager.Draw();
        if (gameData.EnemyManager.Enemies.Count <= 0) gameData.DropHeartDraw.Draw(gameData);
        gameData.BombManager.Draw(gameData);
        gameData.DoorManager.Draw(gameData);
    }
}