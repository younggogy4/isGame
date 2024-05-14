using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace MyGame;

public class RecurringItemsManager
{
    public List<RecurringItemsData> RecurringItems = new(); 
    private bool SpawnItemAfterBoss;
    public bool Taken;
    public RecurringItemsData Item = new ();
    private int limitUpdate;

    public void Init(GameData gameData)
    {
        RecurringItems.Add(new RecurringItemsData { Characteristics = new Characteristics(gameData) });
        RecurringItems[0].Characteristics.Healths = gameData.HeroUpdate.Healths + 2;
        RecurringItems[0].Texture = Globals.Content.Load<Texture2D>("firstItem");
        RecurringItems[0].Characteristics.Description = "Increases HP";
        RecurringItems.Add(new RecurringItemsData(){Characteristics = new Characteristics(gameData)});
        RecurringItems[1].Texture = Globals.Content.Load<Texture2D>("firstItem");
        RecurringItems[1].Characteristics.Speed = gameData.HeroUpdate.Speed + 15;
        RecurringItems[1].Characteristics.Damage = gameData.HeroUpdate.Damage + 1;
        RecurringItems[1].Characteristics.Description = "Increases speed and damage";
        RecurringItems.Add(new RecurringItemsData{Characteristics = new Characteristics(gameData)});
        RecurringItems[2].Texture = Globals.Content.Load<Texture2D>("firstItem");
        RecurringItems[2].Characteristics.RangeTear = gameData.TearData.Range + 0.3f;
        RecurringItems[2].Characteristics.Damage = gameData.HeroUpdate.Damage+1;
        RecurringItems[2].Characteristics.SpeedTear = gameData.TearData.Speed+12;
        RecurringItems[2].Characteristics.Description = "Increases attack range, damage and attack speed";
        RecurringItems.Add(new RecurringItemsData { Characteristics = new Characteristics(gameData) });
        RecurringItems[3].Texture = Globals.Content.Load<Texture2D>("firstItem");
        RecurringItems[3].Characteristics.Damage = gameData.HeroUpdate.Damage+2;
        RecurringItems[3].Characteristics.Healths =  gameData.HeroUpdate.Healths+1;
        RecurringItems[3].Characteristics.Speed = gameData.HeroUpdate.Speed + 4;
        RecurringItems[3].Characteristics.Description = "Increases speed, damage and HP";
        RecurringItems.Add(new RecurringItemsData{Characteristics = new Characteristics(gameData)});
        RecurringItems[4].Texture = Globals.Content.Load<Texture2D>("firstItem");
        RecurringItems[4].Characteristics.Damage = gameData.HeroUpdate.Damage-1;
        RecurringItems[4].Characteristics.Speed = gameData.HeroUpdate.Speed+25;
        RecurringItems[4].Characteristics.RangeTear = gameData.TearData.Range+0.5f;
        RecurringItems[4].Characteristics.Description = "Increases speed and range, but takes away HP";
        RecurringItems.Add(new RecurringItemsData{Characteristics = new Characteristics(gameData)});
        RecurringItems[5].Texture = Globals.Content.Load<Texture2D>("firstItem");
        RecurringItems[5].Characteristics.Damage = gameData.HeroUpdate.Damage-2;
        RecurringItems[5].Characteristics.SpeedTear = gameData.TearData.Speed+20;
        RecurringItems[5].Characteristics.RangeTear = gameData.TearData.Range+0.3f;
        RecurringItems[5].Characteristics.Speed = gameData.HeroUpdate.Speed-33;
        RecurringItems[5].Characteristics.Description = "Increases attack speed, attack range, but decreases speed and damage";
        RecurringItems.Add(new RecurringItemsData{Characteristics = new Characteristics(gameData)});
        RecurringItems[6].Texture = Globals.Content.Load<Texture2D>("firstItem");
        RecurringItems[6].Characteristics.Healths = gameData.HeroUpdate.Healths-5;
        RecurringItems[6].Characteristics.Damage = gameData.HeroUpdate.Damage+2;
        RecurringItems[6].Characteristics.SpeedTear = gameData.TearData.Speed+10;
        RecurringItems[6].Characteristics.RangeTear = gameData.TearData.Range-0.3f;
        RecurringItems[6].Characteristics.Speed = gameData.HeroUpdate.Speed+1;
        RecurringItems[6].Characteristics.Description = "Increases damage, attack speed, and speed, but lowers HP and attack range";
    }

    public void Update(GameData gameData)
    {
        if (gameData.BossManager.Bosses.Count>0 && gameData.BossManager.Bosses[0].Health>0)
        {
            Item = RecurringItems[new Random().Next(0, RecurringItems.Count-1)];
            Item.Position = new Vector2(1200, 500);
            Item.Bounds = new Rectangle((int)Item.Position.X, (int)Item.Position.Y, 80, 80);
            limitUpdate = 10;
        }

        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && gameData.BossManager.Bosses.Count == 0 && !Taken)
        {
            SpawnItemAfterBoss = true;
        }

        else SpawnItemAfterBoss = false;
    }

    public void Draw()
    {
        if (SpawnItemAfterBoss && !Taken)
        {
            Globals.SpriteBatch.Draw(Item.Texture, new Rectangle((int)Item.Position.X, 
                (int)Item.Position.Y, 80, 80), Color.White);
        }
    }
}