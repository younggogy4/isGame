using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MyGame;

public class HeroUpdate
{
    public Rectangle Bounds { get; set; }
    private int TextureWidth { get; }
    private int TextureHeight { get; }
    public int Healths;
    public Vector2 Position;
    public int Speed;
    private Timer timer = new();
    private bool tacheble = true;
    public int Damage=10;

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
        if (InputManager.Direction != Vector2.Zero)
            movement = Vector2.Normalize(InputManager.Direction) * Speed * (float)Globals.Time.TotalSeconds;

        var newPosition = Position + movement;
        var probablyBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, TextureWidth, TextureHeight);
        
        if (Position.Y + TextureHeight >= gameData.Height - 180 || Position.Y <= 45)
        {
            Position.Y = Math.Clamp(Position.Y, 0, gameData.Height - TextureHeight);
            // Отражаем сердечко по вертикали
            Position.Y -= Position.Y + TextureHeight >= gameData.Height - 180 ? 2f : -2f;
        }

        // Проверка столкновения с правой и левой границами экрана
        if (Position.X + TextureWidth >= gameData.Width - 160 || Position.X <= 100)
        {
            Position.X = Math.Clamp(Position.X, 0, gameData.Width - TextureWidth);
            // Отражаем сердечко по горизонтали
            Position.X -= Position.X + TextureWidth >= gameData.Width - 160 ? 2f : -2f;
        }

        // Проверяем столкновение с Jug перед изменением позиции
        foreach (var jug in gameData.JugManager.Jugs)
        {
            if (jug.Bounds.Intersects(probablyBounds))
            {
                if (jug.Bounds.Top - Bounds.Bottom >= 0 && jug.Bounds.Top - Bounds.Bottom < 50)
                {
                    if (InputManager.Direction == new Vector2(1, 0) || InputManager.Direction == new Vector2(1, 1))
                    {
                        Position.X += 4; // Смещаем героя вверх
                    }

                    else if (InputManager.Direction == new Vector2(-1, 0) ||
                             InputManager.Direction == new Vector2(-1, 1))
                    {
                        Position.X -= 4; // Смещаем героя вверх
                    }

                    else if (jug.Position.X > Position.X)
                        Position.X -= 4; // Смещаем героя вверх
                    else if (jug.Position.X < Position.X)
                        Position.X += 4; // Смещаем героя вниз
                } //1

                if (Bounds.Top - jug.Bounds.Bottom >= 0 && Bounds.Top - jug.Bounds.Bottom < 50)
                {
                    if (InputManager.Direction == new Vector2(1, 0) || InputManager.Direction == new Vector2(1, -1))
                    {
                        Position.X += 4; // Смещаем героя вверх
                    }

                    else if (InputManager.Direction == new Vector2(-1, 0) ||
                             InputManager.Direction == new Vector2(-1, -1))
                    {
                        Position.X -= 4; // Смещаем героя вверх
                    }

                    else if (jug.Position.X > Position.X)
                        Position.X -= 4; // Смещаем героя вверх
                    else if (jug.Position.X < Position.X)
                        Position.X += 4; // Смещаем героя вниз
                } //2

                if (jug.Bounds.Left - Bounds.Right >= 0 && jug.Bounds.Left - Bounds.Right < 50)
                {
                    if (InputManager.Direction == new Vector2(0, 1) || InputManager.Direction == new Vector2(1, 1))
                    {
                        Position.Y += 4; // Смещаем героя вверх
                    }

                    else if (InputManager.Direction == new Vector2(0, -1) ||
                             InputManager.Direction == new Vector2(1, -1))
                    {
                        Position.Y -= 4; // Смещаем героя вверх
                    }

                    else if (jug.Position.Y > Position.Y)
                        Position.Y -= 4; // Смещаем героя вверх
                    else if (jug.Position.Y < Position.Y)
                        Position.Y += 4; // Смещаем героя вниз
                } //3

                if (Bounds.Left - jug.Bounds.Right >= 0 && jug.Bounds.Left - Bounds.Right < 50)
                {
                    if (InputManager.Direction == new Vector2(0, 1) || InputManager.Direction == new Vector2(-1, 1))
                    {
                        Position.Y += 4; // Смещаем героя вверх
                    }

                    else if (InputManager.Direction == new Vector2(0, -1) ||
                             InputManager.Direction == new Vector2(-1, -1))
                    {
                        Position.Y -= 4; // Смещаем героя вверх
                    }

                    else if (jug.Position.Y > Position.Y)
                        Position.Y -= 4; // Смещаем героя вверх
                    else if (jug.Position.Y < Position.Y)
                        Position.Y += 4; // Смещаем героя вниз
                } //4
            }
        }

        // Проверяем столкновение с другими препятствиями
        if (Globals.InBounds(newPosition, gameData) &&
            gameData.JugManager.Jugs.All(jug => !jug.Bounds.Intersects(probablyBounds)))
        {
            // Если нет столкновения с другими препятствиями, то меняем позицию героя
            Position = newPosition;
        }

        timer.Update();

        foreach (var enemy in gameData.EnemyManager.Enemies)
        {
            if (!enemy.Bounds.Intersects(Bounds)) continue;
            var directionToEnemy = Vector2.Normalize(Position - enemy.Position);
            var repelVector = directionToEnemy * 100f;
            var probablyPos = repelVector * 3f * (float)Globals.Time.TotalSeconds;
            if (Globals.InBounds(Position + probablyPos, gameData))
                Position += probablyPos;
            if (tacheble)
            {
                Healths--;
                gameData.HeartManager.Hearts[^1].CountHeart--;
                if (gameData.HeartManager.Hearts[^1].CountHeart <= 0)
                    gameData.HeartManager.Hearts.RemoveAt(gameData.HeartManager.Hearts.Count - 1);
                tacheble = false;
                timer.Reset();
            }

            if (!(timer.GetTime > 1)) continue;
            tacheble = true;
            timer.Reset();
        }

        Bounds = new Rectangle((int)Position.X, (int)Position.Y, TextureWidth, TextureHeight);
    }
}