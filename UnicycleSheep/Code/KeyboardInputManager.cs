using SFML.Window;
using System.Collections.Generic;

namespace UnicycleSheep
{
    public class KeyboardInputManager
    {
        static bool isInitialized = false;

        private static int keyCount;
        private static bool[] previousKeyIsPressed;
        private static bool[] currentKeyIsPressed;

        private static void initialize()
        {
            keyCount = (int)Keyboard.Key.KeyCount;
            previousKeyIsPressed = new bool[keyCount];
            currentKeyIsPressed = new bool[keyCount];

            isInitialized = true;
        }

        public static void update()
        {
            if (!isInitialized) { initialize(); }

            for (int i = 0; i < keyCount; ++i)
            {
                previousKeyIsPressed[i] = currentKeyIsPressed[i];
                currentKeyIsPressed[i] = Keyboard.IsKeyPressed((Keyboard.Key)i);
            }
        }

        public static bool isPressed(Keyboard.Key key)
        {
            return currentKeyIsPressed[(int)key];
        }

        /// <summary>returns true, if the key is pressed this frame and was not pressed previous frame</summary>
        /// <param name="key">Key to be evaluated</param>
        /// <returns>returns true, if the key is pressed this frame and was not pressed previous frame</returns>
        public static bool downward(Keyboard.Key key)
        {
            if (!isInitialized) { initialize(); }

            return !previousKeyIsPressed[(int)key] && currentKeyIsPressed[(int)key];
        }

        /// <summary>returns true, if the key is not pressed this frame and was pressed previous frame</summary>
        /// <param name="key">Key to be evaluated</param>
        /// <returns>returns true, if the key is not pressed this frame and was pressed previous frame</returns>
        public static bool upward(Keyboard.Key key)
        {
            if (!isInitialized) { initialize(); }

            return previousKeyIsPressed[(int)key] && !currentKeyIsPressed[(int)key];
        }
    }
}
