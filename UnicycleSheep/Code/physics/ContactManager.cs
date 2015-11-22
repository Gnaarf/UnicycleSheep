using Box2DX.Dynamics;
using System;
using System.Collections.Generic;
using UnicycleSheep;

namespace Physics
{
    class ContactManager : ContactListener
    {
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
                s2.OnContactRemove(point.Shape1, point.Shape1, point);
            }
        }
    }
}