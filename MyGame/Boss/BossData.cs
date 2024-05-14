﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class BossData
{
    public Texture2D Texture;
    public Vector2 Position;
    public Rectangle Bounds;
    public int Health;
    public Vector2 Direction;
    public float Speed;
    public int MaxHealth;
    public int Damage;
}