using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using UnicycleSheep;

namespace DekoElements
{
    public class RemoteControllHand
    {
        Sprite baseSprite;
        Sprite leftThumbSprite;
        Sprite rightThumbSprite;

        public RemoteControllHand(uint index)
        {
            Vector2 pos = Vector2.One * 30F;
            float scale = 0.5F;

            baseSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.Hand1));
            baseSprite.Scale = Vector2.One * scale;
            baseSprite.Position = pos;
            leftThumbSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Mid));
            leftThumbSprite.Scale = Vector2.One * scale;
            leftThumbSprite.Position = pos;
            rightThumbSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Mid));
            rightThumbSprite.Scale = Vector2.One * scale;
            rightThumbSprite.Position = pos;
        }

        public void draw(RenderWindow win)
        {
            win.Draw(baseSprite);
            
            if (KeyboardInputManager.isPressed(Keyboard.Key.A)) 
            { leftThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Leftmost); }
            else if (KeyboardInputManager.isPressed(Keyboard.Key.D)) 
            { leftThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Rightmost); }
            else 
            { leftThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Mid); }
            win.Draw(leftThumbSprite);

            if (KeyboardInputManager.isPressed(Keyboard.Key.Left))
            { rightThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Leftmost); }
            else if (KeyboardInputManager.isPressed(Keyboard.Key.Right))
            { rightThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Rightmost); }
            else
            { rightThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Mid); }
            win.Draw(rightThumbSprite);
        }

    }
}