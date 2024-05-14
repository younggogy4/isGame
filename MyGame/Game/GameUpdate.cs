using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace MyGame;

public class GameUpdate
{
    private TimeSpan lastShotTime = TimeSpan.Zero;
    private TimeSpan timeDamage = TimeSpan.Zero;
    private int spawnBombOrHealth;

    private void UpdateHearts(GameData gameData)
    {
        if (gameData.CountUpdateHeroHelths >= 1) return;
        gameData.CountUpdateHeroHelths++;
        gameData.HeartManager.FillHearts(gameData);
    }

    private void UpdateTear(GameData gameData)
    {
        if (gameData.HeroUpdate.Healths > 0)
        {
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

            if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && gameData.BossManager.Bosses.Count>0)
            {
                gameData.TearsManager.Tears
                    .Where(tear => gameData.BossManager.Bosses.Any(boss => boss.Bounds.Contains(tear.Position)))
                    .Select(tear =>
                    {
                        gameData.BossManager.Bosses[^1].Health -= gameData.HeroUpdate.Damage;
                        return gameData.TearsManager.Tears.Remove(tear);
                    })
                    .ToList();
            }

            var lastEnemy = gameData.EnemyManager.Enemies.Count > 0? gameData.EnemyManager.Enemies[0] : null;
            gameData.TearsManager.Tears
                .Where(tear => gameData.EnemyManager.Enemies.Any(enemy =>
                {
                    lastEnemy = enemy;
                    return enemy.Bounds.Contains(tear.Position);
                }))
                .Select(tear =>
                {
                    lastEnemy.Helths -= gameData.HeroUpdate.Damage;
                    return gameData.TearsManager.Tears.Remove(tear);
                })
                .ToList();
            gameData.TearsManager.Tears
                .Where(tear => gameData.JugManager.Jugs.Any(jug => jug.Bounds.Contains(tear.Position))
                               || tear.Position.X <= 10 || tear.Position.X >= gameData.Width - 40
                               || tear.Position.Y <= 10 || tear.Position.Y >= gameData.Height - 40)
                .Select(tear =>
                {
                    return gameData.TearsManager.Tears.Remove(tear);
                })
                .ToList();
        }
        
