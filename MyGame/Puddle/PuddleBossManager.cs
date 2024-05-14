using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class PuddleBossManager
{
    private TimeSpan timePuddle = TimeSpan.Zero;
    private TimeSpan timePuddleRemove = TimeSpan.Zero;
    private int limitPuddleUpdate = 20;
    public void Init(GameData gameData)
    {
        gameData.PuddleBossData.Damage = RoomManager.IndexLevels;
        gameData.PuddleBossData.Texture = Globals.Content.Load<Texture2D>("puddle");
    }

    public void Draw(GameData gameData)
    {
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && gameData.BossManager.Bosses.Count > 0
            && gameData.BossManager.Bosses[^1].Health <= gameData.BossManager.Bosses[^1].MaxHealth/2)
        {
            if (timePuddle > TimeSpan.FromSeconds(2))
            {
                if (timePuddleRemove > TimeSpan.FromSeconds(7))
                {
                    timePuddle=TimeSpan.Zero;
                    timePuddleRemove = TimeSpan.Zero;
                }
                Globals.SpriteBatch.Draw(gameData.PuddleBossData.Texture, new Rectangle((int)gameData.PuddleBossData.Position.X, (int)gameData.PuddleBossData.Position.Y, 300, 300)
                   , Color.White);
            }
        }
    }

    public void Update(GameData gameData)
    {
        if (gameData.HeroUpdate.Healths > 0 && gameData.BossManager.Bosses.Count>0)
        {
            if (limitPuddleUpdate < 1)
            {
                gameData.PuddleBossData.Position = gameData.BossManager.Bosses[^1].Position;
                gameData.PuddleBossData.Bounds = new 
                    Rectangle((int)gameData.PuddleBossData.Position.X, (int)gameData.PuddleBossData.Position.Y, 7000, 700);
                limitPuddleUpdate = 10;
            }
            if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && gameData.BossManager.Bosses.Count > 0)
            {
                timePuddle += Globals.Time;
                timePuddleRemove += Globals.Time;
                if (gameData.BossManager.Bosses[^1].Health <= gameData.BossManager.Bosses[^1].MaxHealth / 2)
                {
                    if (limitPuddleUpdate == 20)
                    {
                        gameData.PuddleBossData.Position = gameData.BossManager.Bosses[^1].Position;
                        limitPuddleUpdate = 10;
                        gameData.PuddleBossData.Bounds = new 
                            Rectangle((int)gameData.PuddleBossData.Position.X, (int)gameData.PuddleBossData.Position.Y, 700, 700);
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
                            gameData.PuddleBossData.Spill = true;
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
}