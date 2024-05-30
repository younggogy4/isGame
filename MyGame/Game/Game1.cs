using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame;

public class Game1 : Game
{
    public static GraphicsDeviceManager graphics { get; private set; }
    private SpriteBatch spriteBatch;
    private GameData gameData;
    private GameDraw gameDraw;
    private GameUpdate gameUpdate;
    private RoomManager roomManager;
    private Screen screens = Screen.MainMenuScreen;
    private List<Texture2D> screensTexture;
    private bool isSpaceDown;
    private bool exitGame;
    private bool pauseDown;
    private bool pauseUp;
    private bool startNewGame;
    private bool isSpaceUp;
    private bool notExitGame;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        graphics.PreferredBackBufferWidth = 1920;
        graphics.PreferredBackBufferHeight = 1000;
        graphics.ApplyChanges();

        Globals.Content = Content;
        screensTexture = new List<Texture2D>();
        screensTexture.Add(Content.Load<Texture2D>("MainScreen"));
        screensTexture.Add(Content.Load<Texture2D>("EndScreen"));
        screensTexture.Add(Content.Load<Texture2D>("Pause"));


        roomManager = new();
        roomManager.Init();
        base.Initialize();
    }


    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.SpriteBatch = spriteBatch;
    }

    protected override void Update(GameTime gameTime)
    {
        var room = roomManager.Rooms[RoomManager.IndexRooms];
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Space)) isSpaceDown = true;
        //if (keyboardState.IsKeyDown(Keys.Escape)) exitGame = true;
        if (keyboardState.IsKeyDown(Keys.Escape)) pauseDown = true;
        if (keyboardState.IsKeyUp(Keys.Escape) && pauseDown)
        {
            pauseUp = !pauseUp;
            pauseDown = false;
        }

        if (keyboardState.IsKeyUp(Keys.Space) && isSpaceDown)
        {
            isSpaceUp = !isSpaceUp;
            isSpaceDown = false;
        }
        
        /*if (keyboardState.IsKeyUp(Keys.Escape) && exitGame)
        {
            notExitGame = !notExitGame;
            exitGame = false;
        }*/
        
        Globals.Update(gameTime);

        if (screens == Screen.GameScreen && !pauseUp || (screens == Screen.PauseScreen && !pauseUp))
        {
            screens = Screen.GameScreen;
            room.GameUpdate.Update(room.GameData);
            roomManager.Update();
        }
        
        if (screens == Screen.GameScreen && pauseUp)
        {
            screens = Screen.PauseScreen;
        }

        if (isSpaceDown && screens == Screen.MainMenuScreen) screens = Screen.GameScreen;
        
        if (room.GameData.EndScreen.EndGame) screens = Screen.EndScreen;
        if (screens == Screen.EndScreen && pauseUp)
        {
            screens = Screen.MainMenuScreen;
            roomManager = new();
            RoomManager.IndexRooms = 0;
            RoomManager.IndexLevels = 1;
            roomManager.Init();
            pauseDown = true;
        }
        
        
        else if (screens == Screen.MainMenuScreen && pauseUp) Exit();
        
        if (screens == Screen.GameScreen && room.GameData.HeroUpdate.Healths<1)
        {
            screens = Screen.EndScreen;
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        var room = roomManager.Rooms[RoomManager.IndexRooms];
        GraphicsDevice.Clear(Color.Beige);
        spriteBatch.Begin();
        if (screens == Screen.MainMenuScreen)
        {
            spriteBatch.Draw(screensTexture[0], new Rectangle(0, 0, 1920, 1000), Color.White);
        }

        if (screens == Screen.GameScreen)
        {
            room.GameDraw.Draw(room.GameData);
        }


        if (screens == Screen.EndScreen)
        {
            spriteBatch.Draw(screensTexture[1], new Rectangle(0, 0, 1920, 1000), Color.White);
        }

        if (screens == Screen.PauseScreen)
        {
            spriteBatch.Draw(screensTexture[2], new Rectangle(0, 0, 1920, 1000), Color.White);
        }

        spriteBatch.End();


        base.Draw(gameTime);
    }
}