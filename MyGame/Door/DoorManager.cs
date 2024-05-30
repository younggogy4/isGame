using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class DoorManager : IManager
{
    public List<DoorData> Doors = new();
    private Texture2D texture;
    private List<Vector2> positionsDoor = new();
    public bool CanEnter { get; private set; }

    public void Init(GameData gameData, HeroUpdate hero)
    {
        LoadTexture();
        InitializeDoorPositions(gameData);
        FillDoors();
    }

    private void LoadTexture()
    {
        texture = Globals.Content.Load<Texture2D>("blockDoor");
    }

    private void InitializeDoorPositions(GameData gameData)
    {
        positionsDoor.Add(new Vector2(0, gameData.Height / 2 - texture.Height));
        positionsDoor.Add(new Vector2(gameData.Width - 150, gameData.Height / 2 - texture.Height));
    }

    private void FillDoors()
    {
        AddDoor(positionsDoor[0]);
        AddDoor(positionsDoor[1]);
    }

    private void AddDoor(Vector2 position)
    {
        var door = new DoorData();
        door.ChangePosition(position);
        door.ChangeBounds(new Rectangle((int)door.Position.X, (int)door.Position.Y, 100, 100));
        Doors.Add(door);
    }

    public void Update(GameData gameData)
    {
        if (ShouldUnlockDoors(gameData)) UnlockDoors();
        
    }

    private bool ShouldUnlockDoors(GameData gameData)
    {
        return gameData.EnemyManager.Enemies.Count <= 0 && gameData.BossManager.CanEnter;
    }

    private void UnlockDoors()
    {
        texture = Globals.Content.Load<Texture2D>("door");
        CanEnter = true;
    }

    public void Draw(GameData gameData)
    {
        if (IsFirstRoom()) DrawDoor(Doors[1], gameData);
        
        else if (IsLastRoom()) DrawDoor(Doors[0], gameData);
        
        else DrawAllDoors(gameData);
    }

    private bool IsFirstRoom()
    {
        return RoomManager.IndexRooms == 0;
    }

    private bool IsLastRoom()
    {
        return RoomManager.IndexRooms == RoomManager.CountRooms - 1;
    }

    private void DrawDoor(DoorData door, GameData gameData)
    {
        Globals.SpriteBatch.Draw(texture,
            new Rectangle((int)door.Position.X, (int)door.Position.Y, gameData.Width / 14, gameData.Height / 8),
            Color.White);
    }

    private void DrawAllDoors(GameData gameData)
    {
        foreach (var door in Doors)
        {
            DrawDoor(door, gameData);
        }
    }
}
