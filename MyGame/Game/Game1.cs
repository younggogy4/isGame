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

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        graphics.PreferredBackBufferWidth = 1650;
        graphics.PreferredBackBufferHeight = 1000;
        graphics.ApplyChanges();

        Globals.Content = Content;
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
        Globals.Update(gameTime);
        roomManager.Rooms[roomManager.Index].GameUpdate.Update(roomManager.Rooms[roomManager.Index].GameData);
        roomManager.DrawUpdate(roomManager.Rooms[roomManager.Index].GameData);

        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Beige);
        spriteBatch.Begin();
        roomManager.Rooms[roomManager.Index].GameDraw.Draw(roomManager.Rooms[roomManager.Index].GameData);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}