using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace UnicycleSheep
{
    class Helper
    {
        public static float RadianToDegree = 180F / (float)Math.PI;
        public static float DegreeToRadian = (float)Math.PI / 180F;

        public static float PI = (float)Math.PI;

        public static float Lerp(float from, float to, float t)
        {
            return (1F - t) * from + t * to;
        }
        public static Color Lerp(Color from, Color to, float t)
        {
            Color res;
            res.R = (byte)((1F - t) * from.R + t * to.R);
            res.G = (byte)((1F - t) * from.G + t * to.G);
            res.B = (byte)((1F - t) * from.B + t * to.B);
            res.A = (byte)((1F - t) * from.A + t * to.A); 
            return res;
        }
    }
}