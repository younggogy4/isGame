using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    public class HeartManager : IManager
    {
        private Texture2D fullHeart;
        private Texture2D halfHeart;
        private Texture2D emptyHeart;
        public List<Heart> Hearts;

        public void Init(GameData gameData, HeroUpdate hero)
        {
            fullHeart = Globals.Content.Load<Texture2D>("heart");
            halfHeart = Globals.Content.Load<Texture2D>("halfHeart");
            emptyHeart = Globals.Content.Load<Texture2D>("emptyHeart");
            Hearts = new List<Heart>();
        }

        public void FillHearts(GameData gameData)
        {
            Hearts.Clear();
            AddFullHearts(gameData.HeroUpdate.Healths / 3);
            AddPartialHeart(gameData.HeroUpdate.Healths % 3);
        }

        private void AddFullHearts(int count)
        {
            for (var i = 0; i < count; i++)
            {
                AddHeart(new Vector2(200 + 75 * i, 80), 0);
            }
        }

        private void AddPartialHeart(int remainder)
        {
            if (remainder > 0)
            {
                var position = Hearts.Count > 0 ? new Vector2(Hearts[^1].Position.X + 75, 80) : new Vector2(200, 80);
                AddHeart(position, 3 - remainder);
            }
        }

        private void AddHeart(Vector2 position, int missingParts)
        {
            var heart = new Heart(fullHeart, halfHeart, emptyHeart, position);
            heart.CountHeart -= missingParts;
            Hearts.Add(heart);
        }

        public void Draw(GameData gameData)
        {
            foreach (var heart in Hearts)
            {
                heart.Draw();
            }
        }

        public void Update(GameData gameData)
        {
        }
    }
}