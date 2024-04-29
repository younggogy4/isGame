using Microsoft.Xna.Framework;

namespace MyGame;

public class DropHeartDraw
{
    private Timer timer = new();
    
    public void Draw(GameData gameData)
    {
        var dropHeartUpdate = gameData.DropHeartUpdate;
        if (dropHeartUpdate.CountCheckChance < 1)
        {
            dropHeartUpdate.CountCheckChance++;
            dropHeartUpdate.CheckChance();
        }

        if (!dropHeartUpdate.ChanceDropHeart) return;
        timer.Update();
        if (timer.GetTime>0.6)
        {
            dropHeartUpdate.canTakeHeart = true;
            if (dropHeartUpdate.dropHeart) Globals.SpriteBatch.Draw(dropHeartUpdate.textureHeart,
                new Rectangle((int)dropHeartUpdate.Position.X, (int)dropHeartUpdate.Position.Y, dropHeartUpdate.TextureWidth, dropHeartUpdate.TextureHeight),
                new Rectangle(0, 0, dropHeartUpdate.textureHeart.Width, dropHeartUpdate.textureHeart.Height),
                Color.White);
            if (dropHeartUpdate.dropHalfHeart) Globals.SpriteBatch.Draw(dropHeartUpdate.textureHalfHeart,
                new Rectangle((int)dropHeartUpdate.Position.X, (int)dropHeartUpdate.Position.Y, dropHeartUpdate.TextureWidth, dropHeartUpdate.TextureHeight),
                new Rectangle(0, 0, dropHeartUpdate.textureHalfHeart.Width, dropHeartUpdate.textureHalfHeart.Height),
                Color.White);
        }
    }
}