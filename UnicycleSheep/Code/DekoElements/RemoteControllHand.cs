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

        uint index;

        public RemoteControllHand(uint index)
        {
            this.index = index;

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

            if (GamePadInputManager.isConnected(index))
            {
                float threshold = 0.8F;

                float leftStickValue = GamePadInputManager.getLeftStick(index).X;
                if (leftStickValue < -threshold)
                {   leftThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Leftmost); }
                else if (leftStickValue < -float.Epsilon)
                {   leftThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Left); }
                else if (leftStickValue <= float.Epsilon)
                {   leftThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Mid); }
                else if (leftStickValue <= threshold)
                {   leftThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Right); }
                else
                {   leftThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Rightmost); }
                
                float rightStickValue = GamePadInputManager.getRightStick(index).X;
                if (rightStickValue < -threshold)
                {   rightThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Leftmost); }
                else if (rightStickValue < -float.Epsilon)
                {   rightThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Left); }
                else if (rightStickValue <= float.Epsilon)
                {   rightThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Mid); }
                else if (rightStickValue <= threshold)
                {   rightThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Right); }
                else
                {   rightThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Rightmost); }
            }
            else
            {
                if (KeyboardInputManager.isPressed(Keyboard.Key.A))
                { leftThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Leftmost); }
                else if (KeyboardInputManager.isPressed(Keyboard.Key.D))
                { leftThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Rightmost); }
                else
                { leftThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Mid); }

                if (KeyboardInputManager.isPressed(Keyboard.Key.Left))
                { rightThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Leftmost); }
                else if (KeyboardInputManager.isPressed(Keyboard.Key.Right))
                { rightThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Rightmost); }
                else
                { rightThumbSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Mid); }
            }

            win.Draw(leftThumbSprite);
            win.Draw(rightThumbSprite);
        }
    }
}