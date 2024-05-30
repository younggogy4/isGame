using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class GameData
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public DropHeartUpdate DropHeartUpdate;
    public DropHeartDraw DropHeartDraw = new();
    public HeroDraw DrawHero = new();
    public BackGround BackGround = new();
    public int CountUpdateHeroHelths;
    public HeroUpdate HeroUpdate;
    public TearData TearData = new();
    public int SpeedTear = 300;
    public float RangeTear = 0.8f;
    public Random Random = new();
    public EnemyData EnemyData = new();
    public bool CanPickUpHeart;
    public bool PickUpHeart;
    public int LimitSpawnEnemy;
    public EnemyManager EnemyManager = new();
    public JugsData JugsData = new();
    public BombData BombData = new();
    public DoorManager DoorManager = new();
    public JugManager JugManager = new();
    public TearsManager TearsManager = new();
    public BombManager BombManager = new();
    public HeartManager HeartManager = new();
    public LukeData LukeData = new();
    public LukeManager LukeManager = new();
    public EndScreen EndScreen = new();
    public BossManager BossManager = new();
    public TearBossManager TearBossManager = new();
    public TearBossData TearBossData = new();
    public DroppedBombData DroppedBombData = new();
    public DroppedBombManager DroppedBombManager = new();
    public PuddleBossData PuddleBossData = new();
    public PuddleBossManager PuddleBossManager = new();
    public RecurringItemsData RecurringItemsData = new();
    public RecurringItems RecurringItems = new();
    public BossData BossData = new ();
    public List<IManager> Managers;
    public InputManager InputManager = new ();
    public RoomManager RoomManager = new ();

    
    public void Init()
    {
        var managers = new Managers(this);
        Managers = managers.ManagersList;
        Width = Game1.graphics.PreferredBackBufferWidth;
        Height = Game1.graphics.PreferredBackBufferHeight;
        DropHeartUpdate = new DropHeartUpdate(this, Globals.Content.Load<Texture2D>("halfDroppedHeart"),
            Globals.Content.Load<Texture2D>("droppedHeart"));
        HeroUpdate = new HeroUpdate(this);
        foreach (var manager in Managers)
            manager.Init(this, HeroUpdate);
        
    }
}