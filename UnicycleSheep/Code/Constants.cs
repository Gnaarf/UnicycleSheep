
    public static class Constants
    {
        public const int windowSizeY = 1080; //600
		public const int windowSizeX = windowSizeY * 4 / 3; //800

		public const float windowScaleFatctor = windowSizeY / 600f;

        public const float screenRatio = (float)windowSizeY / windowSizeX;

        public const float worldToScreenRatio = Constants.windowSizeX / Constants.worldSizeX;

        public const float worldSizeX = 80.0f;
        public const float worldSizeY = worldSizeX * screenRatio;

        // gameplay
        public const float maxJumpStrength = 84f;
		public const float gameSpeedFactor = 1.5f;
    }
