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
        AnimatedSprite wheelSprite;
        protected float rotation;
        private float RotationFakt;

        public PlayerCharacter(World _world, Vector2 _position)
            :base(_world, _position)
        {
            this.angVelocity = 0;
            CircleDef circleDef = new CircleDef();
            circleDef.Radius = 1;
            circleDef.Density = 1.0f;
            circleDef.Friction = 1.0f;
            circleDef.LocalPosition.Set(0, 0);

            Box2DX.Collision.Shape s = body.CreateShape(circleDef);
            body.SetMassFromShapes();

            Texture wheelTexture = AssetManager.getTexture(AssetManager.TextureName.ShoopWheel);
            wheelSprite = new AnimatedSprite(wheelTexture, 1.0f, 1, (Vector2)wheelTexture.Size);
            wheelSprite.Scale = Vector2.Zero.toScreenCoord() - (Vector2.One / (Vector2)wheelTexture.Size * 2F * circleDef.Radius).toScreenCoord();
            wheelSprite.Origin = ((Vector2)wheelSprite.spriteSize) / 2F;
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
                this.angVelocity += 3f;
            else if ((this.angVelocity > -100) && this.rotation == -1)
                this.angVelocity -= 3f;
        }

        public override void draw(RenderWindow win, View view)
        {
            wheelSprite.Position = new Vector2 (location.toScreenCoord().X,location.toScreenCoord().Y);
            wheelSprite.Rotation = -body.GetAngle() * Helper.RadianToDegree;

            win.Draw(wheelSprite);
        }
    }
}
