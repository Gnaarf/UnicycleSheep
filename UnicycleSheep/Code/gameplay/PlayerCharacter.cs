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
        protected float rotation;
        private float RotationFakt = 3f;

        public PlayerCharacter(World _world, Vector2 _position)
            :base(_world, _position)
        {
            this.angVelocity = 0;
            CircleDef circleDef = new CircleDef();
            circleDef.Radius = 1;
            circleDef.Density = 1.0f;
            circleDef.Friction = 1.0f;
            circleDef.LocalPosition.Set(0, 0);

            body.CreateShape(circleDef);
            body.SetMassFromShapes();

            sprite = new AnimatedSprite(new Texture("Assets/textures/pixel.png"), 1.0f, 1, new Vector2i(1, 1));
            sprite.Origin = ((Vector2)sprite.spriteSize) / 2F;
        }

        public void KeyboardInput()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                rotation = 1;
            else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                rotation = -1;
        }
        public void Move()
        {
            if((this.angVelocity < 100) && this.rotation == 1)
                this.angVelocity += RotationFakt;
            else if ((this.angVelocity > -100) && this.rotation == -1)
                this.angVelocity -= RotationFakt;
        }

        public override void draw(RenderWindow win, View view)
        {
            sprite.Position = new Vector2 (location.toScreenCoord().X,location.toScreenCoord().Y);
            sprite.Scale = new Vector2f(10, 10);
            sprite.Rotation = -body.GetAngle() * Helper.RadianToDegree;

            win.Draw(sprite);
        }
    }
}
