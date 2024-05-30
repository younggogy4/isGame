using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    public class PuddleBossManager : IManager
    {
        private TimeSpan timePuddle = TimeSpan.Zero;
        private TimeSpan timePuddleRemove = TimeSpan.Zero;
        private int limitPuddleUpdate = 20;

        public void Init(GameData gameData, HeroUpdate hero)
        {
            gameData.PuddleBossData.ChangeDamage(RoomManager.IndexLevels);
            gameData.PuddleBossData.Texture = Globals.Content.Load<Texture2D>("puddle");
        }

        public void Draw(GameData gameData)
        {
            if (ShouldDrawPuddle(gameData))
            {
                if (timePuddle > TimeSpan.FromSeconds(2))
                {
                    if (timePuddleRemove > TimeSpan.FromSeconds(7))
                    {
                        timePuddle = TimeSpan.Zero;
                        timePuddleRemove = TimeSpan.Zero;
                    }

                    Globals.SpriteBatch.Draw(gameData.PuddleBossData.Texture,
                        new Rectangle((int)gameData.PuddleBossData.Position.X, (int)gameData.PuddleBossData.Position.Y, 300, 300),
                        Color.White);
                }
            }
        }

        private bool ShouldDrawPuddle(GameData gameData)
        {
            return RoomManager.IndexRooms == RoomManager.CountRooms - 1 &&
                   gameData.BossData.Health > 0 &&
                   gameData.BossData.Health <= gameData.BossData.MaxHealth / 2;
        }

        public void Update(GameData gameData)
        {
            if (CheckGameState(gameData))
            {
                if (limitPuddleUpdate < 1)
                {
                    UpdatePuddlePositionAndBounds(gameData);
                    limitPuddleUpdate = 10;
                }

                if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && gameData.BossData.Health > 0)
                {
                    UpdatePuddleTimersAndSpillState(gameData);
                }
            }
        }

        private bool CheckGameState(GameData gameData)
        {
            return gameData.HeroUpdate.Healths > 0 && gameData.BossData.Health > 0;
        }

        private void UpdatePuddlePositionAndBounds(GameData gameData)
        {
            gameData.PuddleBossData.ChangePosition(gameData.BossData.Position);
            gameData.PuddleBossData.ChangeBounds(new Rectangle(
                (int)gameData.PuddleBossData.Position.X,
                (int)gameData.PuddleBossData.Position.Y,
                7000, 700));
        }

        private void UpdatePuddleTimersAndSpillState(GameData gameData)
        {
            timePuddle += Globals.Time;
            timePuddleRemove += Globals.Time;

            if (gameData.BossData.Health <= gameData.BossData.MaxHealth / 2)
            {
                if (limitPuddleUpdate == 20)
                {
                    UpdatePuddlePositionAndBounds(gameData);
                    limitPuddleUpdate = 10;
                    gameData.PuddleBossData.ChangeBounds(new Rectangle(
                        (int)gameData.PuddleBossData.Position.X,
                        (int)gameData.PuddleBossData.Position.Y,
                        700, 700));
                }

                if (timePuddle > TimeSpan.FromSeconds(2))
                {
                    if (timePuddleRemove > TimeSpan.FromSeconds(7))
                    {
                        timePuddle = TimeSpan.Zero;
                        timePuddleRemove = TimeSpan.Zero;
                        gameData.PuddleBossData.Spill = false;
                    }
                    else
                    {
                        gameData.PuddleBossData.Spill = true;
                    }
                }
                else
                {
                    limitPuddleUpdate = -1;
                    gameData.PuddleBossData.Spill = false;
                }
            }
        }
    }
}