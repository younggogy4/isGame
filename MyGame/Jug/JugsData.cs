using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class JugsData
{
    public Texture2D Texture;
    public Rectangle Bounds;
    public int countSpawn;
    public Vector2 Position;
    
    public JugsData()
    {
        countSpawn = new Random().Next(0, 7);
    }

}