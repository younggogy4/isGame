using System;
using System.Collections.Generic;
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
        var removedTears = new List<Tear>();

        if (gameData.HeroUpdate.Healths > 0)
        {
            HandeBounds(gameData, removedTears);
            HandleShooting(gameData);
            HandleBossDamage(gameData, removedTears);
            HandleEnemyDamage(gameData, removedTears);
            HandleJugCollisionsAndBoundaryChecks(gameData, removedTears);
        }

        RemoveTears(gameData, removedTears);
    }

    private void HandeBounds(GameData gameData, List<Tear> removedTears)
    {
        foreach (var tear in gameData.TearsManager.Tears)
        {
            if (tear.Position.X <= 80 || tear.Position.X >= gameData.Width - 100 || tear.Position.Y <= 40 ||
                tear.Position.Y > gameData.Height - 100)
            {
                removedTears.Add(tear);
            }
        }
    }
    

    private void HandleShooting(GameData gameData)
    {
        if (gameData.InputManager.DirectionShoot != Button.None)
        {
            var direction = GetShootDirection(gameData.InputManager.DirectionShoot);
            var distance = direction * gameData.Width;
            gameData.TearData.ChangeRotation((float)Math.Atan2(distance.Y, distance.X));
            gameData.TearsManager.Shoot(gameData);
        }
    }

    private Vector2 GetShootDirection(Button directionShoot)
    {
        return directionShoot switch
        {
            Button.Right => new Vector2(1, 0),
            Button.Left => new Vector2(-1, 0),
            Button.Down => new Vector2(0, 1),
            Button.Up => new Vector2(0, -1),
            _ => Vector2.Zero
        };
    }

    private void HandleBossDamage(GameData gameData, List<Tear> removedTears)
    {
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && gameData.BossData.Health > 0)
        {
            foreach (var tear in gameData.TearsManager.Tears.Where(x => gameData.BossData.Bounds.Contains(x.Position)))
            {
                gameData.BossData.ReduceHealths(gameData.HeroUpdate.Damage);
                removedTears.Add(tear);
            }
        }
    }

    private void HandleEnemyDamage(GameData gameData, List<Tear> removedTears)
    {
        foreach (var tear in gameData.TearsManager.Tears)
        {
            foreach (var enemy in gameData.EnemyManager.Enemies)
            {
                if (enemy.Bounds.Contains(tear.Position))
                {
                    enemy.ReduceHealth(gameData.HeroUpdate.Damage);
                    removedTears.Add(tear);
                }
            }
        }
    }

    private void HandleJugCollisionsAndBoundaryChecks(GameData gameData, List<Tear> removedTears)
    {
        foreach (var tear in gameData.TearsManager.Tears)
        {
            foreach (var jug in gameData.JugManager.Jugs)
            {
                if (jug.Bounds.Contains(tear.Position))
                {
                    removedTears.Add(tear);
                }
            }

            if (IsTearOutOfBounds(tear, gameData))
            {
                removedTears.Add(tear);
            }
        }
    }

    private bool IsTearOutOfBounds(Tear tear, GameData gameData)
    {
        return tear.Position.X <= 10 || tear.Position.X >= gameData.Width - 40 || tear.Position.Y <= 10 ||
               tear.Position.Y >= gameData.Height - 40;
    }

    private void RemoveTears(GameData gameData, List<Tear> removedTears)
    {
        foreach (var tear in removedTears)
        {
            gameData.TearsManager.Tears.Remove(tear);
        }
    }


    private void UpdateBossTear(GameData gameData)
    {
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && gameData.HeroUpdate.Healths > 0 &&
            gameData.BossData.Health > 0)
        {
            if (gameData.BossData.Health > 0)
            {
                lastShotTime += Globals.Time;

                if (lastShotTime >=
                    TimeSpan.FromSeconds(1))
                {
                    lastShotTime = TimeSpan.Zero;
                    var heroMovementDirection = gameData.InputManager.Direction;
                    var offsetDistance = gameData.HeroUpdate.Speed / 3;
                    var offset = heroMovementDirection * offsetDistance;
                    var targetPosition = gameData.HeroUpdate.Position + offset;
                    var direction = targetPosition - gameData.BossData.Position;
                    direction.Normalize();
                    gameData.TearBossData.ChangeRotation((float)Math.Atan2(direction.Y, direction.X));
                    var spawnPosition = gameData.BossData.Position;
                    gameData.TearBossManager.Shoot(gameData, spawnPosition);
                }
            }
        }
    }

    private void UpdateLevel(GameData gameData)
    {
        if (gameData.LukeData.Bounds.Intersects(gameData.HeroUpdate.Bounds) && gameData.LukeData.CanEnter
                                                                            && RoomManager.IndexLevels <= 2)
        {
            RoomManager.IndexLevels++;
            var rooms = gameData.RoomManager.Rooms;
            gameData.RecurringItems.Taken = false;
            var dataHealths = rooms[rooms.Count - 1].GameData.HeroUpdate.Healths;
            var dataBomb = rooms[rooms.Count - 1].GameData.BombManager.Bombs;
            var dataHero = rooms[RoomManager.CountRooms - 1].GameData.HeroUpdate;
            rooms.Clear();
            RoomManager.IndexRooms = 0;
            gameData.RoomManager.Init();
            rooms[0].GameData.HeroUpdate.ChangeHealth(dataHealths);
            rooms[0].GameData.BombManager.Bombs = dataBomb;
            rooms[0].GameData.HeartManager.FillHearts(rooms[0].GameData);
            rooms[0].GameData.HeroUpdate = dataHero;
            rooms[0].GameData.HeroUpdate.Position = new Vector2(gameData.Width / 2, gameData.Height / 2);
            gameData.LukeData.CanEnter = false;
        }

        if (gameData.LukeData.Bounds.Intersects(gameData.HeroUpdate.Bounds) && gameData.LukeData.CanEnter
                                                                            && RoomManager.IndexLevels == 3)
            gameData.EndScreen.EndGame = true;
    }

    private void JugUpdate(GameData gameData)
    {
        var removedJugs = new List<JugsData>();
        if (gameData.BossData.Health > 0)
        {
            foreach (var jug in gameData.JugManager.Jugs.Where(x => x.Bounds.Intersects(gameData.BossData.Bounds)))
            {
                var rnd = new Random().Next(0, 99);
                if (rnd <= 10)
                {
                    gameData.DroppedBombData.ChangePosition(jug.Position);
                    gameData.DroppedBombData.Dropped = true;
                    gameData.DroppedBombData.ChangeBounds(
                        new Rectangle((int)gameData.DroppedBombData.Position.X,
                            (int)gameData.DroppedBombData.Position.Y, 50, 50));
                }

                removedJugs.Add(jug);
            }
        }

        foreach (var jug in removedJugs)
            gameData.JugManager.Jugs.Remove(jug);
    }

    private void UpdateHero(GameData gameData)
    {
        var hero = gameData.HeroUpdate;
        timeDamage += Globals.Time;
        HandleBossTearDamage(gameData, hero);
        HandleBossCollision(gameData, hero);
        HandleEnemyCollisions(gameData, hero);
        HandleBossPuddleDamage(gameData, hero);
        HandleRecurringItems(gameData, hero);
    }

    private void HandleBossTearDamage(GameData gameData, HeroUpdate hero)
    {
        if (timeDamage >= TimeSpan.FromSeconds(1)
            && gameData.TearBossManager.Tears.Any(tear => hero.Bounds.Intersects(tear.Bounds)))
        {
            timeDamage = TimeSpan.Zero;
            hero.ChangeHealth(hero.Healths - gameData.TearBossData.Damage);
            gameData.HeartManager.FillHearts(gameData);
        }
    }

    private void HandleBossCollision(GameData gameData, HeroUpdate hero)
    {
        if (gameData.BossData.Health > 0 && gameData.BossData.Bounds.Intersects(hero.Bounds))
        {
            var boss = gameData.BossData;
            RepelEntity(hero, boss.Position, gameData);

            if (hero.Tacheble)
            {
                hero.ChangeHealth(hero.Healths - boss.Damage);
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
    }

    private void HandleEnemyCollisions(GameData gameData, HeroUpdate hero)
    {
        foreach (var enemy in gameData.EnemyManager.Enemies)
        {
            if (enemy.Bounds.Intersects(hero.Bounds))
            {
                RepelEntity(hero, enemy.Position, gameData);

                if (hero.Tacheble)
                {
                    hero.ChangeHealth(hero.Healths - gameData.EnemyData.Damage);
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
        }
    }

    private void HandleBossPuddleDamage(GameData gameData, HeroUpdate hero)
    {
        if (gameData.BossData.Health > 0 && RoomManager.IndexRooms == RoomManager.CountRooms - 1)
        {
            if (hero.Bounds.Intersects(gameData.PuddleBossData.Bounds) && gameData.PuddleBossData.Spill)
            {
                if (hero.Tacheble)
                {
                    hero.ChangeHealth(hero.Healths - gameData.PuddleBossData.Damage);
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
    }

    private void RecurringItemsUpdate(GameData gameData, HeroUpdate hero)
    {
        if (gameData.BossData.Health < 1 && RoomManager.IndexRooms == RoomManager.CountRooms-1)
        {
            gameData.RecurringItems.Init(gameData, hero);
            gameData.RecurringItems.Update(gameData);
        } 
    }

    private void HandleRecurringItems(GameData gameData, HeroUpdate hero)
    {
        if (hero.Bounds.Intersects(gameData.RecurringItems.Item.Bounds)
            && !gameData.RecurringItems.Taken
            && RoomManager.IndexRooms == RoomManager.CountRooms - 1
            && gameData.BossData.Health <= 0)
        {
            var item = gameData.RecurringItems.Item;
            hero.ChangeHealth(item.Characteristics.Healths);
            hero.ChangeDamage(item.Characteristics.Damage);
            hero.ChangeSpeed(item.Characteristics.Speed);
            gameData.TearData.ChangeSpeed(item.Characteristics.SpeedTear);
            gameData.TearData.ChangeRange(item.Characteristics.RangeTear);
            gameData.RecurringItems.Taken = true;
            gameData.HeartManager.FillHearts(gameData);
        }
    }

    private void RepelEntity(HeroUpdate hero, Vector2 targetPosition, GameData gameData)
    {
        var directionToTarget = Vector2.Normalize(hero.Position - targetPosition);
        var repelVector = directionToTarget * 100f;
        var probablePosition = repelVector * 3f * (float)Globals.Time.TotalSeconds;

        if (Globals.InBounds(hero.Position + probablePosition, gameData))
        {
            hero.Position += probablePosition;
        }
    }

    private void UpdateDroppedHeart(GameData gameData)
    {
        if (gameData.HeroUpdate.Healths <= 0)
        {
            gameData.CanPickUpHeart = false;
            return;
        }

        CheckCanPickUpHeart(gameData);

        if (gameData.HeroUpdate.Bounds.Intersects(gameData.DropHeartUpdate.Bounds) && gameData.CanPickUpHeart &&
            gameData.DropHeartUpdate.CanTakeHeart)
        {
            PickUpHeart(gameData);
        }

        if (gameData.PickUpHeart)
        {
            ApplyHeartEffect(gameData);
            gameData.PickUpHeart = false;
        }

        if (!gameData.HeroUpdate.Bounds.Intersects(gameData.DropHeartUpdate.Bounds) ||
            !gameData.DropHeartUpdate.CanTakeHeart)
        {
            gameData.CanPickUpHeart = false;
            return;
        }

        MoveHeartTowardsHero(gameData);
    }

    private void CheckCanPickUpHeart(GameData gameData)
    {
        var lastHeart = gameData.HeartManager.Hearts[^1];
        if (lastHeart.CountHeart == 2 || lastHeart.CountHeart == 1)
        {
            gameData.CanPickUpHeart = true;
        }
    }

    private void PickUpHeart(GameData gameData)
    {
        gameData.DropHeartUpdate.ChanceDropHeart = false;
        gameData.PickUpHeart = true;
        gameData.CanPickUpHeart = false;
        gameData.DropHeartUpdate.CanTakeHeart = false;
    }

    private void ApplyHeartEffect(GameData gameData)
    {
        var hero = gameData.HeroUpdate;
        var dropHeart = gameData.DropHeartUpdate;

        if (dropHeart.DropHeart && hero.Healths % 3 == 1)
        {
            hero.ChangeHealth(hero.Healths + 2);
        }
        else if (dropHeart.DropHeart && hero.Healths % 3 == 2)
        {
            hero.ChangeHealth(hero.Healths + 1);
        }
        else if (dropHeart.DropHalfHeart)
        {
            hero.ChangeHealth(hero.Healths + 1);
        }

        gameData.HeartManager.FillHearts(gameData);
    }

    private void MoveHeartTowardsHero(GameData gameData)
    {
        var heart = gameData.DropHeartUpdate;
        var hero = gameData.HeroUpdate;

        var directionToHero = Vector2.Normalize(heart.Position - hero.Position);
        var repelVector = directionToHero * 5f;
        var newPosition = heart.Position + repelVector;

        if (Globals.InBounds(newPosition, gameData))
        {
            heart.ChangePosition(newPosition);
        }

        heart.ChangeBounds((Rectangle)new EllipseF(heart.Position, heart.TextureWidth, heart.TextureHeight)
            .BoundingRectangle);
        gameData.CanPickUpHeart = false;
    }

    public void Update(GameData gameData)
    {
        UpdateBossTear(gameData);
        UpdateTear(gameData);
        UpdateHearts(gameData);
        JugUpdate(gameData);
        gameData.HeroUpdate.Update(gameData);
        gameData.DropHeartUpdate.Update(gameData);
        UpdateLevel(gameData);
        UpdateDroppedHeart(gameData);

        foreach (var manager in gameData.Managers)
            manager.Update(gameData);
        
        UpdateHero(gameData);
        RecurringItemsUpdate(gameData, gameData.HeroUpdate);
    }
}