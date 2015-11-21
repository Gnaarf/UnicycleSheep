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

        //movement input and constant
        protected float rotation = 0;
        private float RotationFakt = 1f;

        private float maxJump = 42f;

        bool jump = false;
        float jumpStrength = 0.0f;

        //state vars
        bool isOnGround;

        static uint count = 0;
        uint index;

        public PlayerCharacter(World _world, Vector2 _position)
            :base(_world, _position)
        {
            index = ++count;

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
            bodydef.Position = _position + new Vector2(0.0f, 4.0f);
            bodydef.Angle = 0.0f;

            head = _world.CreateBody(bodydef);
            head.CreateShape(circleDef);
            head.SetMassFromShapes();

            DistanceJointDef jointDef = new DistanceJointDef();
            jointDef.Body1 = head;
            jointDef.Body2 = body;
            jointDef.CollideConnected = false;
            jointDef.Length = 4f;
     //       jointDef.Type = JointType.DistanceJoint;

            _world.CreateJoint(jointDef);

            Texture wheelTexture = AssetManager.getTexture(AssetManager.TextureName.ShoopWheel);
            wheelSprite = new AnimatedSprite(wheelTexture, 1.0f, 1, (Vector2)wheelTexture.Size);
            wheelSprite.Scale = Vector2.Zero.toScreenCoord() - (Vector2.One / (Vector2)wheelTexture.Size * 2F * circleDef.Radius).toScreenCoord();
            wheelSprite.Origin = ((Vector2)wheelSprite.spriteSize) / 2F;

            sheepSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.Shoop));
            sheepSprite.Scale = new Vector2(0.2f, 0.2f);
            sheepSprite.Origin = ((Vector2)sheepSprite.Texture.Size) / 2F;
        }

        public void KeyboardInput()
        {
            bool jumpButtonIsPressed;

            if(GamePadInputManager.isConnected(index))
            {
                rotation = -GamePadInputManager.getLeftStick(index).X;

                jumpButtonIsPressed = GamePadInputManager.isPressed(GamePadButton.RB, index);
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

                jump = false;
                jumpStrength = 0f;
            }
        }

        public override void draw(RenderWindow win, View view)
        {
            Vector2 sheepLoc = head.GetWorldCenter();
            Vector2 radius = (Vector2)head.GetWorldCenter() - location;

            sheepSprite.Position = sheepLoc.toScreenCoord();
            sheepSprite.Rotation =  (float)System.Math.Atan2(radius.X, radius.Y) * Helper.RadianToDegree;

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
