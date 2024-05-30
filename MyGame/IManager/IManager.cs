namespace MyGame;

public interface IManager
{
    public void Init(GameData gameData, HeroUpdate hero);
    
    public void Draw(GameData gameData);
    
    public void Update(GameData gameData);

}