namespace MyGame;

public class Characteristics
{
    public int Healths;
    public int Speed;
    public float RangeTear;
    public int SpeedTear;
    public int Damage;
    public string Description;

    public Characteristics(HeroUpdate hero, GameData gameData)
    {
        Healths = hero.Healths;
        Speed = hero.Speed;
        RangeTear = gameData.TearData.Range;
        SpeedTear = gameData.TearData.Speed;
        Damage = hero.Damage;
    }
}