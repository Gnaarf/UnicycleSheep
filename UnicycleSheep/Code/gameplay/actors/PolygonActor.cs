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
    /*
     * A actor that has a top line in shape of a function.
     * */
    class PolygonActor : UnicycleSheep.PhysicsActor
    {
        Body[] triangleBodys; //to build a convex polygon triangles are used
        public PolygonActor(World _world, Vector2 _position, uint _seed)
            : base(_world, _position)
        {   
            int res = (int)Constants.worldSizeX / 5;

            //make sure to have an even number
            if (res % 2 != 0) res++;
            Vec2[] verts = new Vec2[(int)res];
            vertexBuffer = new VertexArray(PrimitiveType.LinesStrip);

            Vector2 posScreen = _position.toScreenCoord();

            //repeatable random sequenze
            Rand rnd = new Rand(_seed);

            verts[0] = new Vec2(0, 10);
            vertexBuffer.Append(new Vertex(((Vector2)verts[0] + _position).toScreenCoord()));
            //create the function
            for (int i = 1; i < res; ++i)
            {
                //Vector2 pos = new Vec2(i * 5, 10 + Rand.IntValue(10));
                Vector2 pos = new Vec2(i * 5, verts[i-1].Y + (int)rnd.next(6) - 3);
                verts[i] = pos;
                vertexBuffer.Append(new Vertex((pos + _position).toScreenCoord()));
            }

            Array.Resize<Body>(ref triangleBodys, res);

            PolygonDef triangleDef = new PolygonDef();
            triangleDef.Density = 0.0f;
            triangleDef.Friction = 1.0f;
            triangleDef.VertexCount = 3;

            BodyDef bodydef = new BodyDef();
            bodydef.Position = _position;
            bodydef.Angle = 0.0f;

            //convert to triangles
            for (int i = 0; i < res - 1; ++i)
            {
                //always 3 points of the function form a triangle
                triangleDef.Vertices[0] = verts[i];
                triangleDef.Vertices[1] = verts[i] - new Vec2(0.0f, 10.0f);
                triangleDef.Vertices[2] = verts[i + 1];//.Y < verts[i+1].Y ? verts[i] : verts[i + 1]

                triangleBodys[i] = _world.CreateBody(bodydef);
                triangleBodys[i].CreateShape(triangleDef);

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
    }
}
