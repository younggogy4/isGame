using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MyGame;

public class RoomManager
{
    public static Dictionary<int/*index room*/, RoomData/*room*/> Rooms = new();
    public static int IndexRooms;
    public static int CountRooms;
    public static int IndexLevels=1;
    
    public void Init()
    {
        var room = new RoomData();
        room.Levels.Add(IndexLevels);
        room.Init();
        CountRooms = room.countRooms;
        Rooms[0] = new RoomData();
        Rooms[0].GameData = new GameData();
        Rooms[0].GameData.Init(this);
        Rooms[0].GameDraw = new GameDraw();
        Rooms[0].GameUpdate = new GameUpdate();
    }

    public void Update(GameData gameData)
    {
        if (IndexRooms >= 0 && IndexRooms < CountRooms - 1 && gameData.DoorManager.CanEnter &&
            gameData.DoorManager.Doors[1].Bounds.Intersects(gameData.HeroUpdate.Bounds))
        {
            IndexRooms++;
            if (!Rooms.ContainsKey(IndexRooms))
            {
                Rooms.Add(IndexRooms, new RoomData());
                Rooms[IndexRooms].GameData = new GameData();
                Rooms[IndexRooms].GameData.Init(this);
                Rooms[IndexRooms].GameData.HeroUpdate = Rooms[IndexRooms - 1].GameData.HeroUpdate;
                Rooms[IndexRooms].GameData.HeroUpdate.Position = new Vector2(100, gameData.Height / 2-80);
                Rooms[IndexRooms].GameData.BombManager.Bombs = Rooms[IndexRooms - 1].GameData.BombManager.Bombs;
                Rooms[IndexRooms].GameData.HeartManager.FillHearts(Rooms[IndexRooms].GameData);
                Rooms[IndexRooms].GameDraw = new GameDraw();
                Rooms[IndexRooms].GameUpdate = new GameUpdate();
            }

            else
            {
                Rooms[IndexRooms].GameData.HeroUpdate = Rooms[IndexRooms - 1].GameData.HeroUpdate;
                Rooms[IndexRooms].GameData.HeroUpdate.Position = new Vector2(100, gameData.Height / 2-80);
                Rooms[IndexRooms].GameData.BombManager.Bombs = Rooms[IndexRooms - 1].GameData.BombManager.Bombs;
                Rooms[IndexRooms].GameData.HeartManager.FillHearts(Rooms[IndexRooms].GameData);
            }
        }

        if (IndexRooms > 0 && IndexRooms < CountRooms && gameData.DoorManager.CanEnter
            && gameData.DoorManager.Doors[0].Bounds.Intersects(gameData.HeroUpdate.Bounds))
        {
            IndexRooms--;
            Rooms[IndexRooms].GameData.HeroUpdate = Rooms[IndexRooms + 1].GameData.HeroUpdate;
            Rooms[IndexRooms].GameData.HeroUpdate.Position = new Vector2(gameData.Width-200, gameData.Height / 2-80);
            Rooms[IndexRooms].GameData.BombManager.Bombs = Rooms[IndexRooms + 1].GameData.BombManager.Bombs;
            Rooms[IndexRooms].GameData.HeartManager.FillHearts(Rooms[IndexRooms].GameData);
        }
    }
}