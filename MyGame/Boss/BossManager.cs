using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class BossManager
{
    public List<BossData> Bosses = new();
    public bool CanEnter = true;
    private bool movingRandomly;
    private TimeSpan randomPos = TimeSpan.Zero;
    private int UpdatePerSecond;
    private bool flagDir;
    private bool flag;


    public void Init(GameData gameData)
    {
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1)
        {
            Bosses.Add(new BossData
            {
                Health = 40,
                MaxHealth = 40,
                Position = new Vector2(gameData.Width - 400, gameData.Height - 400),
                Texture = Globals.Content.Load<Texture2D>("BossScuf"),
                Damage = RoomManager.IndexLevels
            });
            Bosses[^1].Bounds = new Rectangle((int)Bosses[^1].Position.X, (int)Bosses[^1].Position.Y, 300, 300);
            Bosses[^1].Speed = 3f;

            CanEnter = false;
        }
    }

    public void Update(GameData gameData)
    {
        if (Bosses.Count == 0) CanEnter = true;
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && Bosses.Count > 0)
        {
            var hero = gameData.HeroUpdate;
            var random = new Random();

            if (Bosses[^1].Health <= 0) Bosses.RemoveAt(0);
            if (Bosses.Count > 0)
            {
                var boss = Bosses[^1];
                if (boss.Health > 0 && gameData.HeroUpdate.Healths > 0)
                {
                    var directionToHero = Vector2.Normalize(hero.Position - boss.Position);
                    var distanceToHero = Vector2.Distance(boss.Position, hero.Position);


                    // Проверяем, не выйдет ли босс за пределы окна при движении
                    if (!Globals.InBounds(boss.Position + Vector2.Normalize(boss.Direction) * boss.Speed, gameData))
                    {
                        boss.Direction = -boss.Direction; // Меняем направление на противоположное
                    }

                    if (distanceToHero <= 300)
                    {
                        randomPos += Globals.Time;
                        if (UpdatePerSecond < 1)
                        {
                            boss.Direction = new Vector2((float)random.NextDouble() - 0.5f,
                                (float)random.NextDouble() - 0.5f);
                            UpdatePerSecond++;
                            flagDir = true;
                        }

                        if (!Globals.InBounds(boss.Position + Vector2.Normalize(boss.Direction) * boss.Speed, gameData))
                        {
                            boss.Direction = -boss.Direction; // Меняем направление на противоположное
                        }

                        boss.Position += Vector2.Normalize(boss.Direction) * boss.Speed;
                        boss.Bounds = new Rectangle((int)boss.Position.X, (int)boss.Position.Y, 300, 300);
                    }
                    else
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
                            if (!Globals.InBounds(boss.Position + Vector2.Normalize(boss.Direction) * boss.Speed,
                                    gameData))
                            {
                                boss.Direction = -boss.Direction; // Меняем направление на противоположное
                            }

                            boss.Position += Vector2.Normalize(boss.Direction) * boss.Speed;
                            boss.Bounds = new Rectangle((int)boss.Position.X, (int)boss.Position.Y, 300, 300);
                        }
                        // Если расстояние до героя больше минимального порога, двигаемся в направлении героя
                        else
                        {
                            if (!Globals.InBounds(boss.Position + Vector2.Normalize(boss.Direction) * boss.Speed,
                                    gameData))
                            {
                                boss.Direction = -boss.Direction; // Меняем направление на противоположное
                            }

                            boss.Position += Vector2.Normalize(boss.Direction) * boss.Speed;
                            boss.Bounds = new Rectangle((int)boss.Position.X, (int)boss.Position.Y, 300, 300);
                        }
                    }
                }
            }
        }
    }

    public void Draw()
    {
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && Bosses.Count > 0 && Bosses[^1].Health > 0)
        {
            var boss = Bosses[^1];
            Globals.SpriteBatch.Draw(boss.Texture,
                new Rectangle((int)boss.Position.X, (int)boss.Position.Y, 250, 250),
                Color.White);
        }
    }
}