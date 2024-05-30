using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame;

public class BombManager : IManager
{
    public List<BombData> Bombs;
    private Texture2D texture;
    private Timer timer;
    private bool changePos;
    private Vector2 bombPosition;
    private GameData gameData;

    public void Init(GameData gameData, HeroUpdate hero)
    {
        this.gameData = gameData;
        texture = Globals.Content.Load<Texture2D>("bomb");
        Bombs = new List<BombData>();

        InitializeBombs();
        SetupTimer();
    }

    private void InitializeBombs()
    {
        gameData.BombData.Count = 2;
        for (var i = 0; i < gameData.BombData.Count; i++)
        {
            Bombs.Add(new BombData());
        }
    }

    private void SetupTimer()
    {
        timer = new Timer(2000);
        timer.Elapsed += OnTimerElapsed;
        timer.AutoReset = false; // We will manually reset it
    }

    public void Draw(GameData gameData)
    {
        if (gameData.InputManager.DropBomb && Bombs.Count > 0)
        {
            if (!timer.Enabled)
            {
                StartBombTimer(gameData.HeroUpdate.Position);
            }

            if (!changePos)
            {
                ChangeBombPosition();
            }

            DrawBomb();
        }
    }

    private void StartBombTimer(Vector2 heroPosition)
    {
        bombPosition = new Vector2(heroPosition.X, heroPosition.Y);
        changePos = false;
        timer.Start();
    }

    private void ChangeBombPosition()
    {
        gameData.BombData.ChangePos(bombPosition);
        changePos = true;
    }

    private void DrawBomb()
    {
        Globals.SpriteBatch.Draw(texture, gameData.BombData.Position, Color.White);
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        timer.Stop();
        HandleBombExplosion();
        ResetBombState();
    }

    private void ResetBombState()
    {
        gameData.InputManager.DropBomb = false;
        if (Bombs.Count > 0)
        {
            Bombs.RemoveAt(Bombs.Count - 1);
        }
        changePos = false;
    }

    private void HandleBombExplosion()
    {
        SetBlastRadius();
        DamageHeroIfInBlastRadius();
        DamageEnemiesInBlastRadius();
        HandleJugDestructionAndDrops();
    }

    private void SetBlastRadius()
    {
        gameData.BombData.SetBlastRadius(new Rectangle(
            (int)gameData.BombData.Position.X - 100 + texture.Width / 2,
            (int)gameData.BombData.Position.Y - 100 + texture.Height / 2,
            200, 200));
    }

    private void DamageHeroIfInBlastRadius()
    {
        if (gameData.BombData.BlastRadius.Intersects(gameData.HeroUpdate.Bounds))
        {
            gameData.HeroUpdate.ChangeHealth(gameData.HeroUpdate.Healths - 2);
            HandleHeroHealthAndHearts();
        }
    }

    private void HandleHeroHealthAndHearts()
    {
        if (gameData.HeartManager.Hearts[^1].CountHeart >= 2)
        {
            gameData.HeartManager.Hearts[^1].CountHeart -= 2;
            if (gameData.HeartManager.Hearts[^1].CountHeart < 1)
                gameData.HeartManager.Hearts.RemoveAt(gameData.HeartManager.Hearts.Count - 1);
        }
        else
        {
            gameData.HeartManager.Hearts.RemoveAt(gameData.HeartManager.Hearts.Count - 1);
            if (gameData.HeartManager.Hearts.Count > 0) gameData.HeartManager.Hearts[^1].CountHeart--;
        }
    }

    private void DamageEnemiesInBlastRadius()
    {
        foreach (var enemy in gameData.EnemyManager.Enemies.Where(x => gameData.BombData.BlastRadius.Intersects(x.Bounds)))
        {
            enemy.ReduceHealth(2);
        }
    }

    private void HandleJugDestructionAndDrops()
    {
        var removedJugs = new List<JugsData>();

        foreach (var jug in gameData.JugManager.Jugs.Where(x => gameData.BombData.BlastRadius.Intersects(x.Bounds)))
        {
            gameData.DroppedBombData.Taken = false;
            removedJugs.Add(jug);

            if (new Random().Next(0, 99) <= 10)
            {
                DropBombAtJugPosition(jug);
            }
        }

        RemoveDestroyedJugs(removedJugs);
    }

    private void DropBombAtJugPosition(JugsData jug)
    {
        gameData.DroppedBombData.ChangePosition(jug.Position);
        gameData.DroppedBombData.Dropped = true;
        gameData.DroppedBombData.ChangeBounds(new Rectangle(
            (int)gameData.DroppedBombData.Position.X,
            (int)gameData.DroppedBombData.Position.Y,
            50, 50));
    }

    private void RemoveDestroyedJugs(List<JugsData> removedJugs)
    {
        foreach (var jug in removedJugs)
        {
            gameData.JugManager.Jugs.Remove(jug);
        }
    }

    public void Update(GameData gameData)
    {
    }
}