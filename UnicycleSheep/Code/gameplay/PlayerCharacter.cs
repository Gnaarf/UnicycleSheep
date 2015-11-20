using System;
using SFML.Graphics;
using SFML.Window;
using Box2DX;
using Box2DX.Dynamics;
using Box2DX.Collision;
using System.Collections.Generic;

namespace UnicycleSheep
{
    class PlayerCharacter : PhysicsActor//, IContactEvent
    {
        //remove after manager exists
        AnimatedSprite sprite;

        public PlayerCharacter(World _world, Vector2 _position)
            :base(_world, _position)
        {
            PolygonDef polyDef = new PolygonDef();
            polyDef.SetAsBox(1, 1);
            polyDef.Density = 1.0f;

            body.CreateShape(polyDef);
            body.SetMassFromShapes();

            sprite = new AnimatedSprite(new Texture("Assets/textures/pixel.png"), 1.0f, 1, new Vector2i(1, 1));
        }

        public override void draw(RenderWindow win, View view)
        {
            sprite.Position = location.toScreenCoord();
            sprite.Scale = new Vector2f(10, 10);
            sprite.Rotation = body.GetAngle() * Helper.RadianToDegree;

            win.Draw(sprite);
        }
    }
}
