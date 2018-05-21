using UnityEngine;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class OnCollisionStaySurrogate : MessageSurrogate
    {
        void OnCollisionStay(Collision collision) => Fire();
    }
}