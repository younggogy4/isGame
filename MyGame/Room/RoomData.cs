using System.Collections.Generic;
using System.Linq;

namespace MyGame;

public class RoomData
{
    public int CountRooms { get; set; }
    public List<int> Levels = new();
    public GameData GameData { get; set; }
    public GameUpdate GameUpdate { get; set; }
    public GameDraw GameDraw { get; set; }

    public RoomData()
    {
        Levels = new List<int>();
    }

    public void Init()
    {
        if (Levels[0] == 1) CountRooms = 2;
        if (Levels[0] == 2) CountRooms = 4;
        if (Levels[0] == 3) CountRooms = 6;
    }
}