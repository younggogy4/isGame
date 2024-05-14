using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;

namespace MyGame;

public class TearBossManager
{
    private Texture2D texture;
    public List<TearBoss> Tears { get; } = new();

    public void Init()
    {
        texture = Globals.Content.Load<Texture2D>("tear");
    }

    public void Add(TearBossData data)
    {
        Tears.Add(new TearBoss(texture, data));
    }
    
    public void Shoot(GameData gameData, Vector2 spawnPosition)
    {
        gameData.TearBossData.Range = gameData.RangeTear;
        gameData.TearBossData.Speed = gameData.SpeedTear;
        gameData.TearBossData.Position = spawnPosition;
        gameData.TearBossData.Bounds = new Rectangle((int)gameData.TearBossData.Position.X,
            (int)gameData.TearBossData.Position.Y, texture.Width + 40, texture.Height + 40);
        Add(gameData.TearBossData);
    }

    public void Update()
    {
        foreach (var t in Tears)
        {
            t.Position += t.Direction * t.Speed * (float)Globals.Time.TotalSeconds;
            t.Bounds = new Rectangle((int)t.Position.X,
                (int)t.Position.Y, texture.Width + 40, texture.Height + 40);
            t.Range -= (float)Globals.Time.TotalSeconds;
        }

        Tears.RemoveAll(t => t.Range <= 0);
    }

    public void Draw()
    {
        foreach (var t in Tears)
        {
            Globals.SpriteBatch.Draw(texture, 
                new Rectangle((int)t.Position.X, (int)t.Position.Y, texture.Width+40, texture.Height+40), 
                Color.Yellow);
        }
    }
}