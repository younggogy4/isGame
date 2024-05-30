using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class TearsManager : IManager
{
    private Texture2D texture;
    public List<Tear> Tears { get; } = new();

    public void Init(GameData gameData, HeroUpdate hero)
    {
        texture = Globals.Content.Load<Texture2D>("tear");
    }

    public void Add(TearData data)
    {
        Tears.Add(new Tear(texture, data));
    }

    public void Shoot(GameData gameData)
    {
        gameData.TearData.ChangeRange(gameData.RangeTear);
        gameData.TearData.ChangeSpeed(gameData.SpeedTear);
        gameData.TearData.ChangePosition(new Vector2(gameData.HeroUpdate.Position.X, gameData.HeroUpdate.Position.Y));
        Add(gameData.TearData);
    }

    public void Update(GameData gameData)
    {
        foreach (var t in Tears)
        {
            t.Position += t.Direction * t.Speed * (float)Globals.Time.TotalSeconds;
            t.ChangeRange((float)Globals.Time.TotalSeconds);
        }

        Tears.RemoveAll(t => t.Range <= 0);
    }

    public void Draw(GameData gameData)
    {
        foreach (var t in Tears)
        {
            Globals.SpriteBatch.Draw(texture, new Vector2(t.Position.X, t.Position.Y + 10), Color.White);
        }
    }
}