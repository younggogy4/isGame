using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace MyGame
{
    public class EnemyManager : IManager
    {
        private Texture2D texture;
        public List<Enemy> Enemies { get; } = new();

        public void Init(GameData gameData, HeroUpdate hero)
        {
            texture = Globals.Content.Load<Texture2D>("enemy");
        }

        private void Add(EnemyData data)
        {
            Enemies.Add(new Enemy(texture, data));
        }

        public void Draw(GameData gameData)
        {
            foreach (var enemy in Enemies)
            {
                Globals.SpriteBatch.Draw(enemy.Texture, enemy.Position, Color.White);
            }
        }

        public void Update(GameData gameData)
        {
            if (gameData.LimitSpawnEnemy < 1)
            {
                gameData.LimitSpawnEnemy++;
                SpawnEnemy(gameData);
            }

            UpdateEnemies(gameData);
        }

        private void SpawnEnemy(GameData gameData)
        {
            var listZonesSpawn = GetSpawnZones(gameData);

            if (RoomManager.IndexRooms != RoomManager.CountRooms - 1)
            {
                foreach (var randomPosition in listZonesSpawn.Select(t =>
                             new Vector2(gameData.Random.Next(t.Left, t.Right),
                                 gameData.Random.Next(t.Top, t.Bottom))))
                {
                    gameData.EnemyData.ChangeSpeed(gameData.HeroUpdate.Speed - 100);
                    gameData.EnemyData.ChangePosition(randomPosition);
                    Add(gameData.EnemyData);
                }
            }
        }

        private List<Rectangle> GetSpawnZones(GameData gameData)
        {
            if (RoomManager.IndexRooms == 0)
            {
                return new List<Rectangle>
                {
                    new(200, 200, 300, 300),
                    new(200, gameData.Height - 600, 200, 200),
                    new(gameData.Width - 600, 200, 300, 300),
                    new(gameData.Width - 600, gameData.Height - 600, 300, 300)
                };
            }
            else
            {
                return new List<Rectangle>
                {
                    new(gameData.Width / 2 + 100, 200, 300, 300),
                    new(gameData.Width / 2 + 100, gameData.Height - 600, 300, 300),
                    new(gameData.Width - 600, 200, 300, 300),
                    new(gameData.Width - 600, gameData.Height - 600, 300, 300)
                };
            }
        }

        private void UpdateEnemies(GameData gameData)
        {
            for (var i = 0; i < Enemies.Count; i++)
            {
                HandleEnemyCollisions(gameData, i);
                MoveEnemyTowardsHero(gameData, i);
                UpdateEnemyBounds(i);
                HandleJugCollisions(gameData, i);
                RemoveDeadEnemies(i);
            }
        }

        private void HandleEnemyCollisions(GameData gameData, int i)
        {
            foreach (var enemy in Enemies.Where(x => x.Bounds.Intersects(Enemies[i].Bounds)))
            {
                var directionFromEnemy = Vector2.Normalize(enemy.Position - Enemies[i].Position);
                var repelVector = -directionFromEnemy * 3f;
                var newPosition = Enemies[i].Position + repelVector;
                if (Globals.InBounds(newPosition, gameData)) Enemies[i].Position = newPosition;
            }
        }

        private void MoveEnemyTowardsHero(GameData gameData, int i)
        {
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
        }

        private void UpdateEnemyBounds(int i)
        {
            Enemies[i].ChangeBounds((Rectangle)new EllipseF(Enemies[i].Position,
                Enemies[i].TextureWidth, Enemies[i].TextureHeight).BoundingRectangle);
        }

        private void HandleJugCollisions(GameData gameData, int i)
        {
            foreach (var jug in gameData.JugManager.Jugs)
            {
                if (Enemies[i].Bounds.Intersects(jug.Bounds))
                {
                    var directionFromJug = Vector2.Normalize(Enemies[i].Position - jug.Position);
                    var repelVector = directionFromJug * 3f;
                    Enemies[i].Position += repelVector;
                }
            }
        }

        private void RemoveDeadEnemies(int i)
        {
            if (Enemies[i].Healths <= 0)
            {
                Enemies.RemoveAt(i);
                i--;
            }
        }
    }
}