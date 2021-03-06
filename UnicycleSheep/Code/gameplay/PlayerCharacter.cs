﻿using System;
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
        public static readonly SFML.Graphics.Color[] Colors = new SFML.Graphics.Color[] 
        {
            new SFML.Graphics.Color(206,   0,   0), // Red
            new SFML.Graphics.Color( 12, 183,   0), // Green
            new SFML.Graphics.Color(  0,  34, 183), // Blue
            new SFML.Graphics.Color(156,   0, 158)  // Violett
        };

        public SFML.Graphics.Color color { get; protected set; }

        //remove after manager exists
        Sprite sheepSprite;
        AnimatedSprite wheelSprite;

        Box2DX.Collision.Shape head;
        Box2DX.Collision.Shape wheel;
        Body chest;
        Vector2 rotationHead;

        //movement input and constant
        private float wheelToSheepRot = 0.0f; //basicly the rotation of the unicycle's frame
        public float accelerate;
        public float rotate;
        private float RotationFakt = 1f;
        //verstärkung der gegensteuerung
        public float Counterfactf = 10f;

        public float JumptimePassed;
        public bool jump = false;
        public bool isLoadingJump = false;
        public float jumpStrength = 0.0f;

        public float jumpLoadPercentage { get { return jumpStrength / Constants.maxJumpStrength; } }

        //state vars
        bool isOnGround;

        public bool isDead { get; private set; }

        public int playerIndex { get; private set; }
        public static int playerCount = 0;

        public PlayerCharacter(World _world, Vector2 _position)
            :base(_world, _position)
        {
            playerIndex = playerCount++;
            color = PlayerCharacter.Colors[playerIndex];
            accelerate = 0;
            rotate = 0;

            isDead = false;

            this.angVelocity = 0;
            //build the unicycle
            CircleDef circleDef = new CircleDef();
            circleDef.Radius = 1;
            circleDef.Density = 1f;
            circleDef.Friction = 1.0f;
            circleDef.Restitution = 0.0f;
            circleDef.LocalPosition.Set(0, 0);

            wheel = body.CreateShape(circleDef);
            body.SetMassFromShapes();
            body.SetUserData(this); // link body and this to register collisions in this

            //build the head and connect with the wheel
            BodyDef bodydef = new BodyDef();
            bodydef.Position = _position + new Vector2(0.0f, 3.5f);
            bodydef.Angle = 0f;

            chest = _world.CreateBody(bodydef);

            //add the head
            circleDef.Density = 0.0001f;
            circleDef.Radius = 0.75f;
            circleDef.LocalPosition.Set(0, 3);
            head = chest.CreateShape(circleDef);

            rotationHead = _position;

            PolygonDef Boxdef = new PolygonDef();
            Boxdef.SetAsBox(1, 1.5f);
            Boxdef.Density = 0.25f;
            Boxdef.Friction = 0.4f;
            Box2DX.Collision.Shape s2 = chest.CreateShape(Boxdef);
            chest.SetMassFromShapes();
            chest.SetUserData(this);

            //Jointshit
            RevoluteJointDef jointDefKW = new RevoluteJointDef();
            jointDefKW.Body2 = chest;
            jointDefKW.Body1 = body;
            jointDefKW.CollideConnected = false;
            jointDefKW.LocalAnchor2 = new Vector2(-0.0f, -3.8f);
            jointDefKW.LocalAnchor1 = new Vector2(0, 0);
            jointDefKW.EnableLimit = false;

            _world.CreateJoint(jointDefKW);

			// add visuals
            Texture wheelTexture = AssetManager.getTexture(AssetManager.TextureName.ShoopWheel);
            wheelSprite = new AnimatedSprite(wheelTexture, 1.0f, 1, (Vector2)wheelTexture.Size);
			wheelSprite.Scale = Constants.windowScaleFactor * new Vector2(0.2f, 0.2f); //(Vector2.One / (Vector2)wheelTexture.Size * 2F * circleDef.Radius).toScreenCoord() - Vector2.Zero.toScreenCoord();//new Vector2(0.08f, 0.08f);
            wheelSprite.Origin = ((Vector2)wheelSprite.spriteSize) / 2F;

            sheepSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.ShoopInfronUnicycle));
            sheepSprite.Origin = ((Vector2)sheepSprite.Texture.Size) / 2F;
			sheepSprite.Scale = Constants.windowScaleFactor *  new Vector2(_position.X > Constants.worldSizeX / 2F ? -0.2f : 0.2f, 0.2f);
        }

        public void Move()
        {
            if (isDead) return;

            if((this.angVelocity < 100) && this.accelerate == 1)
                this.angVelocity += RotationFakt;
            else if ((this.angVelocity > -100) && this.accelerate == -1)
                this.angVelocity -= RotationFakt;

			//perform a jump
            if (jump && /*isOnGround &&*/ jumpStrength > 0f)
            {
                Vector2 jumpDir = chest.GetWorldCenter() - body.GetWorldCenter();
                jumpDir.normalize(); jumpDir *= jumpStrength;
                body.ApplyImpulse(jumpDir, body.GetWorldCenter());
                chest.ApplyImpulse(jumpDir * 0.01f, chest.GetWorldCenter());
                jump = false;
                jumpStrength = 0f;
            }

			//balance if not in the air
            if (JumptimePassed < 0.66f /* && wantsToBalance == 0 */)
            {
                Console.WriteLine(JumptimePassed);
                float scalfact = 0;
                Vector2 theAngVec = chest.GetPosition() - body.GetPosition();
                theAngVec.normalize();
                scalfact = (float)Math.Acos(Math.Abs((double)theAngVec.X));
                //if(float.IsNaN(scalfact))
                //{
                //    Console.WriteLine(theAngVec.X);
                //}
                float angVel = chest.GetAngularVelocity();

                if (theAngVec.X > 0 && !float.IsNaN(scalfact))
                {
                    if (angVel < -2.5f)
                    {
                        chest.ApplyTorque(scalfact * 55 * Math.Abs(angVel * 12));
                    }
                    else
                        chest.ApplyTorque(scalfact * 55);

                }
                if (theAngVec.X < 0 && !float.IsNaN(scalfact))
                {
                    if (angVel > 2.5f)
                    {
                        chest.ApplyTorque(scalfact * -55 * Math.Abs(angVel * 12));
                    }
                    else
                        chest.ApplyTorque(scalfact * -55);
                }
            }

			// perform player controlled balancing
            if (rotate != 0)
            {
                //Vector2 targetVel = (Vector2)head.GetWorldCenter() - location;//optPos - headPos;
                //targetVel = wantsToBalance == 1 ? new Vector2(targetVel.X, -targetVel.Y) : new Vector2(-targetVel.X, targetVel.Y);
                //targetVel.normalize();
                Vector2 theAngVec = chest.GetPosition() - body.GetPosition();
                //Vector2 targetVec = wantsToBalance == 1 ? new Vector2 ((float) -(Math.Sin((double) head.GetAngle())),(float) Math.Cos((double) head.GetAngle())) : new Vector2((float)Math.Sin((double)head.GetAngle()), (float)-Math.Cos((double)head.GetAngle()));
                Vector2 targetVec = rotate > 0 ? new Vector2(-theAngVec.Y, theAngVec.X) : new Vector2(theAngVec.Y, -theAngVec.X);
                
                    targetVec.normalize();
                    float scalfact = (float) Math.Acos(Math.Abs((double)targetVec.X));

                //head.ApplyForce(targetVec * Counterfactf * scalfact, head.GetWorldCenter());
                
                chest.ApplyTorque(JumptimePassed < 0.5f ? 80 * scalfact * rotate : 65 * rotate);

                rotate = 0;
            }
        }


		public override void update(float _deltaTime)
        {
            base.update();

            Vector2 radius = (Vector2)chest.GetWorldCenter() - location;
            wheelToSheepRot = (float)System.Math.Atan2(radius.X, radius.Y) * Helper.RadianToDegree;

			if (isLoadingJump)
			{
				jumpStrength += Math.Min(0.8f * _deltaTime / 0.01667f, Constants.maxJumpStrength - jumpStrength);
			}
			JumptimePassed += _deltaTime;

        }


        public override void draw(RenderWindow win, View view)
        {
            Vector2 sheepLoc = chest.GetWorldCenter();

            sheepSprite.Position = sheepLoc.toScreenCoord();
            sheepSprite.Rotation = wheelToSheepRot;
            if (isDead)
                sheepSprite.Color = new SFML.Graphics.Color(color.R, color.G, color.B, 100);


            wheelSprite.Position = location.toScreenCoord();
            wheelSprite.Rotation = -body.GetAngle() * Helper.RadianToDegree;

            // draw in correct order, to have correct overlap
            win.Draw(wheelSprite);

            sheepSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.ShoopBehindUnicycle);
            win.Draw(sheepSprite);
            
            sheepSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.ShoopUnicycle);
            SFML.Graphics.Color c = sheepSprite.Color;
            if (!isDead)
                sheepSprite.Color = Helper.Lerp(color, SFML.Graphics.Color.White, 0.1F);
            win.Draw(sheepSprite);

            sheepSprite.Color = c;
            sheepSprite.Texture = AssetManager.getTexture(AssetManager.TextureName.ShoopInfronUnicycle);
            win.Draw(sheepSprite);

        //    DebugDraw(win);
        }

        private void DebugDraw(RenderWindow win)
        {
            SFML.Graphics.CircleShape wheel_Debug = new SFML.Graphics.CircleShape(Vector2.Zero.toScreenCoord().X - Vector2.One.toScreenCoord().X);
            wheel_Debug.Origin = Vector2.One * wheel_Debug.Radius;
            wheel_Debug.Position = ((Vector2)body.GetPosition()).toScreenCoord();
            wheel_Debug.FillColor = isDead ? SFML.Graphics.Color.Black : PlayerCharacter.Colors[playerIndex];
            win.Draw(wheel_Debug);

            //debugDraw for sheepBody
            SFML.Graphics.RectangleShape body_Debug = new SFML.Graphics.RectangleShape(new Vector2(2,3).toScreenCoord() - Vector2.Zero.toScreenCoord());
            body_Debug.Origin = (Vector2)body_Debug.Size / 2F;
            body_Debug.Position = ((Vector2)chest.GetPosition()).toScreenCoord();
            body_Debug.Rotation = -chest.GetAngle() * Helper.RadianToDegree;
            body_Debug.FillColor = isDead ? SFML.Graphics.Color.Black : PlayerCharacter.Colors[playerIndex];
            win.Draw(body_Debug);

            //the head
            SFML.Graphics.CircleShape headDeb = new SFML.Graphics.CircleShape(Vector2.Zero.toScreenCoord().X - (0.75f * Vector2.One).toScreenCoord().X);
            headDeb.Origin = Vector2.One * headDeb.Radius;
            headDeb.Position = ((Vector2)chest.GetPosition() + (new Vector2(0, 3)).rotate(chest.GetAngle())).toScreenCoord();
            headDeb.Rotation = -chest.GetAngle() * Helper.RadianToDegree;
            headDeb.FillColor = isDead ? SFML.Graphics.Color.Black : SFML.Graphics.Color.Red;

            win.Draw(headDeb);
        }

        // ********************************************************** //

        // ********************************************************** //

        Box2DX.Collision.Shape _lastContact;

        public void OnContact(Box2DX.Collision.Shape _other, Box2DX.Collision.Shape _self, ContactPoint _point)
        {
            if (wheel == _self)
            {
                _lastContact = _other;
                isOnGround = true;
                JumptimePassed = 0;
            }
            else if(head == _self && Physics.ContactManager.g_contactManager.isLethal(_other))
            {
                isDead = true;
            }
        }

        public void OnContactRemove(Box2DX.Collision.Shape _other, Box2DX.Collision.Shape _self, ContactPoint _point)
        {
      //      if (wheel == _self)
            {
                //only when the tile is left which was just hit
                if (_lastContact == _other)
                {
                    isOnGround = false;
                }
            }
        }

    }
}
