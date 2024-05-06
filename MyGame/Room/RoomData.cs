using System.Collections.Generic;
using System.Linq;

namespace MyGame;

public class RoomData
{
    public int countRooms;
    public List<int> Levels = new();
    public GameData GameData;
    public GameUpdate GameUpdate;
    public GameDraw GameDraw;

    public RoomData()
    {
        Levels = new List<int>();
    }

    public void Init()
    {
        if (Levels[0] == 1) countRooms = 2;
        if (Levels[0] == 2) countRooms = 4;
        if (Levels[0] == 3) countRooms = 6;
    }
}