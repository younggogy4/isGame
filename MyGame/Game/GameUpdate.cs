using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MyGame;

public class GameUpdate
{
    private TimeSpan lastShotTime = TimeSpan.Zero;
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
            .Where(tear => gameData.EnemyManager.Enemies.Any(enemy => enemy.Bounds.Contains(tear.Position)) 
                           || gameData.JugManager.Jugs.Any(jug => jug.Bounds.Contains(tear.Position))
                           || tear.Position.X <= 10 || tear.Position.X >= gameData.Width-40
                           ||tear.Position.Y <= 10 || tear.Position.Y >= gameData.Height-40 
                           || gameData.BossManager.Bosses.Any(boss => boss.Bounds.Contains(tear.Position)))
            .Select(tear => gameData.TearsManager.Tears.Remove(tear))
            .ToList();
       gameData.TearsManager.Update();
    }


    private void UpdateBossTear(GameData gameData)
    {
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1)
        {
            if (gameData.BossManager.Bosses[0].Health > 0) 
            {
                lastShotTime += Globals.Time;

                if (lastShotTime >= TimeSpan.FromSeconds(1)) // Проверка, прошла ли секунда с момента последнего выстрела
                {
                    // Обнуляем время с момента последнего выстрела
                    lastShotTime = TimeSpan.Zero;

                    // Получаем направление движения героя
                    Vector2 heroMovementDirection = InputManager.Direction;

                    // Вычисляем смещение для стрельбы вперед от героя
                    float offsetDistance = gameData.HeroUpdate.Speed/3; // Например, смещение на 50 пикселей
                    Vector2 offset = heroMovementDirection * offsetDistance;

                    // Корректируем позицию для стрельбы с учетом смещения
                    var targetPosition = gameData.HeroUpdate.Position + offset;

                    // Вычисляем направление на цель
                    var direction = targetPosition - gameData.BossManager.Bosses[0].Position;
                    direction.Normalize();

                    // Устанавливаем угол поворота для выстрела босса
                    gameData.TearBossData.Rotation = (float)Math.Atan2(direction.Y, direction.X);

                    // Вычисляем позицию для стрельбы
                    var spawnPosition = gameData.BossManager.Bosses[0].Position;

                    // Осуществляем выстрел босса
                    gameData.TearBossManager.Shoot(gameData, spawnPosition);
                }
            }
            // Увеличиваем время с момента последнего выстрела
            gameData.TearBossManager.Update();
        }
    }

    public void UpdateLevel(GameData gameData)
    {
        if (gameData.LukeData.Bounds.Intersects(gameData.HeroUpdate.Bounds) && gameData.LukeData.CanEnter 
                                                                            && RoomManager.IndexLevels<=2)
        {
            RoomManager.IndexLevels++;
            var dataHealths = RoomManager.Rooms[RoomManager.Rooms.Count-1].GameData.HeroUpdate.Healths;
            var dataBomb = RoomManager.Rooms[RoomManager.Rooms.Count - 1].GameData.BombManager.Bombs;
            RoomManager.Rooms.Clear();
            RoomManager.IndexRooms = 0;
            gameData.RoomManager.Init();
            RoomManager.Rooms[0].GameData.HeroUpdate.Healths = dataHealths;
            RoomManager.Rooms[0].GameData.BombManager.Bombs = dataBomb;
            RoomManager.Rooms[0].GameData.HeartManager.FillHearts(RoomManager.Rooms[0].GameData);
            gameData.LukeData.CanEnter = false;
        }

        if (gameData.LukeData.Bounds.Intersects(gameData.HeroUpdate.Bounds) && gameData.LukeData.CanEnter
                                                                            && RoomManager.IndexLevels == 3)
        {
            gameData.EndScreen.EndGame = true;
        }
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
        gameData.LukeManager.Update(gameData);
        UpdateLevel(gameData);
        gameData.BossManager.Update(gameData);
        UpdateBossTear(gameData);
    }
}