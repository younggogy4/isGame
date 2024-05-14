using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame;

public class BombManager
{
    public List<BombData> Bombs;
    private Texture2D texture;
    private Timer timer = new();
    private bool changePos;
    
    public void Init(GameData gameData)
    {
        texture = Globals.Content.Load<Texture2D>("bomb");
        Bombs = new List<BombData>();
        gameData.BombData.Count = 2;
        for (var i = 0; i < gameData.BombData.Count; i++)
        {
            Bombs.Add(new BombData());
        }
    }

    public void Draw(GameData gameData)
    {
        if (InputManager.DropBomb && Bombs.Count > 0)
        {
            
            var heroLastPos = new Vector2(gameData.HeroUpdate.Position.X, gameData.HeroUpdate.Position.Y);
            timer.Update();
            if (timer.GetTime < 2)
            {
                if (!changePos) gameData.BombData.Position = heroLastPos;
                Globals.SpriteBatch.Draw(texture, gameData.BombData.Position, Color.White);
                changePos = true;
            }
            else InputManager.DropBomb = false;
        }
    }

    public void Update(GameData gameData)
    {
        if (InputManager.DropBomb && Bombs.Count > 0)
        {
           
        }
        if (timer.GetTime > 2)
        {
            gameData.BombData.CanExplosion = true;
            InputManager.DropBomb = false;
            Bombs.RemoveAt(Bombs.Count - 1);
            
            timer.Reset();
            changePos = false;
            gameData.BombData.BlastRadius = new Rectangle((int)gameData.BombData.Position.X - 100 + texture.Width / 2,
                (int)gameData.BombData.Position.Y - 100 + texture.Height / 2, 200, 200);
            if (gameData.BombData.BlastRadius.Intersects(gameData.HeroUpdate.Bounds))
            {
                gameData.HeroUpdate.Healths -= 2;
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

            gameData.EnemyManager.Enemies.Where(x => gameData.BombData.BlastRadius.Intersects(x.Bounds)).Select(x =>
            {
                x.Helths -= 2;
                return x;
            }).ToList();

            gameData.JugManager.Jugs
                .Where(x => gameData.BombData.BlastRadius.Intersects(x.Bounds))
                .Select(x =>
                {
                    gameData.DroppedBombData.Taken = false;
                    gameData.JugManager.Jugs.Remove(x);
                    var rnd = new Random().Next(0, 99);
                    //1 = бомба 2 = хп
                    if (rnd <= 100)
                    {
                        gameData.DroppedBombData.Position = x.Position;
                        gameData.DroppedBombData.Dropped = true;
                        gameData.DroppedBombData.Bounds = 
                            new Rectangle((int)gameData.DroppedBombData.Position.X, (int)gameData.DroppedBombData.Position.Y, 50, 50);
                    }
                    return x;
                }).ToList();
        }

        gameData.BombData.CanExplosion = false;
    }

}