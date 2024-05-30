using System.Collections.Generic;

namespace MyGame;

public class Managers
{
    public readonly List<IManager> ManagersList = new();

    public Managers(GameData gameData)
    {
        ManagersList.Clear();
        ManagersList.Add(gameData.InputManager);
        ManagersList.Add(gameData.BombManager);
        ManagersList.Add(gameData.PuddleBossManager);
        ManagersList.Add(gameData.TearBossManager);
        ManagersList.Add(gameData.DroppedBombManager);
        ManagersList.Add(gameData.JugManager);
        ManagersList.Add(gameData.LukeManager);
        ManagersList.Add(gameData.EndScreen);
        ManagersList.Add(gameData.EnemyManager);
        ManagersList.Add(gameData.DoorManager);
        ManagersList.Add(gameData.TearsManager);
        ManagersList.Add(gameData.HeartManager);
        ManagersList.Add(gameData.BossManager);
    }
}