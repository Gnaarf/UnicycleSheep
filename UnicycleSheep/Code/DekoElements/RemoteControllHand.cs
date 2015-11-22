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

        public RemoteControllHand(uint index, Vector2 PlayerStartingPos)
        {
            this.index = index;

            Vector2 pos = PlayerStartingPos.toScreenCoord();
            pos.X = (pos.X - Constants.windowSizeX * 0.5F) * 0.8F + Constants.windowSizeX * 0.5F;
            pos.Y = 500;
            Vector2 scale = Vector2.One * 0.5F;
            if (pos.X > Constants.windowSizeX * 0.5F)
                scale.X *= -1F;

            baseSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.Hand1));
            baseSprite.Origin = ((Vector2)baseSprite.Texture.Size) * 0.5F;
            baseSprite.Scale = scale;
            baseSprite.Position = pos;
            leftThumbSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.Hand1_LeftThumb_Mid));
            leftThumbSprite.Origin = ((Vector2)leftThumbSprite.Texture.Size) * 0.5F;
            leftThumbSprite.Scale = scale;
            leftThumbSprite.Position = pos;
            rightThumbSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.Hand1_RightThumb_Mid));
            rightThumbSprite.Origin = ((Vector2)rightThumbSprite.Texture.Size) * 0.5F;
            rightThumbSprite.Scale = scale;
            rightThumbSprite.Position = pos;
        }

        public void draw(RenderWindow win)
        {
            win.Draw(baseSprite);

            float totalTime = (float)Program.gameTime.TotalTime.TotalSeconds;
            float blinkTime = 3F; 
            Color fromColor = new Color(255, 255, 255);
            Color toColor;
            if (totalTime < 15F)
                toColor = Helper.Lerp(new Color(255, 100, 100), new Color(255, 200, 200), totalTime / 30F);
            else
                toColor = new Color(255, 200, 200);

            if (totalTime % blinkTime < blinkTime * 0.5F)
            {
                float t = (totalTime % blinkTime) / (blinkTime * 0.5F);
                leftThumbSprite.Color = Helper.Lerp(fromColor, toColor, t);
                rightThumbSprite.Color = Helper.Lerp(fromColor, toColor, t);
            }
            else
            {
                float t = (totalTime % blinkTime) / (blinkTime * 0.5F) - 1F;
                leftThumbSprite.Color = Helper.Lerp(toColor, fromColor, t);
                rightThumbSprite.Color = Helper.Lerp(toColor, fromColor, t);
            }

            if (GamePadInputManager.isConnected(index))
            {
                float threshold = 0.8F;

                float leftStickValue = GamePadInputManager.getLeftStick(index).X;
                if (leftStickValue < -threshold)
                { leftThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_RightThumb_Rightmost : AssetManager.TextureName.Hand1_LeftThumb_Leftmost); }
                else if (leftStickValue < -float.Epsilon)
                { leftThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_RightThumb_Right : AssetManager.TextureName.Hand1_LeftThumb_Left); }
                else if (leftStickValue <= float.Epsilon)
                { leftThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_RightThumb_Mid : AssetManager.TextureName.Hand1_LeftThumb_Mid); }
                else if (leftStickValue <= threshold)
                { leftThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_RightThumb_Left : AssetManager.TextureName.Hand1_LeftThumb_Right); }
                else
                { leftThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_RightThumb_Leftmost : AssetManager.TextureName.Hand1_LeftThumb_Rightmost); }
                
                float rightStickValue = GamePadInputManager.getRightStick(index).X;
                if (rightStickValue < -threshold)
                { rightThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_LeftThumb_Rightmost : AssetManager.TextureName.Hand1_RightThumb_Leftmost); }
                else if (rightStickValue < -float.Epsilon)
                { rightThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_LeftThumb_Right : AssetManager.TextureName.Hand1_RightThumb_Left); }
                else if (rightStickValue <= float.Epsilon)
                { rightThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_LeftThumb_Mid : AssetManager.TextureName.Hand1_RightThumb_Mid); }
                else if (rightStickValue <= threshold)
                { rightThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_LeftThumb_Left : AssetManager.TextureName.Hand1_RightThumb_Right); }
                else
                { rightThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_LeftThumb_Leftmost : AssetManager.TextureName.Hand1_RightThumb_Rightmost); }
            }
            else
            {
                if (KeyboardInputManager.isPressed(Keyboard.Key.A))
                { leftThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_RightThumb_Leftmost : AssetManager.TextureName.Hand1_LeftThumb_Leftmost); }
                else if (KeyboardInputManager.isPressed(Keyboard.Key.D))
                { leftThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_RightThumb_Rightmost : AssetManager.TextureName.Hand1_LeftThumb_Rightmost); }
                else
                { leftThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_RightThumb_Mid : AssetManager.TextureName.Hand1_LeftThumb_Mid); }

                if (KeyboardInputManager.isPressed(Keyboard.Key.Left))
                { rightThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_LeftThumb_Leftmost : AssetManager.TextureName.Hand1_RightThumb_Leftmost); }
                else if (KeyboardInputManager.isPressed(Keyboard.Key.Right))
                { rightThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_LeftThumb_Rightmost : AssetManager.TextureName.Hand1_RightThumb_Rightmost); }
                else
                { rightThumbSprite.Texture = AssetManager.getTexture(baseSprite.Scale.X < 0F ? AssetManager.TextureName.Hand1_LeftThumb_Mid : AssetManager.TextureName.Hand1_RightThumb_Mid); }
            }

            win.Draw(leftThumbSprite);
            win.Draw(rightThumbSprite);
        }
    }
}