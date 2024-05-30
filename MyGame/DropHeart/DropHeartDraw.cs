using Microsoft.Xna.Framework;
using System.Timers;

namespace MyGame
{
    public class DropHeartDraw
    {
        private Timer timer;
        private bool canDrawHeart;

        public DropHeartDraw()
        {
            timer = new Timer(600); // Таймер с интервалом 600 миллисекунд (0.6 секунд)
            timer.Elapsed += OnTimerElapsed;
            timer.AutoReset = false; // Таймер будет сброшен вручную
        }

        public void Draw(GameData gameData)
        {
            if (ShouldCheckChance(gameData))
            {
                var dropHeartUpdate = gameData.DropHeartUpdate;

                if (dropHeartUpdate.CountCheckChance < 1)
                {
                    dropHeartUpdate.CountCheckChance++;
                    dropHeartUpdate.CheckChance();
                }

                if (!dropHeartUpdate.ChanceDropHeart) return;

                HandleTimer();

                if (canDrawHeart)
                {
                    dropHeartUpdate.CanTakeHeart = true;
                    DrawHeart(dropHeartUpdate);
                }
            }
        }

        private bool ShouldCheckChance(GameData gameData)
        {
            return gameData.EnemyManager.Enemies.Count <= 0 && gameData.BossManager.CanEnter;
        }

        private void HandleTimer()
        {
            if (!timer.Enabled && !canDrawHeart)
            {
                timer.Start();
            }
        }

        private void DrawHeart(DropHeartUpdate dropHeartUpdate)
        {
            if (dropHeartUpdate.DropHeart)
            {
                Globals.SpriteBatch.Draw(
                    dropHeartUpdate.TextureHeart,
                    new Rectangle((int)dropHeartUpdate.Position.X, (int)dropHeartUpdate.Position.Y, dropHeartUpdate.TextureWidth, dropHeartUpdate.TextureHeight),
                    new Rectangle(0, 0, dropHeartUpdate.TextureHeart.Width, dropHeartUpdate.TextureHeart.Height),
                    Color.White
                );
            }

            if (dropHeartUpdate.DropHalfHeart)
            {
                Globals.SpriteBatch.Draw(
                    dropHeartUpdate.TextureHalfHeart,
                    new Rectangle((int)dropHeartUpdate.Position.X, (int)dropHeartUpdate.Position.Y, dropHeartUpdate.TextureWidth, dropHeartUpdate.TextureHeight),
                    new Rectangle(0, 0, dropHeartUpdate.TextureHalfHeart.Width, dropHeartUpdate.TextureHalfHeart.Height),
                    Color.White
                );
            }
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            canDrawHeart = true;
        }
    }
}