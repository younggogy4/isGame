using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MyGame;

public class HeroUpdate
{
    public Rectangle Bounds { get; private set; }
    private int TextureWidth { get; }
    private int TextureHeight { get; }
    public int Healths { get; private set; }
    public Vector2 Position;
    public int Speed { get; private set; }
    public MyTimer Timer = new();
    public bool Tacheble { get; set; } = true;
    public int Damage { get; private set; } = 1;

    public HeroUpdate(GameData gameData)
    {
        Position = new Vector2(gameData.Width / 2, gameData.Height / 2);
        TextureWidth = HeroDraw.Texture.Width / 2;
        TextureHeight = HeroDraw.Texture.Height / 2;
        Bounds = new Rectangle((int)Position.X, (int)Position.Y, TextureWidth, TextureHeight);
        Speed = 300;
        Healths = 9;
    }

    public void Update(GameData gameData)
    {
        if (Healths <= 0 || gameData.HeartManager.Hearts.Count <= 0) return;

        var movement = Vector2.Zero;
        if (gameData.InputManager.Direction != Vector2.Zero)
            movement = Vector2.Normalize(gameData.InputManager.Direction) * Speed * (float)Globals.Time.TotalSeconds;

        var newPosition = Position + movement;
        var probablyBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, TextureWidth, TextureHeight);

        if (Position.Y + TextureHeight >= gameData.Height - 180 || Position.Y <= 45)
        {
            Position.Y = Math.Clamp(Position.Y, 0, gameData.Height - TextureHeight);
            Position.Y -= Position.Y + TextureHeight >= gameData.Height - 180 ? 2f : -2f;
        }

        if (Position.X + TextureWidth >= gameData.Width - 160 || Position.X <= 100)
        {
            Position.X = Math.Clamp(Position.X, 0, gameData.Width - TextureWidth);
            Position.X -= Position.X + TextureWidth >= gameData.Width - 160 ? 2f : -2f;
        }
        
        foreach (var jug in gameData.JugManager.Jugs)
        {
            if (jug.Bounds.Intersects(probablyBounds))
            {
                if (jug.Bounds.Top - Bounds.Bottom >= 0 && jug.Bounds.Top - Bounds.Bottom < 50)
                {
                    if (gameData.InputManager.Direction == new Vector2(1, 0) || gameData.InputManager.Direction == new Vector2(1, 1))
                    {
                        Position.X += 4;
                    }

                    else if (gameData.InputManager.Direction == new Vector2(-1, 0) ||
                             gameData.InputManager.Direction == new Vector2(-1, 1))
                    {
                        Position.X -= 4;
                    }

                    else if (jug.Position.X > Position.X)
                        Position.X -= 4;
                    else if (jug.Position.X < Position.X)
                        Position.X += 4;
                } 

                if (Bounds.Top - jug.Bounds.Bottom >= 0 && Bounds.Top - jug.Bounds.Bottom < 50)
                {
                    if (gameData.InputManager.Direction == new Vector2(1, 0) || gameData.InputManager.Direction == new Vector2(1, -1))
                    {
                        Position.X += 4; 
                    }

                    else if (gameData.InputManager.Direction == new Vector2(-1, 0) ||
                             gameData.InputManager.Direction == new Vector2(-1, -1))
                    {
                        Position.X -= 4; 
                    }

                    else if (jug.Position.X > Position.X)
                        Position.X -= 4; 
                    else if (jug.Position.X < Position.X)
                        Position.X += 4; 
                } 
                
                if (jug.Bounds.Left - Bounds.Right >= 0 && jug.Bounds.Left - Bounds.Right < 50)
                {
                    if (gameData.InputManager.Direction == new Vector2(0, 1) || gameData.InputManager.Direction == new Vector2(1, 1))
                    {
                        Position.Y += 4; 
                    }

                    else if (gameData.InputManager.Direction == new Vector2(0, -1) ||
                             gameData.InputManager.Direction == new Vector2(1, -1))
                    {
                        Position.Y -= 4; 
                    }

                    else if (jug.Position.Y > Position.Y)
                        Position.Y -= 4; 
                    else if (jug.Position.Y < Position.Y)
                        Position.Y += 4; 
                } 

                if (Bounds.Left - jug.Bounds.Right >= 0 && jug.Bounds.Left - Bounds.Right < 50)
                {
                    if (gameData.InputManager.Direction == new Vector2(0, 1) || gameData.InputManager.Direction == new Vector2(-1, 1))
                    {
                        Position.Y += 4; 
                    }

                    else if (gameData.InputManager.Direction == new Vector2(0, -1) ||
                             gameData.InputManager.Direction == new Vector2(-1, -1))
                    {
                        Position.Y -= 4; 
                    }

                    else if (jug.Position.Y > Position.Y)
                        Position.Y -= 4; 
                    else if (jug.Position.Y < Position.Y)
                        Position.Y += 4; 
                } 
            }
        }

        if (Globals.InBounds(newPosition, gameData) &&
            gameData.JugManager.Jugs.All(jug => !jug.Bounds.Intersects(probablyBounds)))
            Position = newPosition;
        
        Timer.Update();
        Bounds = new Rectangle((int)Position.X, (int)Position.Y, TextureWidth, TextureHeight);
    }

    public void ChangeBounds(Rectangle bounds)
    {
        Bounds = bounds;
    }

    public void ChangeSpeed(int speed)
    {
        Speed = speed;
    }

    public void ChangeDamage(int damage)
    {
        Damage = damage;
    }

    public void ChangeHealth(int health)
    {
        Healths = health;
    }
}
