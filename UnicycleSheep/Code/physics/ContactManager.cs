using Box2DX.Collision;
using Box2DX.Dynamics;
using System;
using System.Collections.Generic;
using UnicycleSheep;

namespace Physics
{
    class ContactManager : ContactListener
    {
        public static ContactManager g_contactManager = new ContactManager();

        public override void Add(ContactPoint point)
        {
            IContactEvent s1 = point.Shape1.GetBody().GetUserData() as IContactEvent;
            IContactEvent s2 = point.Shape2.GetBody().GetUserData() as IContactEvent;
            if (s1 != null)
            {
                s1.OnContact(point.Shape2, point.Shape1, point);
            }
            if (s2 != null)
            {
                s2.OnContact(point.Shape1, point.Shape2, point);
            }
        }

        public override void Remove(ContactPoint point)
        {
            IContactEvent s1 = point.Shape1.GetBody().GetUserData() as IContactEvent;
            IContactEvent s2 = point.Shape2.GetBody().GetUserData() as IContactEvent;
            if (s1 != null)
            {
                s1.OnContactRemove(point.Shape2, point.Shape1, point);
            }
            if (s2 != null)
            {
                s2.OnContactRemove(point.Shape1, point.Shape2, point);
            }
        }

        List<Shape> shapes = new List<Shape>();

        public void addNonLethalShape(Shape _shape)
        {
            shapes.Add(_shape);
        }

        public bool isLethal(Shape _shape)
        {
            return !shapes.Contains(_shape);
        }
    }
}