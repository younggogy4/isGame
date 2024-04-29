using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class DoorData
{
    public int countDoor = new Random().Next(1, 4);
    public Vector2 Position;
    public Rectangle Bounds;
}