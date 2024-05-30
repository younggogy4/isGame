using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    public class JugManager : IManager
    {
        public List<JugsData> Jugs = new();
        private Random random = new();
        private List<Rectangle> spawnRates = new();
        private Texture2D texture;

        public void Init(GameData gameData, HeroUpdate hero)
        {
            SetupSpawnRates(gameData);
            LoadTextures();
        }

        private void SetupSpawnRates(GameData gameData)
        {
            spawnRates.Add(new Rectangle(200, 200, 300, 300));
            spawnRates.Add(new Rectangle(200, gameData.Height - 600, 300, 300));
            spawnRates.Add(new Rectangle(gameData.Width - 600, 200, 300, 300));
            spawnRates.Add(new Rectangle(gameData.Width - 600, gameData.Height - 600, 300, 300));
        }

        private void LoadTextures()
        {
            texture = Globals.Content.Load<Texture2D>("jug");
        }

        public void Update(GameData gameData)
        {
            while (gameData.JugsData.CountSpawn > 0)
            {
                TrySpawnJug(gameData);
            }
        }

        private void TrySpawnJug(GameData gameData)
        {
            var randomIndex = random.Next(0, spawnRates.Count);
            var randomPos = GenerateRandomPosition(randomIndex);

            if (IsPositionValid(randomPos))
            {
                var newJugsData = new JugsData
                {
                    Texture = texture
                };
                newJugsData.ChangePosition(randomPos);
                Jugs.Add(newJugsData);
                gameData.JugsData.CountSpawn--;
                MakeCollision();
            }
        }

        private Vector2 GenerateRandomPosition(int spawnRateIndex)
        {
            var spawnRate = spawnRates[spawnRateIndex];
            return new Vector2(
                random.Next(spawnRate.Left, spawnRate.Right),
                random.Next(spawnRate.Top, spawnRate.Bottom)
            );
        }

        private bool IsPositionValid(Vector2 position)
        {
            return !Jugs.Any(x =>
                Math.Abs(x.Position.X - position.X) <= 50 ||
                Math.Abs(x.Position.Y - position.Y) <= 50);
        }

        private void MakeCollision()
        {
            foreach (var jug in Jugs)
            {
                jug.ChangeBounds(new Rectangle(
                    (int)jug.Position.X - 20, 
                    (int)jug.Position.Y - 30, 
                    jug.Texture.Width + 15,
                    jug.Texture.Height + 20
                ));
            }
        }

        public void Draw(GameData gameData)
        {
            foreach (var jug in Jugs)
            {
                Globals.SpriteBatch.Draw(jug.Texture, jug.Position, Color.White);
            }
        }
    }
}