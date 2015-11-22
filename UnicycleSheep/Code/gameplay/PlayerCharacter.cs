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

        Body head;
        Vector2 rotationHead;

        //movement input and constant
        private float wheelToSheepRot = 0.0f; //basicly the rotation of the unicycle's frame
        protected float rotation = 0;
        private float wantsToBalance = 0;
        private float RotationFakt = 1f;

        private float maxJump = 42f;

        bool jump = false;
        float jumpStrength = 0.0f;

        //state vars
        bool isOnGround;

        uint controllerIndex;

        public PlayerCharacter(World _world, Vector2 _position, uint controllerIndex)
            :base(_world, _position)
        {
            this.controllerIndex = controllerIndex;

            this.angVelocity = 0;
            //build the unicycle
            CircleDef circleDef = new CircleDef();
            circleDef.Radius = 1;
            circleDef.Density = 1f;
            circleDef.Friction = 1.0f;
            circleDef.Restitution = 0.0f;
            circleDef.LocalPosition.Set(0, 0);

            Box2DX.Collision.Shape s = this.body.CreateShape(circleDef);
            body.SetMassFromShapes();
            body.SetUserData(this); // link body and this to register collisions in this
            
            
            //build the head and connect with the wheel
            BodyDef bodydef = new BodyDef();
            bodydef.Position = _position + new Vector2(0.0f, 4.0f);
            bodydef.Angle = -1f;

            head = _world.CreateBody(bodydef);

            rotationHead = _position;

            PolygonDef Boxdef = new PolygonDef();
            Boxdef.SetAsBox(1, 3);
            Boxdef.Density = 0.25f;
            Boxdef.Friction = 0.0f;
            Box2DX.Collision.Shape s2 = this.head.CreateShape(Boxdef);
            this.head.SetMassFromShapes();

            //Jointshit
            DistanceJointDef jointDefKW = new DistanceJointDef();
            jointDefKW.Body1 = head;
            jointDefKW.Body2 = body;
            jointDefKW.CollideConnected = false;
            jointDefKW.Length = 4f;

            //       jointDef.Type = JointType.DistanceJoint;

            _world.CreateJoint(jointDefKW);

            Texture wheelTexture = AssetManager.getTexture(AssetManager.TextureName.ShoopWheel);
            wheelSprite = new AnimatedSprite(wheelTexture, 1.0f, 1, (Vector2)wheelTexture.Size);
            wheelSprite.Scale = Vector2.Zero.toScreenCoord() - (Vector2.One / (Vector2)wheelTexture.Size * 2F * circleDef.Radius).toScreenCoord();
            wheelSprite.Origin = ((Vector2)wheelSprite.spriteSize) / 2F;

            sheepSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.Shoop));
            sheepSprite.Scale = new Vector2(0.2f, 0.2f);
            if(_position.X > Constants.worldSizeX / 2F)
                sheepSprite.Scale = new Vector2(-0.2f, 0.2f);
            sheepSprite.Origin = ((Vector2)sheepSprite.Texture.Size) / 2F;
        }

        public void KeyboardInput()
        {
            bool jumpButtonIsPressed;

            if(GamePadInputManager.isConnected(controllerIndex))
            {
                rotation = -GamePadInputManager.getLeftStick(controllerIndex).X;
                wantsToBalance = -GamePadInputManager.getRightStick(controllerIndex).X;

                jumpButtonIsPressed = GamePadInputManager.isPressed(GamePadButton.RB, controllerIndex);
            }
            else
            {
                rotation = 0F;
                if (KeyboardInputManager.isPressed(Keyboard.Key.A))
                    rotation += 1F;
                if (KeyboardInputManager.isPressed(Keyboard.Key.D))
                    rotation += -1F;

                jumpButtonIsPressed = KeyboardInputManager.isPressed(Keyboard.Key.Space);
            }

            if (jumpButtonIsPressed)
            {
                if (jumpStrength < maxJump)
                    jumpStrength += 0.8f;
                jump = false;
            }
            else if (jumpStrength > 0) jump = true;  

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
                head.ApplyImpulse(new Vector2(0, jumpStrength * 0.01f), body.GetWorldCenter());
                jump = false;
                jumpStrength = 0f;
            }

            if(wantsToBalance != 0)
            {
                Vector2 targetVel = (Vector2)head.GetWorldCenter() - location;//optPos - headPos;
                targetVel = wantsToBalance == 1 ? new Vector2(targetVel.X, -targetVel.Y) : new Vector2(-targetVel.X, targetVel.Y);
                targetVel.normalize();

                head.ApplyImpulse(targetVel, head.GetWorldCenter());
            }
            if(isOnGround)
            {
                //some auto correction to make it easier to not fall over
                Vector2 headPos = head.GetWorldCenter();
                Vector2 optPos = location + new Vector2(0.0f, 4.0f);
                Vector2 currentVel = head.GetLinearVelocity();

                Vector2 targetDir = optPos - headPos;
                Vector2 targetVel = (Vector2)head.GetWorldCenter() - location;//optPos - headPos;
                targetVel = targetDir.Y < 0 ? new Vector2(targetVel.X, -targetVel.Y) : new Vector2(-targetVel.X, targetVel.Y);

                float len = targetDir.lengthSqr;
                if (len < 0.05f * 0.05f) return;

                targetVel.normalize();
                float dif = (currentVel.normalize() - targetVel).length;

                head.ApplyImpulse(targetVel , head.GetWorldCenter());//(Vector2)(body.GetLinearVelocity()*/
            }
        }


        public override void update()
        {
            base.update();

            Vector2 radius = (Vector2)head.GetWorldCenter() - location;
            wheelToSheepRot = (float)System.Math.Atan2(radius.X, radius.Y) * Helper.RadianToDegree;
        }


        public override void draw(RenderWindow win, View view)
        {
            Vector2 sheepLoc = head.GetWorldCenter();
            Vector2 radius = (Vector2)head.GetWorldCenter() - location;

            sheepSprite.Position = sheepLoc.toScreenCoord();
            sheepSprite.Rotation = wheelToSheepRot;

            wheelSprite.Position = location.toScreenCoord();
            wheelSprite.Rotation = -body.GetAngle() * Helper.RadianToDegree;

            win.Draw(wheelSprite);
            //draw after to overlap
            win.Draw(sheepSprite);
        }

        // ********************************************************** //

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
