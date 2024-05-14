using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class DoorManager
{
    public List<DoorData> Doors = new();
    private Texture2D texture;
    private List<Vector2> positionsDoor = new();
    public bool CanEnter;

    public void Init(GameData gameData, RoomManager roomManager)
    {
        texture = Globals.Content.Load<Texture2D>("blockDoor");
        positionsDoor.Add(new Vector2(0, gameData.Height / 2 - texture.Height));
        positionsDoor.Add(new Vector2(gameData.Width - 150, gameData.Height / 2 - texture.Height));
        //positionsDoor.Add(new Vector2(gameData.Width / 2 - texture.Width, gameData.Height - 150));
        //positionsDoor.Add(new Vector2(gameData.Width / 2 - texture.Width, 0));
        Fill(gameData, roomManager);
    }

    public void Fill(GameData gameData, RoomManager roomManager)
    {
        var door = new DoorData();
        door.Position = positionsDoor[0];
        door.Bounds = door.Bounds = new Rectangle((int)door.Position.X, (int)door.Position.Y, 100, 100);
        Doors.Add(door);
        var door1 = new DoorData();
        door1.Position = positionsDoor[1];
        door1.Bounds = door1.Bounds = new Rectangle((int)door1.Position.X, (int)door1.Position.Y, 100, 100); 
        Doors.Add(door1);
        /*for (var i = 0; i < gameData.DoorData.countDoor; i++)
        {
            var random = new Random().Next(0, 4);
            while (Doors.Any(x => x.Position == positionsDoor[random])) random = new Random().Next(0, 3);
            var door = new DoorData();
            door.Position = positionsDoor[random];
            door.Bounds = new Rectangle((int)door.Position.X, (int)door.Position.Y, 700, 700);
            Doors.Add(door);
        }*/
    }

    public void Update(GameData gameData)
    {
        if (gameData.EnemyManager.Enemies.Count <= 0 && gameData.BossManager.CanEnter)
        {
            texture = Globals.Content.Load<Texture2D>("door");
            CanEnter = true;
        }
    }

    public void Draw(GameData gameData)
    {
        if (RoomManager.IndexRooms == 0)
        {
            Globals.SpriteBatch.Draw(texture,
                new Rectangle((int)Doors[1].Position.X, (int)Doors[1].Position.Y, gameData.Width / 14,
                    gameData.Height / 8),
                Color.White);
        }
        
        else if (RoomManager.IndexRooms == RoomManager.CountRooms - 1)
        {
            Globals.SpriteBatch.Draw(texture,
                new Rectangle((int)Doors[0].Position.X, (int)Doors[0].Position.Y, gameData.Width / 14,
                    gameData.Height / 8),
                Color.White);
        }

        else
        {
            foreach (var door in Doors)
            {
                Globals.SpriteBatch.Draw(texture,
                    new Rectangle((int)door.Position.X, (int)door.Position.Y, gameData.Width / 14,
                        gameData.Height / 8),
                    Color.White);
            }
        }

        
    }
}