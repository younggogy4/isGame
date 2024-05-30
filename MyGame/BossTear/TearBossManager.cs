using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class TearBossManager : IManager
{
    private Texture2D texture;
    public List<TearBoss> Tears { get; } = new();

    public void Init(GameData gameData, HeroUpdate hero)
    {
        texture = Globals.Content.Load<Texture2D>("tear");
    }

    public void Add(TearBossData data)
    {
        Tears.Add(new TearBoss(texture, data));
    }

    public void Shoot(GameData gameData, Vector2 spawnPosition)
    {
        gameData.TearBossData.ChangeRange(gameData.RangeTear);
        gameData.TearBossData.ChangeSpeed(gameData.SpeedTear);
        gameData.TearBossData.ChangePosition(spawnPosition);
        gameData.TearBossData.ChangeBounds(new Rectangle((int)gameData.TearBossData.Position.X,
            (int)gameData.TearBossData.Position.Y, texture.Width + 40, texture.Height + 40));
        Add(gameData.TearBossData);
    }

    public void Update(GameData gameData)
    {
        foreach (var t in Tears)
        {
            t.Position += t.Direction * t.Speed * (float)Globals.Time.TotalSeconds;
            t.ChangeBounds(new Rectangle((int)t.Position.X,
                (int)t.Position.Y, texture.Width + 40, texture.Height + 40));
            t.ChangeRange((float)Globals.Time.TotalSeconds);
        }

        Tears.RemoveAll(t => t.Range <= 0);
    }

    public void Draw(GameData gameData)
    {
        foreach (var t in Tears)
        {
            Globals.SpriteBatch.Draw(texture,
                new Rectangle((int)t.Position.X, (int)t.Position.Y, texture.Width + 40, texture.Height + 40),
                Color.Yellow);
        }
    }
}