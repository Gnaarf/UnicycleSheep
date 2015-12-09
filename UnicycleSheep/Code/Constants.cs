
    public static class Constants
    {
        public const int windowSizeX = 800;
        public const int windowSizeY = 600;
        public const float screenRatio = (float)windowSizeY / windowSizeX;

        public const float worldToScreenRatio = Constants.windowSizeX / Constants.worldSizeX;

        public const float worldSizeX = 80.0f;
        public const float worldSizeY = worldSizeX * screenRatio;

        // gameplay
        public const float maxJumpStrength = 84f;
    }
