using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class BossManager
{
    public List<BossData> Bosses = new();
    public bool CanEnter = true;

    public void Init(GameData gameData)
    {
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1)
        {
            Bosses.Add(new BossData
            {
                Health = 40,
                Position = new Vector2(gameData.Width-400, gameData.Height-400),
                Bounds = new Rectangle(gameData.Width-400, gameData.Height-400, 300, 300),
                Texture = Globals.Content.Load<Texture2D>("BossScuf")
            });

            CanEnter = false;
        }
    }

    public void Update(GameData gameData)
    {
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1)
        {
            var boss = Bosses[0];
            if (boss.Health < 1) CanEnter = true;
            if (gameData.TearsManager.Tears.Any(tear => boss.Bounds.Contains(tear.Position)))
                boss.Health -= gameData.HeroUpdate.Damage;
        }
    }

    public void Draw()
    {
        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && Bosses[0].Health>0)
        {
            var boss = Bosses[0];
            Globals.SpriteBatch.Draw(boss.Texture, 
                new Rectangle((int)boss.Position.X, (int)boss.Position.Y, 250, 250), 
                Color.White);
        }
    }
}