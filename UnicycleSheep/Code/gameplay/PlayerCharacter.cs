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
            sprite = new AnimatedSprite(new Texture("Assets/schwert2.png"), 1.0f, 1, new Vector2i(256, 128));
        }

        public override void draw(RenderWindow win, View view)
        {
            sprite.Position = new Vector2f(40, 32);
            sprite.Scale = new Vector2f(1, 1);
            sprite.Rotation = body.GetAngle() * Helper.RadianToDegree;

            win.Draw(sprite);
        }
    }
}
