using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace MyGame;

public class EnemyManager
{
    private Texture2D texture;
    private int limitSpawn;
    public List<Enemy> Enemies { get; } = new();

    public void Init()
    {
        texture = Globals.Content.Load<Texture2D>("enemy");
    }

    public void Add(EnemyData data)
    {
        Enemies.Add(new Enemy(texture, data));
    }

    public void Draw()
    {
        foreach (var enemy in Enemies)
        {
            Globals.SpriteBatch.Draw(enemy.Texture, enemy.Position, Color.White);
        }
    }

    private void SpawnEnemy(GameData gameData)
    {
        var listZonesSpawn = new List<Rectangle>();

        if (RoomManager.IndexRooms == 0)
        {
            listZonesSpawn = new List<Rectangle>
            {
                new(200, 200, 300, 300),
                new(200, gameData.Height - 600, 200, 200),
                new(gameData.Width - 600, 200, 300, 300),
                new(gameData.Width - 600, gameData.Height - 600, 300, 300)
            };
        }

        else
        {
            listZonesSpawn = new List<Rectangle>
            {
                new (gameData.Width/2+100, 200, 300, 300),
                new (gameData.Width/2+100, gameData.Height-600, 300, 300),
                new(gameData.Width - 600, 200, 300, 300),
                new(gameData.Width - 600, gameData.Height - 600, 300, 300)
            };
        }

        if (RoomManager.IndexRooms != RoomManager.CountRooms - 1)
        {
            foreach (var randomPosition in listZonesSpawn.Select(t => new Vector2(gameData.Random.Next(t.Left, t.Right),
                         gameData.Random.Next(t.Top, t.Bottom))))
            {
                //if (gameData.JugManager.Jugs.Any(x => Math.Abs(x.Position.X - randomPosition.X) <= 50 ||
                //Math.Abs(x.Position.Y - randomPosition.Y) <= 50)) continue;
                gameData.EnemyData.Speed = gameData.HeroUpdate.Speed - 100;
                gameData.EnemyData.Position = randomPosition;
                Add(gameData.EnemyData);
            }
        }
    }

    public void Update(GameData gameData)
    {
        if (gameData.LimitSpawnEnemy < 1)
        {
            gameData.LimitSpawnEnemy++;
            SpawnEnemy(gameData);
        }

        for (var i = 0; i < Enemies.Count; i++)
        {
            Enemies.Where(enemy => enemy.Bounds.Intersects(Enemies[i].Bounds)).Select(enemy =>
            {
                var directionFromEnemy = Vector2.Normalize(enemy.Position - Enemies[i].Position);
                var repelVector = -directionFromEnemy * 3f;
                var newPosition = Enemies[i].Position + repelVector;
                if (Globals.InBounds(newPosition, gameData)) Enemies[i].Position = newPosition;
                return enemy;
            }).ToList();

            var directionToHero = Vector2.Normalize(gameData.HeroUpdate.Position - Enemies[i].Position);

            if (gameData.HeroUpdate.Bounds.Intersects(Enemies[i].Bounds) && gameData.HeroUpdate.Healths > 0)
            {
                var enemyRepelDistance = 1f;
                var repelVector = -directionToHero * enemyRepelDistance;
                var newPosition = Enemies[i].Position + repelVector;
                if (Globals.InBounds(newPosition, gameData)) Enemies[i].Position = newPosition;
            }
            else if (gameData.HeroUpdate.Healths > 0)
            {
                Enemies[i].Position += directionToHero * gameData.EnemyData.Speed * (float)Globals.Time.TotalSeconds;
            }

            Enemies[i].Bounds = (Rectangle)new EllipseF(Enemies[i].Position,
                Enemies[i].TextureWidth, Enemies[i].TextureHeight).BoundingRectangle;

            foreach (var jug in gameData.JugManager.Jugs)
            {
                if (Enemies[i].Bounds.Intersects(jug.Bounds))
                {
                    var directionFromJug = Vector2.Normalize(Enemies[i].Position - jug.Position);
                    var repelVector = directionFromJug * 3f;
                    Enemies[i].Position += repelVector;
                }
            }
            
            if (Enemies[i].Helths <= 0)
            {
                Enemies.RemoveAt(i);
                i--;
            }
        }
    }
}


