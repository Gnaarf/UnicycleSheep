using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using UnicycleSheep;

namespace DekoElements
{
    class RemoteControllHand
    {
        Sprite baseSprite;
        Sprite flagSprite;
        Sprite flagColorablePartSprite;
        Sprite leftThumbSprite;
        Sprite rightThumbSprite;
        Sprite leftFingerSprite;
        Sprite rightFingerSprite;

        Sprite jumpbarSprite;

        Color playerColor;

        bool mirrored { get { return baseSprite.Scale.X < 0F; } }

        PlayerCharacter player;

        public RemoteControllHand(PlayerCharacter player)
        {
            this.player = player;

            playerColor = player.playerIndex == 0 ? Color.Black : Color.Magenta;

            Vector2 pos = player.location.toScreenCoord();
            pos.X = (pos.X - Constants.windowSizeX * 0.5F) * 0.8F + Constants.windowSizeX * 0.5F;
            pos.Y = 500;
            Vector2 scale = Vector2.One * 0.5F;
            if (pos.X > Constants.windowSizeX * 0.5F)
                scale.X *= -1F;

            baseSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.Hand));
            baseSprite.Origin = ((Vector2)baseSprite.Texture.Size) * 0.5F;
            baseSprite.Scale = scale;
            baseSprite.Position = pos;

            // Player 0 red, Player 1 green
            flagSprite = new Sprite(AssetManager.getTexture(player.playerIndex == 0 ? AssetManager.TextureName.RemoteFlagRed : AssetManager.TextureName.RemoteFlagGreen));
            flagSprite.Origin = ((Vector2)flagSprite.Texture.Size) * 0.5F;
            flagSprite.Scale = scale;
            flagSprite.Position = pos;

            flagColorablePartSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.RemoteFlagWhitePart));
            flagColorablePartSprite.Origin = ((Vector2)flagSprite.Texture.Size) * 0.5F;
            flagColorablePartSprite.Scale = scale;
            flagColorablePartSprite.Position = pos;
            flagColorablePartSprite.Color = playerColor;

            leftThumbSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.LeftThumb_Mid));
            leftThumbSprite.Origin = ((Vector2)leftThumbSprite.Texture.Size) * 0.5F;
            leftThumbSprite.Scale = scale;
            leftThumbSprite.Position = pos;

            rightThumbSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.RightThumb_Mid));
            rightThumbSprite.Origin = ((Vector2)rightThumbSprite.Texture.Size) * 0.5F;
            rightThumbSprite.Scale = scale;
            rightThumbSprite.Position = pos;


            leftFingerSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.LeftFinger_Up));
            leftFingerSprite.Origin = ((Vector2)leftThumbSprite.Texture.Size) * 0.5F;
            leftFingerSprite.Scale = scale;
            leftFingerSprite.Position = pos;

            rightFingerSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.RightFinger_Down));
            rightFingerSprite.Origin = ((Vector2)rightThumbSprite.Texture.Size) * 0.5F;
            rightFingerSprite.Scale = scale;
            rightFingerSprite.Position = pos;

            jumpbarSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.Jumpbar1));
            jumpbarSprite.Origin = ((Vector2)jumpbarSprite.Texture.Size) * 0.5F;
            jumpbarSprite.Scale = scale;
            jumpbarSprite.Position = pos;
        }

        public void draw(RenderWindow win)
        {
            float totalTime = (float)Program.gameTime.TotalTime.TotalSeconds;
            float blinkTime = 3F; 
            Color fromColor = new Color(255, 255, 255);
            float toColorT = Helper.LerpClamp(0.2F, 0.5F, totalTime / 15F);
            Color toColor = Helper.Lerp(playerColor, Color.White, toColorT);

            float t;
            if (totalTime % blinkTime < blinkTime * 0.5F)
            {
                t = (totalTime % blinkTime) / (blinkTime * 0.5F);
            }
            else
            {
                t = 1F - ((totalTime % blinkTime) / (blinkTime * 0.5F) - 1F);
            }
            leftFingerSprite.Color = Helper.Lerp(fromColor, toColor, t);
            rightFingerSprite.Color = Helper.Lerp(fromColor, toColor, t);
            leftThumbSprite.Color = Helper.Lerp(fromColor, toColor, t);
            rightThumbSprite.Color = Helper.Lerp(fromColor, toColor, t);

            if (GamePadInputManager.isConnected(player.controllerIndex))
            {
                float threshold = 0.8F;

                float leftStickValue = GamePadInputManager.getLeftStick(player.controllerIndex).X;
                if (leftStickValue < -threshold)          { setLeftThumb(Horizontal.Leftmost); }
                else if (leftStickValue < -float.Epsilon) { setLeftThumb(Horizontal.Left); }
                else if (leftStickValue <= float.Epsilon) { setLeftThumb(Horizontal.Mid); }
                else if (leftStickValue <= threshold)     { setLeftThumb(Horizontal.Right); }
                else                                      { setLeftThumb(Horizontal.Rightmost); }

                float rightStickValue = GamePadInputManager.getRightStick(player.controllerIndex).X;
                if (rightStickValue < -threshold)          { setRightThumb(Horizontal.Leftmost); }
                else if (rightStickValue < -float.Epsilon) { setRightThumb(Horizontal.Left); }
                else if (rightStickValue <= float.Epsilon) { setRightThumb(Horizontal.Mid); }
                else if (rightStickValue <= threshold)     { setRightThumb(Horizontal.Right); }
                else { setRightThumb(Horizontal.Rightmost); }

                if (GamePadInputManager.isPressed(GamePadButton.LT, player.controllerIndex))
                { setLeftFinger(Vertical.Down); }
                else
                { setLeftFinger(Vertical.Up); }

                if (GamePadInputManager.isPressed(GamePadButton.RT, player.controllerIndex))
                { setRightFinger(Vertical.Down); }
                else
                { setRightFinger(Vertical.Up); }
            }
            else
            {
                if (KeyboardInputManager.isPressed(Keyboard.Key.S))      { setLeftThumb(Horizontal.Leftmost); }
                else if (KeyboardInputManager.isPressed(Keyboard.Key.W)) { setLeftThumb(Horizontal.Rightmost); }
                else                                                     { setLeftThumb(Horizontal.Mid); }

                if (KeyboardInputManager.isPressed(Keyboard.Key.A))       { setRightThumb(Horizontal.Leftmost); }
                else if (KeyboardInputManager.isPressed(Keyboard.Key.D)) { setRightThumb(Horizontal.Rightmost); }
                else                                                         { setRightThumb(Horizontal.Mid); }

                if (KeyboardInputManager.isPressed(Keyboard.Key.Space))
                {
                    setRightFinger(Vertical.Down);
                    setLeftFinger(Vertical.Down);
                }
                else
                {
                    setRightFinger(Vertical.Up);
                    setLeftFinger(Vertical.Up);
                }
            }

            win.Draw(leftFingerSprite);
            win.Draw(rightFingerSprite);

            win.Draw(flagSprite);
            win.Draw(flagColorablePartSprite);
            win.Draw(baseSprite);

            win.Draw(leftThumbSprite);
            win.Draw(rightThumbSprite);

            drawJumpBar(win);
        }

        public void drawJumpBar(RenderWindow win)
        {
            float jumpLoad = player.jumpLoadPercentage;
            if (jumpLoad < float.Epsilon) { return; }
            else if (jumpLoad < 0.05F) { jumpbarSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Jumpbar1); }
            else if (jumpLoad < 0.15F) { jumpbarSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Jumpbar2); }
            else if (jumpLoad < 0.3F) { jumpbarSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Jumpbar3); }
            else if (jumpLoad < 0.5F) { jumpbarSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Jumpbar4); }
            else if (jumpLoad < 0.8F) { jumpbarSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Jumpbar5); }
            else { jumpbarSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.Jumpbar6); }

            win.Draw(jumpbarSprite);
        }

        void setRightFinger(Vertical v)
        {
            switch (v)
            {
                case Vertical.Up:
                    rightFingerSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.LeftFinger_Up : AssetManager.TextureName.RightFinger_Up);
                    break;

                case Vertical.Down:
                    rightFingerSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.LeftFinger_Down : AssetManager.TextureName.RightFinger_Down);
                    break;
            }
        }

        void setLeftFinger(Vertical v)
        {
            switch (v)
            {
                case Vertical.Up:
                    leftFingerSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.RightFinger_Up : AssetManager.TextureName.LeftFinger_Up);
                    break;

                case Vertical.Down:
                    leftFingerSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.RightFinger_Down : AssetManager.TextureName.LeftFinger_Down);
                    break;
            }
        }

        void setRightThumb(Horizontal h)
        {
            switch (h)
            {
                case Horizontal.Leftmost:
                    rightThumbSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.LeftThumb_Rightmost : AssetManager.TextureName.RightThumb_Leftmost);
                    break;

                case Horizontal.Left:
                    rightThumbSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.LeftThumb_Right : AssetManager.TextureName.RightThumb_Left);
                    break;

                case Horizontal.Mid:
                    rightThumbSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.LeftThumb_Mid : AssetManager.TextureName.RightThumb_Mid);
                    break;

                case Horizontal.Right:
                    rightThumbSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.LeftThumb_Left : AssetManager.TextureName.RightThumb_Right);
                    break;

                case Horizontal.Rightmost:
                    rightThumbSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.LeftThumb_Leftmost : AssetManager.TextureName.RightThumb_Rightmost);
                    break;
            }
        }

        void setLeftThumb(Horizontal h)
        {
            switch (h)
            {
                case Horizontal.Leftmost:
                    leftThumbSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.RightThumb_Rightmost : AssetManager.TextureName.LeftThumb_Leftmost);
                    break;

                case Horizontal.Left:
                    leftThumbSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.RightThumb_Right : AssetManager.TextureName.LeftThumb_Left);
                    break;

                case Horizontal.Mid:
                    leftThumbSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.RightThumb_Mid : AssetManager.TextureName.LeftThumb_Mid);
                    break;

                case Horizontal.Right:
                    leftThumbSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.RightThumb_Left : AssetManager.TextureName.LeftThumb_Right);
                    break;

                case Horizontal.Rightmost:
                    leftThumbSprite.Texture = AssetManager.getTexture(mirrored ? AssetManager.TextureName.RightThumb_Leftmost : AssetManager.TextureName.LeftThumb_Rightmost);
                    break;
            }
        }

        private enum Vertical
        {
            Up,
            Down
        }

        private enum Horizontal
        {
            Leftmost,
            Left,
            Mid,
            Right,
            Rightmost
        }
    }
}