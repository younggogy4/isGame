using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class BossManager : IManager
{
    public bool CanEnter = true;
    private bool movingRandomly;
    private TimeSpan randomPos = TimeSpan.Zero;
    private int UpdatePerSecond;
    private bool flagDir;
    private bool flag;

    public void Init(GameData gameData, HeroUpdate hero)
    {
        if (IsFinalRoom())
        {
            InitializeBoss(gameData);
            CanEnter = false;
        }
    }

    private bool IsFinalRoom()
    {
        return RoomManager.IndexRooms == RoomManager.CountRooms - 1;
    }

    private void InitializeBoss(GameData gameData)
    {
        gameData.BossData.Position = new Vector2(gameData.Width - 400, gameData.Height - 400);
        gameData.BossData.Texture = Globals.Content.Load<Texture2D>("BossScuf");
        gameData.BossData.SetBounds(new Rectangle(
            (int)gameData.BossData.Position.X,
            (int)gameData.BossData.Position.Y,
            300, 300));
    }

    public void Update(GameData gameData)
    {
        var boss = gameData.BossData;
        if (boss.Health <= 0)
        {
            CanEnter = true;
            return;
        }

        if (IsFinalRoom() && boss.Health > 0)
        {
            UpdateBossMovement(gameData, boss);
        }
    }

    private void UpdateBossMovement(GameData gameData, BossData boss)
    {
        var hero = gameData.HeroUpdate;
        var random = new Random();
        var directionToHero = Vector2.Normalize(hero.Position - boss.Position);
        var distanceToHero = Vector2.Distance(boss.Position, hero.Position);

        if (distanceToHero <= 300)
        {
            MoveRandomly(boss, random, gameData);
        }
        else
        {
            MoveTowardsHero(boss, directionToHero, gameData);
        }
    }

    private void MoveRandomly(BossData boss, Random random, GameData gameData)
    {
        randomPos += Globals.Time;
        if (UpdatePerSecond < 1)
        {
            boss.Direction = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f);
            UpdatePerSecond++;
            flagDir = true;
        }

        UpdateBossPosition(boss, gameData);
    }

    private void MoveTowardsHero(BossData boss, Vector2 directionToHero, GameData gameData)
    {
        randomPos += Globals.Time;
        if (randomPos > TimeSpan.FromSeconds(5))
        {
            randomPos = TimeSpan.Zero;
            flagDir = false;
            UpdatePerSecond = 0;
        }

        if (!flagDir)
        {
            boss.Direction = directionToHero;
        }

        UpdateBossPosition(boss, gameData);
    }

    private void UpdateBossPosition(BossData boss, GameData gameData)
    {
        if (!Globals.InBounds(boss.Position + Vector2.Normalize(boss.Direction) * boss.Speed, gameData))
        {
            boss.Direction = -boss.Direction;
        }

        boss.Position += Vector2.Normalize(boss.Direction) * boss.Speed;
        boss.SetBounds(new Rectangle(
            (int)boss.Position.X,
            (int)boss.Position.Y,
            300, 300));
    }

    public void Draw(GameData gameData)
    {
        if (IsFinalRoom() && gameData.BossData.Health > 0)
        {
            DrawBoss(gameData);
        }
    }

    private void DrawBoss(GameData gameData)
    {
        var boss = gameData.BossData;
        Globals.SpriteBatch.Draw(boss.Texture,
            new Rectangle((int)boss.Position.X, (int)boss.Position.Y, 250, 250),
            Color.White);
    }
}