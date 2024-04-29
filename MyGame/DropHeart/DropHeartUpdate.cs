using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SharpDX.Direct2D1;

namespace MyGame;

public class DropHeartUpdate
{
    public Vector2 Position;
    public readonly Texture2D textureHalfHeart;
    public readonly Texture2D textureHeart;
    public int TextureWidth;
    public int TextureHeight;
    private Random chance;
    public bool ChanceDropHeart;
    public int CountCheckChance;
    public Rectangle Bounds { get; set; }
    public bool canTakeHeart;
    public bool dropHeart;
    public bool dropHalfHeart;

    public DropHeartUpdate(GameData gameData, Texture2D textureHalfHeart, Texture2D textureHeart)
    {
        chance = new Random();
        Position = new Vector2(gameData.Width / 2, gameData.Height / 2);
        this.textureHalfHeart = textureHalfHeart;
        this.textureHeart = textureHeart;
        TextureWidth = 50;
        TextureHeight = 40;
        Bounds = (Rectangle)new EllipseF(Position, TextureWidth, TextureHeight).BoundingRectangle;
    }
    
    public void Update(GameData gameData)
    {
        // Проверка столкновения с нижней и верхней границами экрана
        if (Position.Y + gameData.DropHeartUpdate.TextureHeight >= gameData.Height - 180 || Position.Y <= 45)
        {
            Position.Y = Math.Clamp(Position.Y, 0, gameData.Height - gameData.DropHeartUpdate.TextureHeight);
            // Отражаем сердечко по вертикали
            Position.Y -= Position.Y + gameData.DropHeartUpdate.TextureHeight >= gameData.Height - 180 ? 5f : -5f;
        }

        // Проверка столкновения с правой и левой границами экрана
        if (Position.X + gameData.DropHeartUpdate.TextureWidth >= gameData.Width - 160 || Position.X <= 100)
        {
            Position.X = Math.Clamp(Position.X, 0, gameData.Width - gameData.DropHeartUpdate.TextureWidth);
            // Отражаем сердечко по горизонтали
            Position.X -= Position.X + gameData.DropHeartUpdate.TextureWidth >= gameData.Width - 160 ? 5f : -5f;
        }

        Bounds = (Rectangle)new EllipseF(Position, gameData.DropHeartUpdate.TextureWidth, gameData.DropHeartUpdate.TextureHeight).BoundingRectangle;

        if (gameData.HeroUpdate.Healths <= 0)
        {
            gameData.CanPickUpHeart = false;
            return;
        }

        if (gameData.HeartManager.Hearts[^1].CountHeart == 2 || gameData.HeartManager.Hearts[^1].CountHeart == 1)
            gameData.CanPickUpHeart = true; //можно подобрать хп, если последнее сердечко не полное

        if (gameData.HeroUpdate.Bounds.Intersects(Bounds) && gameData.CanPickUpHeart && canTakeHeart)
        {
            ChanceDropHeart = false;
            gameData.PickUpHeart = true;
            gameData.CanPickUpHeart = false;
            canTakeHeart = false;
        }

        if (gameData.PickUpHeart) //можно подобрать хп, только если последнее сердечко не полное
        {
            //Здесь проверяется сколько нужно хп добавить персонажу, в зависимости от текущего здоровья.
            if (dropHeart && gameData.HeroUpdate.Healths % 3 == 1) gameData.HeroUpdate.Healths += 2;
            //если сердечко пустое, и хиро подбирает фулхп, то получает тоже полное хп.
            if (dropHeart && gameData.HeroUpdate.Healths % 3 == 2) gameData.HeroUpdate.Healths++;
            //если сердечко хотя бы на половину заполнено и хиро подбирает фулхп, то получает пол хп.
            if (dropHalfHeart) gameData.HeroUpdate.Healths++;
            //в любом случае получает половину хп, если подбирает половину хп.
            gameData.HeartManager.FillHearts(gameData);
            gameData.PickUpHeart = false;
            //убирает сердечко из игры, когда его подбирают
        }


        if (!gameData.HeroUpdate.Bounds.Intersects(Bounds) || !canTakeHeart)
        {
            gameData.CanPickUpHeart = false;
            //проверка на то, что герой подобрал сердечко.
            return;
        }

        var directionToHero = Vector2.Normalize(Position - gameData.HeroUpdate.Position);
        var repelVector = directionToHero * 5f;
        var newPosition = Position + repelVector;
        if (Globals.InBounds(newPosition, gameData))
            Position = newPosition;

        Bounds = (Rectangle)new EllipseF(Position, TextureWidth, TextureHeight).BoundingRectangle;
        gameData.CanPickUpHeart = false;
    }

    public void CheckChance()
    {
        var nextChance = chance.Next(50, 98);

        if (nextChance < 100 && !ChanceDropHeart && !dropHalfHeart)
        {
            ChanceDropHeart = true;
            dropHeart = true;
            dropHalfHeart = false;
        }

        if (nextChance < 99 && nextChance > 14 && !ChanceDropHeart && !dropHeart)
        {
            ChanceDropHeart = true;
            dropHalfHeart = true;
            dropHeart = false;
        }
    }
}