using Box2DX.Collision;
using Box2DX.Dynamics;

namespace Physics
{
    interface IContactEvent
    {
        void OnContact(Shape _other, Shape _self, ContactPoint _point);
        void OnContactRemove(Shape _other, Shape _self, ContactPoint _point);
    }
}
