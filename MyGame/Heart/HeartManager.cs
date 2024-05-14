using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class HeartManager
{
    private Texture2D fullHeart;
    private Texture2D halfHeart;
    private Texture2D emptyHeart;
    public List<Heart> Hearts;

    public void Init()
    {
        fullHeart = Globals.Content.Load<Texture2D>("heart");
        halfHeart = Globals.Content.Load<Texture2D>("halfHeart");
        emptyHeart = Globals.Content.Load<Texture2D>("emptyHeart");
        Hearts = new List<Heart>();
    }
    
    public void FillHearts(GameData gameData)
    {
        Hearts.Clear();
        for (var i = 0; i < gameData.HeroUpdate.Healths / 3; i++)
            Add(new Vector2(200 + 75 * i, 80));

        if (gameData.HeroUpdate.Healths % 3 == 1)
        {
            Add(Hearts.Count > 0 ? new Vector2(Hearts[^1].Position.X + 75, 80) : new Vector2(200, 80));
            Hearts[^1].CountHeart-=2;
        }

        if (gameData.HeroUpdate.Healths % 3 == 2)
        {
            Add(Hearts.Count > 0 ? new Vector2(Hearts[^1].Position.X + 75, 80) : new Vector2(200, 80));
            Hearts[^1].CountHeart--;
        }
    }

    public void Add(Vector2 position)
    {
        Hearts.Add(new Heart(fullHeart, halfHeart, emptyHeart, position));
    }

    public void Draw()
    {
        foreach (var heart in Hearts)
            heart.Draw();
    }
}