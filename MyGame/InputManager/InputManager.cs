using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Timers;

namespace MyGame
{
    public enum Button
    {
        Up = Keys.I,
        Down = Keys.K,
        Left = Keys.J,
        Right = Keys.L,
        None
    }

    public class InputManager : IManager
    {
        private Vector2 direction;
        public Vector2 Direction => direction;
        private Button button = Button.None;
        private double speedTear;
        private Timer timer;
        private bool canShoot = true;
        public bool DropBomb;

        public Button DirectionShoot => button;

        private void GetDirectionMove(KeyboardState keyboardState)
        {
            direction = Vector2.Zero;
            button = Button.None;
            if (keyboardState.IsKeyDown(Keys.A)) direction.X--;
            if (keyboardState.IsKeyDown(Keys.D)) direction.X++;
            if (keyboardState.IsKeyDown(Keys.W)) direction.Y--;
            if (keyboardState.IsKeyDown(Keys.S)) direction.Y++;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            canShoot = true;
            timer.Stop();
        }

        private void GetShootDirection(Keys key)
        {
            if (canShoot)
            {
                button = (Button)key;
                canShoot = false;
                timer.Start();
            }
        }

        public void Init(GameData gameData, HeroUpdate hero)
        {
            speedTear = 0.4;
            timer = new Timer(speedTear * 1000); // speedTear in seconds
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = false; // Make sure the timer fires only once per interval
        }

        public void Draw(GameData gameData)
        {
        }

        public void Update(GameData gameData)
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
}