        gameData.TearsManager.Update();
    }


    private void UpdateBossTear(GameData gameData)
    {
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && gameData.HeroUpdate.Healths>0 && gameData.BossManager.Bosses.Count>0)
        {
            if (gameData.BossManager.Bosses[^1].Health > 0)
            {
                lastShotTime += Globals.Time;

                if (lastShotTime >= 
                    TimeSpan.FromSeconds(1)) // Проверка, прошла ли секунда с момента последнего выстрела
                {
                    // Обнуляем время с момента последнего выстрела
                    lastShotTime = TimeSpan.Zero;

                    // Получаем направление движения героя
                    Vector2 heroMovementDirection = InputManager.Direction;

                    // Вычисляем смещение для стрельбы вперед от героя
                    float offsetDistance = gameData.HeroUpdate.Speed / 3; // Например, смещение на 50 пикселей
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
        }
        gameData.TearBossManager.Update();
    }
    
    public void UpdateLevel(GameData gameData)
    {
        if (gameData.LukeData.Bounds.Intersects(gameData.HeroUpdate.Bounds) && gameData.LukeData.CanEnter
                                                                            && RoomManager.IndexLevels <= 2)
        {
            RoomManager.IndexLevels++;
            gameData.RecurringItemsManager.Taken = false;
            var dataHealths = RoomManager.Rooms[RoomManager.Rooms.Count - 1].GameData.HeroUpdate.Healths;
            var dataBomb = RoomManager.Rooms[RoomManager.Rooms.Count - 1].GameData.BombManager.Bombs;
            var dataHero = RoomManager.Rooms[RoomManager.CountRooms - 1].GameData.HeroUpdate;
            RoomManager.Rooms.Clear();
            RoomManager.IndexRooms = 0;
            gameData.RoomManager.Init();
            RoomManager.Rooms[0].GameData.HeroUpdate.Healths = dataHealths;
            RoomManager.Rooms[0].GameData.BombManager.Bombs = dataBomb;
            RoomManager.Rooms[0].GameData.HeartManager.FillHearts(RoomManager.Rooms[0].GameData);
            RoomManager.Rooms[0].GameData.HeroUpdate = dataHero;
            RoomManager.Rooms[0].GameData.HeroUpdate.Position = new Vector2(gameData.Width / 2, gameData.Height / 2);
            gameData.LukeData.CanEnter = false;
        }

        if (gameData.LukeData.Bounds.Intersects(gameData.HeroUpdate.Bounds) && gameData.LukeData.CanEnter
                                                                            && RoomManager.IndexLevels == 3)
        {
            gameData.EndScreen.EndGame = true;
        }
    }

    public void JugUpdate(GameData gameData)
    {
        if (gameData.BossManager.Bosses.Count > 0)
        {
            gameData.JugManager.Jugs
                .Where(x => x.Bounds.Intersects(gameData.BossManager.Bosses[^1].Bounds))
                .Select(x =>
                {
                    var rnd = new Random().Next(0, 99);
                    //1 = бомба 2 = хп
                    if (rnd <= 100)
                    {
                        gameData.DroppedBombData.Position = x.Position;
                        gameData.DroppedBombData.Dropped = true;
                        gameData.DroppedBombData.Bounds = 
                            new Rectangle((int)gameData.DroppedBombData.Position.X, (int)gameData.DroppedBombData.Position.Y, 50, 50);
                    }
                    
                    return gameData.JugManager.Jugs.Remove(x);
                })
                .ToList();
        }
    }
    
    public void UpdateHero(GameData gameData)
    {
        var hero = gameData.HeroUpdate;
        timeDamage += Globals.Time;
        if (timeDamage >= TimeSpan.FromSeconds(1)
            && gameData.TearBossManager.Tears.Any(tear => hero.Bounds.Intersects(tear.Bounds)))
        {
            timeDamage = TimeSpan.Zero;
            hero.Healths -= gameData.TearBossData.Damage;
            gameData.HeartManager.FillHearts(gameData);
        }
        
        if (gameData.BossManager.Bosses.Count > 0 && gameData.BossManager.Bosses[^1].Bounds.Intersects(hero.Bounds))
        {
            var boss = gameData.BossManager.Bosses[^1];
            var directionToBoss = Vector2.Normalize(hero.Position - boss.Position);
            var repelVector = directionToBoss * 100f;
            var probablyPos = repelVector * 3f * (float)Globals.Time.TotalSeconds;
            if (Globals.InBounds(hero.Position + probablyPos, gameData))
                hero.Position += probablyPos;
            if (hero.Tacheble)
            {
                hero.Healths-=gameData.BossManager.Bosses[^1].Damage;
                gameData.HeartManager.FillHearts(gameData);
                hero.Tacheble = false;
                hero.Timer.Reset();
            }

            if (hero.Timer.GetTime > 1)
            {
                hero.Tacheble = true;
                hero.Timer.Reset();
            }
        }
        
        foreach (var enemy in gameData.EnemyManager.Enemies)
        {
            if (!enemy.Bounds.Intersects(hero.Bounds)) continue;
            var directionToEnemy = Vector2.Normalize(hero.Position - enemy.Position);
            var repelVector = directionToEnemy * 100f;
            var probablyPos = repelVector * 3f * (float)Globals.Time.TotalSeconds;
            if (Globals.InBounds(hero.Position + probablyPos, gameData))
                hero.Position += probablyPos;
            if (hero.Tacheble)
            {
                hero.Healths -= gameData.EnemyData.Damage;
                gameData.HeartManager.FillHearts(gameData);
                hero.Tacheble = false;
                hero.Timer.Reset();
            }

            if (!(hero.Timer.GetTime > 1)) continue;
            hero.Tacheble = true;
            hero.Timer.Reset();
        }

        if (gameData.BossManager.Bosses.Count > 0 && RoomManager.IndexRooms == RoomManager.CountRooms - 1)
        {
            if (hero.Bounds.Intersects(gameData.PuddleBossData.Bounds) && gameData.PuddleBossData.Spill)
            {
                if (hero.Tacheble)
                {
                    hero.Healths -= gameData.PuddleBossData.Damage;
                    hero.Tacheble = false;
                    gameData.HeartManager.FillHearts(gameData);
                    hero.Timer.Reset();
                }
                
                if (hero.Timer.GetTime > 1)
                {
                    hero.Tacheble = true;
                    hero.Timer.Reset();
                }
            }
        }

        
        if (hero.Bounds.Intersects(gameData.RecurringItemsManager.Item.Bounds) 
            && !gameData.RecurringItemsManager.Taken && RoomManager.IndexRooms==RoomManager.CountRooms-1 
            && gameData.BossManager.Bosses.Count<=0)
        {
            var item = gameData.RecurringItemsManager.Item;
            hero.Healths = item.Characteristics.Healths;
            hero.Damage = item.Characteristics.Damage;
            hero.Speed = item.Characteristics.Speed;
            gameData.TearData.Speed = item.Characteristics.SpeedTear;
            gameData.TearData.Range = item.Characteristics.RangeTear;
            gameData.RecurringItemsManager.Taken = true;
            gameData.HeartManager.FillHearts(gameData);
        }
    }

    public void UpdateDroppedHeart(GameData gameData)
    {
        if (gameData.HeroUpdate.Healths <= 0)
        {
            gameData.CanPickUpHeart = false;
            return;
        }

        if (gameData.HeartManager.Hearts[^1].CountHeart == 2 || gameData.HeartManager.Hearts[^1].CountHeart == 1)
            gameData.CanPickUpHeart = true; //можно подобрать хп, если последнее сердечко не полное

        if (gameData.HeroUpdate.Bounds.Intersects(gameData.DropHeartUpdate.Bounds) && gameData.CanPickUpHeart && gameData.DropHeartUpdate.canTakeHeart)
        {
            gameData.DropHeartUpdate.ChanceDropHeart = false;
            gameData.PickUpHeart = true;
            gameData.CanPickUpHeart = false;
            gameData.DropHeartUpdate.canTakeHeart = false;
        }

        if (gameData.PickUpHeart) //можно подобрать хп, только если последнее сердечко не полное
        {
            //Здесь проверяется сколько нужно хп добавить персонажу, в зависимости от текущего здоровья.
            if (gameData.DropHeartUpdate.dropHeart && gameData.HeroUpdate.Healths % 3 == 1) gameData.HeroUpdate.Healths += 2;
            //если сердечко пустое, и хиро подбирает фулхп, то получает тоже полное хп.
            if (gameData.DropHeartUpdate.dropHeart && gameData.HeroUpdate.Healths % 3 == 2) gameData.HeroUpdate.Healths++;
            //если сердечко хотя бы на половину заполнено и хиро подбирает фулхп, то получает пол хп.
            if (gameData.DropHeartUpdate.dropHalfHeart) gameData.HeroUpdate.Healths++;
            //в любом случае получает половину хп, если подбирает половину хп.
            gameData.HeartManager.FillHearts(gameData);
            gameData.PickUpHeart = false;
            //убирает сердечко из игры, когда его подбирают
        }


        if (!gameData.HeroUpdate.Bounds.Intersects(gameData.DropHeartUpdate.Bounds) || !gameData.DropHeartUpdate.canTakeHeart)
        {
            gameData.CanPickUpHeart = false;
            //проверка на то, что герой подобрал сердечко.
            return;
        }

        var directionToHero = Vector2.Normalize(gameData.DropHeartUpdate.Position - gameData.HeroUpdate.Position);
        var repelVector = directionToHero * 5f;
        var newPosition = gameData.DropHeartUpdate.Position + repelVector;
        if (Globals.InBounds(newPosition, gameData))
            gameData.DropHeartUpdate.Position = newPosition;

        gameData.DropHeartUpdate.Bounds = (Rectangle)new EllipseF(gameData.DropHeartUpdate.Position, gameData.DropHeartUpdate.TextureWidth, gameData.DropHeartUpdate.TextureHeight).BoundingRectangle;
        gameData.CanPickUpHeart = false;
    }
    
    public void Update(GameData gameData)
    {
        InputManager.Update();
        gameData.EnemyManager.Update(gameData);
        gameData.LukeManager.Update(gameData);
        gameData.DoorManager.Update(gameData);
        UpdateBossTear(gameData);
        gameData.BossManager.Update(gameData);
        UpdateTear(gameData);
        UpdateHearts(gameData);
        gameData.JugManager.Update(gameData);
        JugUpdate(gameData);
        gameData.HeroUpdate.Update(gameData);
        gameData.DropHeartUpdate.Update(gameData);
        gameData.BombManager.Update(gameData);
        UpdateLevel(gameData);
        UpdateHero(gameData);
        UpdateDroppedHeart(gameData);
        gameData.DroppedBombManager.Update(gameData);
        gameData.PuddleBossManager.Update(gameData);
        gameData.RecurringItemsManager.Update(gameData);
    }
}