
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
        if (gameData.EnemyManager.Enemies.Count <= 0 && gameData.BossManager.CanEnter) gameData.DropHeartDraw.Draw(gameData);
        gameData.BombManager.Draw(gameData);
        gameData.DoorManager.Draw(gameData);
        gameData.LukeManager.Draw(gameData);
        if (gameData.EndScreen.EndGame) gameData.EndScreen.Draw();
        gameData.BossManager.Draw();
        gameData.TearBossManager.Draw();
    }
}