using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MyGame;

public class RoomManager
{
    public Dictionary<int/*index room*/, RoomData/*room*/> Rooms = new();
    public int Index;
    private int currentHeart;
    public int CountRooms;
    private GameData savedGameData = new GameData();

    private void Fill()
    {
        var door = new RoomData();
        door.Levels.Add(1);
        door.Init();
        CountRooms = door.countRooms;
        Rooms[0] = new RoomData();
        Rooms[0].GameData = new GameData();
        Rooms[0].GameData.Init(this);
        Rooms[0].GameDraw = new GameDraw();
        Rooms[0].GameUpdate = new GameUpdate();
    }

    public void Init()
    {
        Fill();
    }

    public void DrawUpdate(GameData gameData)
    {
        if (Index >= 0 && Index < CountRooms - 1 && gameData.DoorManager.CanEnter &&
            gameData.DoorManager.Doors[0].Bounds.Intersects(gameData.HeroUpdate.Bounds))
        {
            Index++;
            if (!Rooms.ContainsKey(Index)) Rooms.Add(Index, new RoomData());
            Rooms[Index].GameData = new GameData();
            Rooms[Index].GameData.Init(this);
            Rooms[Index].GameData.HeroUpdate.Healths = Rooms[Index - 1].GameData.HeroUpdate.Healths;
            Rooms[Index].GameData.BombManager.Bombs = Rooms[Index - 1].GameData.BombManager.Bombs;
            Rooms[Index].GameData.HeartManager.FillHearts(Rooms[Index].GameData);
            Rooms[Index].GameDraw = new GameDraw();
            Rooms[Index].GameUpdate = new GameUpdate();
        }
    }
}