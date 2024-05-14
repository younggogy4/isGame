using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class Characteristics
{
    public int Healths;
    public int Speed;
    public float RangeTear;
    public int SpeedTear;
    public int Damage;
    public string Description;

    public Characteristics(GameData gameData)
    {
        Healths = gameData.HeroUpdate.Healths;
        Speed = gameData.HeroUpdate.Speed;
        RangeTear = gameData.TearData.Range;
        SpeedTear = gameData.TearData.Speed;
        Damage = gameData.HeroUpdate.Damage;
    }
}