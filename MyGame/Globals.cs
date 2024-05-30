using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public static class Globals
{
    public static TimeSpan Time { get; set; }
    public static ContentManager Content { get; set; }
    public static SpriteBatch SpriteBatch { get; set; }
    public static void Update(GameTime gt)
    {
        Time = gt.ElapsedGameTime;
    }

    public static bool InBounds(Vector2 position, GameData gameData)
    {
        return position is { X: > 95, Y: > 40 } && position.X < gameData.Width - 160 &&
               position.Y < gameData.Height - 180;
    }
}