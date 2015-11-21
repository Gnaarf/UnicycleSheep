using System;
using SFML.Graphics;
using SFML.Window;
using Box2DX;
using Box2DX.Dynamics;
using Box2DX.Collision;
using System.Collections.Generic;
using Box2DX.Common;

namespace Actors
{
    enum FunctionType
    {
        Simple,
        GradientNoise
    }
    /*
     * A actor that has a top line in shape of a function.
     * */
    class PolygonActor : UnicycleSheep.PhysicsActor
    {
        Body[] triangleBodys; //to build a convex polygon triangles are used

        /*
         * @param _funcType Style of the top line that generated
         * @param _res Size of mapunits for one linear segment
         * */
        public PolygonActor(World _world, Vector2 _position, uint _seed, FunctionType _funcType, int _res = 5)
            : base(_world, _position)
        {   
            int lineCount = (int)Constants.worldSizeX / _res;

            //make sure to have an even number
            if (lineCount % 2 != 0) lineCount++;
            Vec2[] verts = new Vec2[(int)lineCount + 1 + 4];
            vertexBuffer = new VertexArray(PrimitiveType.LinesStrip);

            Vector2 posScreen = _position.toScreenCoord();

            //repeatable random sequenze
            Rand rnd = new Rand(_seed);

            verts[0] = new Vec2(0, 6);
            vertexBuffer.Append(new Vertex(((Vector2)verts[0] + _position).toScreenCoord()));
            //create the function
            if (_funcType == FunctionType.Simple)
            {
                for (int i = 1; i <= lineCount; ++i)
                {
                    //Vector2 pos = new Vec2(i * 5, 10 + Rand.IntValue(10));
                    Vector2 pos = new Vec2(i * _res, System.Math.Max((verts[i - 1].Y + (int)rnd.next(6) - 3), 0));
                    verts[i] = pos;
                }
            }
            else if(_funcType == FunctionType.GradientNoise)
            {
                for (int i = 0; i <= lineCount; i += 4)
                {
                    //the random points
                 //   verts[i] = new Vec2(i * _res, rnd.next((int)maxHeight));
                    int nextGrad = i + 4;
                    verts[nextGrad] = new Vec2(nextGrad * _res, rnd.next((int)maxHeight));

                    //interpolate between
                    float relativeA = verts[i].Y / maxHeight;
                    float relativeB = verts[i + 4].Y / maxHeight;
                    for (int c = i + 1; c < nextGrad; ++c)
                    {
                        verts[c] = new Vec2(c * _res, maxHeight * interpolateCos(relativeA, relativeB, (float)(c - i) / 4));
                    }
                }


            }

            Array.Resize<Body>(ref triangleBodys, lineCount);

            PolygonDef triangleDef = new PolygonDef();
            triangleDef.Density = 0.0f;
            triangleDef.Friction = 1.0f;
            triangleDef.VertexCount = 3;

            BodyDef bodydef = new BodyDef();
            bodydef.Position = _position;
            bodydef.Angle = 0.0f;

            //convert to triangles
            for (int i = 0; i < lineCount; ++i)
            {
                //always 3 points of the function form a triangle
                triangleDef.Vertices[0] = verts[i];
                triangleDef.Vertices[1] = verts[i] - new Vec2(0.0f, 50.0f);
                triangleDef.Vertices[2] = verts[i + 1];//.Y < verts[i+1].Y ? verts[i] : verts[i + 1]

                triangleBodys[i] = _world.CreateBody(bodydef);
                triangleBodys[i].CreateShape(triangleDef);

                vertexBuffer.Append(new Vertex(((Vector2)verts[i+1] + _position).toScreenCoord()));
            }
        }

        public override void draw(RenderWindow win, View view)
        {
            win.Draw(vertexBuffer);
        }

  /*      void updateVertexBuf()
        {
            vertexBuffer.Resize(0);
            for(int i = 0; i < vertCount - 2; ++i)
            {

            }
        }*/

        VertexArray vertexBuffer;

        private const float maxHeight = 14f;
        private static float interpolateCos(float a, float b, float x)
        {
            float ft = x * 3.1415927f;
            float f = (1f - (float)System.Math.Cos(ft)) * 0.5f;

            return a * (1 - f) + b * f;
        }
    }
}
