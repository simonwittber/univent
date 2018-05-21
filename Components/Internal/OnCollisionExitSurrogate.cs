using UnityEngine;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class OnCollisionExitSurrogate : MessageSurrogate
    {
        void OnCollisionExit(Collision collision) => Fire();
    }
}