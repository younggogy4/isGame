using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MyGame;

public enum Button
{
    Up = Keys.I,
    Down = Keys.K,
    Left = Keys.J,
    Right = Keys.L,
    None
}

public static class InputManager
{
    private static Vector2 direction;
    public static Vector2 Direction => direction;
    private static Button button;
    private static double speedTear;
    private static Timer holdTimer = new ();
    public static bool DropBomb;
    
    public static Button DirectionShoot => button;

    static InputManager()
    {
        speedTear = 0.4;
    }
    
    private static void GetDirectionMove(KeyboardState keyboardState)
    {
        direction = Vector2.Zero;
        button = Button.None;
        if (keyboardState.IsKeyDown(Keys.A)) direction.X--;
        if (keyboardState.IsKeyDown(Keys.D)) direction.X++;
        if (keyboardState.IsKeyDown(Keys.W)) direction.Y--;
        if (keyboardState.IsKeyDown(Keys.S)) direction.Y++;
    }

    private static bool GetEpsilon(float first, double second)
    {
        return Math.Abs(first % second) < 1e-2;
    }
    private static void GetShootDirection(Keys key)
    {
        if (GetEpsilon(holdTimer.GetTime, speedTear))
        {
            button = (Button)key;
        }
        holdTimer.Update();
        if (Math.Abs(2-holdTimer.GetTime)<1e-2) holdTimer.Reset();
    }
    
    public static void Update()
    {
        var keyboardState = Keyboard.GetState();
        GetDirectionMove(keyboardState);
        if (keyboardState.IsKeyDown(Keys.I)) GetShootDirection(Keys.I);
        else if (keyboardState.IsKeyDown(Keys.K)) GetShootDirection(Keys.K);
        else if (keyboardState.IsKeyDown(Keys.J)) GetShootDirection(Keys.J);
        else if (keyboardState.IsKeyDown(Keys.L)) GetShootDirection(Keys.L);
        if (keyboardState.IsKeyDown(Keys.Q)) DropBomb = true;
    }
}