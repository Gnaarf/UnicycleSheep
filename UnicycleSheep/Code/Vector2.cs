using System;
using Box2DX.Common;

public struct Vector2
{
    public float X;
    public float Y;

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }

    //------------------------------------------//
    //                  Casts                   //
    //------------------------------------------//
    // Box2DX-Vectors
    public static implicit operator Vector2(Box2DX.Common.Vec2 v)
    {
        return new Vector2(v.X, v.Y);
    }

    public static implicit operator Box2DX.Common.Vec2(Vector2 v)
    {
        return new Box2DX.Common.Vec2(v.X, v.Y);
    }

    // SFML-Vectors
    public static implicit operator Vector2(SFML.Window.Vector2f v)
    {
        return new Vector2(v.X, v.Y);
    }

    public static implicit operator SFML.Window.Vector2f(Vector2 v)
    {
        return new SFML.Window.Vector2f(v.X, v.Y);
    }

    public static implicit operator Vector2(SFML.Window.Vector2u v)
    {
        return new Vector2(v.X, v.Y);
    }

    public static implicit operator SFML.Window.Vector2u(Vector2 v)
    {
        return new SFML.Window.Vector2u((uint)v.X, (uint)v.Y);
    }

    public static implicit operator Vector2(SFML.Window.Vector2i v)
    {
        return new Vector2(v.X, v.Y);
    }

    public static implicit operator SFML.Window.Vector2i(Vector2 v)
    {
        return new SFML.Window.Vector2i((int)v.X, (int)v.Y);
    }


    //------------------------------------------//
    //                 Constants                //
    //------------------------------------------//
    public static Vector2 Zero { get { return new Vector2(0F, 0F); } }
    public static Vector2 One { get { return new Vector2(1F, 1F); } }
    public static Vector2 Up { get { return new Vector2(0F, -1F); } }
    public static Vector2 Right { get { return new Vector2(1F, 0F); } }
    public static Vector2 Down { get { return new Vector2(0F, 1F); } }
    public static Vector2 Left { get { return new Vector2(-1F, 0F); } }

   
    //------------------------------------------//
    //                Conversions               //
    //------------------------------------------//
    public Vector2 toScreenCoord()
    {
        return new Vector2(X * Constants.worldToScreenRatio, (Constants.worldSizeY - Y) * Constants.worldToScreenRatio);
    }
   /* public Vector2 PixelCoord { get { return this * UnicycleSheep.GameConstants.PIXEL_PER_UNIT; } }
    
    public Vector2 toWorldCoord()
    {
        return this /= GameProject2D.GameConstants.PIXEL_PER_UNIT;
    }
    public Vector2 WorldCoord { get { return this / GameProject2D.GameConstants.PIXEL_PER_UNIT; } }
*/

    //------------------------------------------//
    //             Instance Functions           //
    //------------------------------------------//
    public float length { get { return (float)System.Math.Sqrt(X * X + Y * Y); } }
    public float lengthSqr { get { return X * X + Y * Y; } }

    public Vector2 normalized 
    { 
        get 
        {
            float l = length;
            return this / l; 
        } 
    }

    public Vector2 normalize() 
    {
        float l = length;
        return this /= l; 
    }

    public Vector2 right { get { return new Vector2(Y, -X); } }
    public Vector2 rightNormalized { get { return new Vector2(Y, -X) / length; } }

    //------------------------------------------//
    //           Static Functions               //
    //------------------------------------------//
    /// <summary>linear interpolation by t=[0,1]</summary>
    public static Vector2 lerp(Vector2 from, Vector2 to, float t)
    {
        return (1F - t) * from + t * to;
    }

    public static Vector2 average(params Vector2[] values)
    {
        return sum(values) / (float)values.Length;
    }

    public static Vector2 sum(params Vector2[] values)
    {
        Vector2 sum = new Vector2(0F, 0F);
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }
        return sum;
    }

    public static float distance(Vector2 v1, Vector2 v2)
    {
        return (v1 - v2).length;
    }

    public static float distanceSqr(Vector2 v1, Vector2 v2)
    {
        return (v1 - v2).lengthSqr;
    }

    public static float dot(Vector2 v1, Vector2 v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y;
    }

    //------------------------------------------//
    //           Arithmetic Operators           //
    //------------------------------------------//
    // Addition
    /// <summary>add component-wise</summary>
    public static Vector2 operator +(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
    }

    // Subtraction
    /// <summary>subtract component-wise</summary>
    public static Vector2 operator -(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
    }
    /// <summary>negate every component</summary>
    public static Vector2 operator -(Vector2 v)
    {
        return new Vector2(-v.X, -v.Y);
    }

    // Multiplication
    /// <summary>multiply component-wise</summary>
    public static Vector2 operator *(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
    }
    /// <summary>multiply both components with factor</summary>
    public static Vector2 operator *(float f, Vector2 v)
    {
        return new Vector2(f * v.X, f * v.Y);
    }
    /// <summary>multiply both components with factor</summary>
    public static Vector2 operator *(Vector2 v, float f)
    {
        return new Vector2(f * v.X, f * v.Y);
    }

    // Division
    /// <summary>divide component-wise</summary>
    public static Vector2 operator /(Vector2 v1, Vector2 v2)
    {
        if (v2.X == 0F || v2.Y == 0F) { throw new Exception("Devide by Zero"); }
        return new Vector2(v1.X / v2.X, v1.Y / v2.Y);
    }
    /// <summary>divide both components by factor</summary>
    public static Vector2 operator /(Vector2 v, float f)
    {
        if (f == 0F) { throw new Exception("Devide by Zero"); }
        return new Vector2(v.X / f, v.Y / f);
    }

    // Equality
    /// <summary>check component-wise</summary>
    public static bool operator ==(Vector2 v1, Vector2 v2)
    {
        return (v1.X == v2.X) && (v1.Y == v2.Y);
    }
    /// <summary>check component-wise</summary>
    public static bool operator !=(Vector2 v1, Vector2 v2)
    {
        return (v1.X != v2.X) || (v1.Y != v2.Y);
    }
}

