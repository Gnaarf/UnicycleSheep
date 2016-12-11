
    public static class Constants
    {
		// all screen depended constants are now initialized in Program.cs

        public static int windowSizeY; //600
		public static int windowSizeX; //800

		public static float windowScaleFactor = windowSizeY / 600f;

        public static float screenRatio = (float)windowSizeY / windowSizeX;

        public static float worldToScreenRatio;

        public const float worldSizeX = 80.0f;
        public static float worldSizeY;

        // gameplay
        public const float maxJumpStrength = 84f;
		public const float gameSpeedFactor = 1.2f;
    }
