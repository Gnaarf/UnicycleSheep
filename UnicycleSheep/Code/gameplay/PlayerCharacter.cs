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
            sprite = new AnimatedSprite(new Texture("Assets/textures/pixel.png"), 1.0f, 1, new Vector2i(1, 1));
        }

        public override void draw(RenderWindow win, View view)
        {
            sprite.Position = location.toPixelCoord();
            sprite.Scale = new Vector2f(10, 10);
            sprite.Rotation = body.GetAngle() * Helper.RadianToDegree;

            win.Draw(sprite);
        }
    }
}
