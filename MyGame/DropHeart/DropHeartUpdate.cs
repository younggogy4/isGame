using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MyGame;

public class DropHeartUpdate
{
    private Vector2 position;
    public Vector2 Position => position;
    public Texture2D TextureHalfHeart { get; }
    public Texture2D TextureHeart { get; }
    public int TextureWidth { get; }
    public int TextureHeight { get; }
    private Random chance;
    public bool ChanceDropHeart { get; set; }
    public int CountCheckChance { get; set; }
    public Rectangle Bounds { get; private set; }
    public bool CanTakeHeart { get; set; }
    public bool DropHeart { get; private set; }
    public bool DropHalfHeart { get; private set; }

    public DropHeartUpdate(GameData gameData, Texture2D textureHalfHeart, Texture2D textureHeart)
    {
        chance = new Random();
        position = new Vector2(gameData.Width / 2 + 500, gameData.Height / 2 + 200);
        TextureHalfHeart = textureHalfHeart;
        TextureHeart = textureHeart;
        TextureWidth = 50;
        TextureHeight = 40;
        UpdateBounds();
    }

    public void Update(GameData gameData)
    {
        UpdatePositionY(gameData);
        UpdatePositionX(gameData);
        UpdateBounds();
    }

    private void UpdatePositionY(GameData gameData)
    {
        if (position.Y + TextureHeight >= gameData.Height - 180 || position.Y <= 45)
        {
            position.Y = Math.Clamp(position.Y, 0, gameData.Height - TextureHeight);
            position.Y -= position.Y + TextureHeight >= gameData.Height - 180 ? 5f : -5f;
        }
    }

    private void UpdatePositionX(GameData gameData)
    {
        if (position.X + TextureWidth >= gameData.Width - 160 || position.X <= 100)
        {
            position.X = Math.Clamp(position.X, 0, gameData.Width - TextureWidth);
            position.X -= position.X + TextureWidth >= gameData.Width - 160 ? 5f : -5f;
        }
    }

    private void UpdateBounds()
    {
        Bounds = (Rectangle)new EllipseF(position, TextureWidth, TextureHeight).BoundingRectangle;
    }

    public void CheckChance()
    {
        var nextChance = chance.Next(0, 98);

        if (ShouldDropHeart(nextChance))
        {
            DropHeart = true;
            DropHalfHeart = false;
            ChanceDropHeart = true;
        }
        else if (ShouldDropHalfHeart(nextChance))
        {
            DropHalfHeart = true;
            DropHeart = false;
            ChanceDropHeart = true;
        }
    }

    private bool ShouldDropHeart(int nextChance)
    {
        return nextChance < 15 && !ChanceDropHeart && !DropHalfHeart;
    }

    private bool ShouldDropHalfHeart(int nextChance)
    {
        return nextChance < 45 && nextChance > 30 && !ChanceDropHeart && !DropHeart;
    }

    public void ChangePosition(Vector2 newPosition)
    {
        position = newPosition;
        UpdateBounds();
    }

    public void ChangeBounds(Rectangle newBounds)
    {
        Bounds = newBounds;
    }
}