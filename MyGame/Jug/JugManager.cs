using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace MyGame;

public class JugManager
{
    public List<JugsData> Jugs= new ();
    private Random random = new ();
    private List<Rectangle> spawnRates = new List<Rectangle>();
    private Texture2D texture;
    
    public void Init(GameData gameData)
    {
        spawnRates.Add(new Rectangle(200, 200, 300, 300));
        spawnRates.Add(new Rectangle(200, gameData.Height-600, 300, 300));
        spawnRates.Add(new Rectangle(gameData.Width-600, 200, 300, 300));
        spawnRates.Add(new Rectangle(gameData.Width-600, gameData.Height-600, 300, 300));
        spawnRates.Add(new Rectangle(gameData.Width/2 - 150, gameData.Height/2-150, 300, 300));
        texture = Globals.Content.Load<Texture2D>("jug");
    }

    public void Update(GameData gameData)
    {
        while (gameData.JugsData.countSpawn > 0)
        {
            var randomIndex = new Random().Next(0, 4);
            var randomPos = new Vector2(random.Next(spawnRates[randomIndex].Left, spawnRates[randomIndex].Right), 
                random.Next(spawnRates[randomIndex].Top, spawnRates[randomIndex].Bottom));
            if (Jugs.Any(x =>
                    Math.Abs(x.Position.X - randomPos.X) <= 50 || Math.Abs(x.Position.Y - randomPos.Y) <= 50)) continue;

            var newJugsData = new JugsData();
            newJugsData.Texture = texture;
            newJugsData.Position=randomPos;
            Jugs.Add(newJugsData);
            gameData.JugsData.countSpawn--;
            MakeCollision();
        }
    }

    private void MakeCollision()
    {
        foreach (var jug in Jugs)
        {
            jug.Bounds = new Rectangle((int)jug.Position.X-20, (int)jug.Position.Y-30, jug.Texture.Width+15, jug.Texture.Height+20);
        }
    }

    public void Draw()
    {
        foreach (var jug in Jugs)
        {
            Globals.SpriteBatch.Draw(jug.Texture, jug.Position, Color.White);
        }
    }
}