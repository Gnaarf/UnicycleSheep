using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace UnicycleSheep
{
    public class AnimatedSprite : Sprite
    {
        public float secondsPerFrame { get; private set; }
        public Vector2i spriteSize { get; private set; }
        public int frameCount { get; private set; }
        Vector2i upperLeftCorner;
        
        float? startSecond;

        public AnimatedSprite(Texture spriteSheet, float secondsPerFrame, int frameCount, Vector2i spriteSize)
            : this(spriteSheet, secondsPerFrame, frameCount, spriteSize, new Vector2i(0, 0))
        {
        }

        public AnimatedSprite(Texture spriteSheet, float secondsPerFrame, int frameCount, Vector2i spriteSize, Vector2i upperLeftCorner)
            : base(spriteSheet)
        {
            this.secondsPerFrame = secondsPerFrame;
            this.frameCount = frameCount;
            this.spriteSize = spriteSize;
            this.upperLeftCorner = upperLeftCorner;
            startSecond = 0F;
        }

        /// <summary>start or restart the animation</summary>
        public void restartAnimation(GameTime currentTime)
        {
            startSecond = (float)currentTime.TotalTime.TotalSeconds;
        }

        /// <summary>start or restart the animation</summary>
        public void stopAnimation()
        {
            startSecond = null;
        }

        public Sprite updateFrame(GameTime currentTime)
        {
            int currentFrame = 0;

            if(startSecond.HasValue)
            {
                float passedSeconds = 0F;
                passedSeconds = (float)currentTime.TotalTime.TotalSeconds - startSecond.Value;
                passedSeconds /= ((float)frameCount * secondsPerFrame);
                passedSeconds -= (float)Math.Floor(passedSeconds);
                
                currentFrame = (int)(passedSeconds * frameCount);
            }

            TextureRect = new IntRect(upperLeftCorner.X + (currentFrame * spriteSize.X), upperLeftCorner.Y, spriteSize.X, spriteSize.Y);
            return this;

        }
    }
}