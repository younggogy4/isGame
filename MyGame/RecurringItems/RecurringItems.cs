using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace MyGame;

public class RecurringItems
{
    public List<RecurringItemsData> Items = new();
    private bool SpawnItemAfterBoss;
    public bool Taken;
    public RecurringItemsData Item = new();
    private int limitUpdate;

    public void Init(GameData gameData, HeroUpdate hero)
    {
        Items.Add(new RecurringItemsData
        {
            Texture = Globals.Content.Load<Texture2D>("firstItem"),
            Characteristics = new Characteristics(hero, gameData)
            {
                Damage = hero.Damage - 2,
                SpeedTear = gameData.TearData.Speed + 20,
                RangeTear = gameData.TearData.Range + 0.5f,
                Speed = hero.Speed - 40,
                Description = "Increases attack \n"
                              +"speed, attack range,\n"
                              +"but decreases\n"
                              + "speed and damage"
            }
        });
        
        Items.Add(new RecurringItemsData
        {
            Texture = Globals.Content.Load<Texture2D>("firstItem"),
            Characteristics = new Characteristics(hero, gameData)
            {
                Healths = hero.Healths - 1,
                Speed = hero.Speed + 25,
                RangeTear = gameData.TearData.Range + 0.5f,
                Description = "Increases speed \n"
                              +"and range, but\n"
                              +"takes away HP"
            }
        });
        Items.Add(new RecurringItemsData
        {
            Texture = Globals.Content.Load<Texture2D>("firstItem"),
            Characteristics = new Characteristics(hero, gameData)
            {
                Healths = hero.Healths - 5,
                Damage = hero.Damage + 2,
                SpeedTear = gameData.TearData.Speed + 10,
                RangeTear = gameData.TearData.Range - 0.3f,
                Speed = hero.Speed + 1,
                Description = "Increases damage,\n"+"" +
                              "attack speed, \n" +
                              "and speed, but\n" +
                              "lowers HP and\n" +
                              "attack range"
            }
        });
        
        Items.Add(new RecurringItemsData
        {
            Texture = Globals.Content.Load<Texture2D>("firstItem"),
            Characteristics = new Characteristics(hero, gameData)
            {
                Healths = hero.Healths + 2,
                Description = "Increases HP"
            }
        });
        
        Items.Add(new RecurringItemsData
        {
            Texture = Globals.Content.Load<Texture2D>("firstItem"),
            Characteristics = new Characteristics(hero, gameData)
            {
                Speed = hero.Speed + 15,
                Damage = hero.Damage + 1,
                Description = "Increases speed \n"+
                              "and damage"
            }
        });
        
        Items.Add(new RecurringItemsData
        {
            Texture = Globals.Content.Load<Texture2D>("firstItem"),
            Characteristics = new Characteristics(hero, gameData)
            {
                RangeTear = gameData.TearData.Range + 0.3f,
                Damage =hero.Damage + 1,
                SpeedTear = gameData.TearData.Speed + 12,
                Description = "Increases attack \n"+
                              "range, damage\n" +
                              "and attack speed"
            }
        });
        
        Items.Add(new RecurringItemsData
        {
            Texture = Globals.Content.Load<Texture2D>("firstItem"),
            Characteristics = new Characteristics(hero, gameData)
            {
                Damage = hero.Damage + 2,
                Healths = hero.Healths + 1,
                Speed = hero.Speed + 4,
                Description = "Increases speed,\n" +
                              "damage and HP"
            }
        });
        
       
    }

    public void Update(GameData gameData)
    {
        if (limitUpdate < 10)
        {
            Item = Items[new Random().Next(0, Items.Count - 1)];
            Item.ChangePosition(new Vector2(1200, 500));
            Item.ChangeBounds(new Rectangle((int)Item.Position.X, (int)Item.Position.Y, 80, 80));
            limitUpdate = 10;
        }

        if (RoomManager.IndexRooms == RoomManager.CountRooms - 1 && gameData.BossData.Health <= 0 && !Taken)
        {
            SpawnItemAfterBoss = true;
        }

        else SpawnItemAfterBoss = false;
    }

    public void Draw(GameData gameData)
    {
        if (SpawnItemAfterBoss && !Taken)
        {
            Globals.SpriteBatch.Draw(Item.Texture, new Rectangle((int)Item.Position.X,
                (int)Item.Position.Y, 80, 80), Color.White);
        }
    }
}