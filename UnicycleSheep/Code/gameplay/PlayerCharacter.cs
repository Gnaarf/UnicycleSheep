using System;
using SFML.Graphics;
using SFML.Window;
using Box2DX;
using Box2DX.Dynamics;
using Box2DX.Collision;
using System.Collections.Generic;
using Physics;

namespace UnicycleSheep
{
    class PlayerCharacter : PhysicsActor, IContactEvent
    {
        //remove after manager exists
        AnimatedSprite wheelSprite;
        Sprite sheepSprite;
        Sprite testsprite;

        Body chest;

        //movement input and constant
        private float wheelToSheepRot = 0.0f; //basicly the rotation of the unicycle's frame
        protected float rotation = 0;
        private float RotationFakt = 1f;

        private float maxJump = 42f;

        bool jump = false;
        float jumpStrength = 0.0f;

        //state vars
        bool isOnGround;

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
            body.SetUserData(this); // link body and this to register collisions in this

            //build the head and connect with the wheel
            BodyDef bodydef = new BodyDef();
            bodydef.Angle = 0.0f;

            bodydef.Position = _position + new Vector2(0.0f, 4f);
            chest = _world.CreateBody(bodydef);
            circleDef.Density = 0.2f;
            chest.CreateShape(circleDef);
            chest.SetMassFromShapes();

            DistanceJointDef jointDef = new DistanceJointDef();
            jointDef.Body1 = chest;
            jointDef.Body2 = body;
            jointDef.CollideConnected = false;
            jointDef.Length = 4f;
            _world.CreateJoint(jointDef);

            Texture wheelTexture = AssetManager.getTexture(AssetManager.TextureName.ShoopWheel);
            wheelSprite = new AnimatedSprite(wheelTexture, 1.0f, 1, (Vector2)wheelTexture.Size);
            wheelSprite.Scale = Vector2.Zero.toScreenCoord() - (Vector2.One / (Vector2)wheelTexture.Size * 2F * circleDef.Radius).toScreenCoord();
            wheelSprite.Origin = ((Vector2)wheelSprite.spriteSize) / 2F;

            sheepSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.Shoop));
            sheepSprite.Scale = new Vector2(0.2f, 0.2f);
            sheepSprite.Origin = ((Vector2)sheepSprite.Texture.Size) / 2F;

            testsprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.WhitePixel));
            testsprite.Origin = ((Vector2)testsprite.Texture.Size) / 2F;
        }

        public void KeyboardInput()
        {
            if (KeyboardInputManager.isPressed(Keyboard.Key.A))
                rotation = 1;
            else if (KeyboardInputManager.isPressed(Keyboard.Key.D))
                rotation = -1;
            else //stop accelerating
                rotation = 0f;

            if (KeyboardInputManager.isPressed(Keyboard.Key.Space))
            {
                if (jumpStrength < maxJump) jumpStrength += 0.8f;
                jump = false;
            }
            else if(jumpStrength > 0) jump = true;

       /*     bool stabilizePlus = KeyboardInputManager.isPressed(Keyboard.Key.X);
            bool stabilizeMinus = KeyboardInputManager.isPressed(Keyboard.Key.L);
            if (stabilizePlus || stabilizeMinus)
            {
                Vector2 targetVel = (Vector2)chest.GetWorldCenter() - location;//optPos - headPos;
                targetVel = stabilizePlus ? new Vector2(targetVel.X, -targetVel.Y) : new Vector2(-targetVel.X, targetVel.Y); 
             //   targetVel.normalize();

                chest.ApplyImpulse(targetVel, chest.GetWorldCenter());
            }*/
        }
        public void Move()
        {
            if((this.angVelocity < 100) && this.rotation == 1)
                this.angVelocity += RotationFakt;
            else if ((this.angVelocity > -100) && this.rotation == -1)
                this.angVelocity -= RotationFakt;

            if (jump && /*isOnGround &&*/ jumpStrength > 0f)
            {
                body.ApplyImpulse(new Vector2(0, jumpStrength), body.GetWorldCenter());

                jump = false;
                jumpStrength = 0f;
            }

           if (isOnGround)
            {
                //some auto correction to make it easier to not fall over
                Vector2 headPos = chest.GetWorldCenter();
                Vector2 optPos = location + new Vector2(0.0f, 4.0f);
                Vector2 currentVel = chest.GetLinearVelocity();

                Vector2 targetDir = optPos - headPos;
                Vector2 targetVel = (Vector2)chest.GetWorldCenter() - location;//optPos - headPos;
                targetVel = targetDir.Y < 0 ? new Vector2(targetVel.X, -targetVel.Y) : new Vector2(-targetVel.X, targetVel.Y);

                float len = targetDir.lengthSqr;
                if (len < 0.05f * 0.05f)
                {
                //    chest.ApplyImpulse(-chest.GetLinearVelocity(), chest.GetWorldCenter());
                    return;
                }

                targetVel.normalize();
                float dif = (currentVel.normalize() - targetVel).length;

                chest.ApplyImpulse(targetVel * dif* 0.5f, chest.GetWorldCenter());//(Vector2)(body.GetLinearVelocity()
            }
        }

        public override void update()
        {
            base.update();

             Vector2 radius = (Vector2)chest.GetWorldCenter() - location;
             wheelToSheepRot = (float)System.Math.Atan2(radius.X, radius.Y) * Helper.RadianToDegree;
        }

        public override void draw(RenderWindow win, View view)
        {
            Vector2 sheepLoc = chest.GetWorldCenter();
  
            sheepSprite.Position = sheepLoc.toScreenCoord();
            sheepSprite.Rotation = wheelToSheepRot;

            wheelSprite.Position = location.toScreenCoord();
            wheelSprite.Rotation = -body.GetAngle() * Helper.RadianToDegree;

            win.Draw(wheelSprite);
            //draw after to overlap
            win.Draw(sheepSprite);

         //   testsprite.Scale = new Vector2(10, 10);
         //   testsprite.Position = ((Vector2)chest.GetWorldCenter()).toScreenCoord();
            win.Draw(testsprite);
        }

        // ********************************************************** //

        Box2DX.Collision.Shape _lastContact;

        public void OnContact(Box2DX.Collision.Shape _other, ContactPoint _point)
        {
            _lastContact = _other;
            isOnGround = true;
        }

        public void OnContactRemove(Box2DX.Collision.Shape _other, ContactPoint _point)
        {
            //only when the tile is left which was just hit
            if (_lastContact == _other)
                isOnGround = false;
        }

    }
}
