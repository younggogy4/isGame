using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MyGame;

public class GameUpdate
{
    private void UpdateHearts(GameData gameData)
    {
        if (gameData.CountUpdateHeroHelths >= 1) return;
        gameData.CountUpdateHeroHelths++;
        gameData.HeartManager.FillHearts(gameData);
    }
    
    private void UpdateTear(GameData gameData)
    {
        if (gameData.HeroUpdate.Healths <= 0) return;
        if (InputManager.DirectionShoot != Button.None)
        {
            var direction = Vector2.Zero;

            switch (InputManager.DirectionShoot)
            {
                case Button.Right:
                    direction = new Vector2(1, 0);
                    break;
                case Button.Left:
                    direction = new Vector2(-1, 0);
                    break;
                case Button.Down:
                    direction = new Vector2(0, 1);
                    break;
                case Button.Up:
                    direction = new Vector2(0, -1);
                    break;
            }

            var distance = direction * gameData.Width;
            gameData.TearData.Rotation = (float)Math.Atan2(distance.Y, distance.X);
            gameData.TearsManager.Shoot(gameData);
        }

       gameData.TearsManager.Tears
            .Where(tear => gameData.EnemyManager.Enemies.Any(enemy => enemy.Bounds.Contains(tear.Position)))
            .Select(tear => gameData.TearsManager.Tears.Remove(tear))
            .ToList();
       gameData.TearsManager.Update();
    }
    
    public void Update(GameData gameData)
    {
        InputManager.Update();
        gameData.EnemyManager.Update(gameData);
        UpdateTear(gameData);
        UpdateHearts(gameData);
        gameData.HeroUpdate.Update(gameData);
        gameData.DropHeartUpdate.Update(gameData);
        gameData.BombManager.Update(gameData);
        gameData.DoorManager.Update(gameData);
    }
}