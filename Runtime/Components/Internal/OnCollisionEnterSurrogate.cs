using UnityEngine;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class OnCollisionEnterSurrogate : MessageSurrogate
    {
        void OnCollisionEnter(Collision collision) => Fire();
    }
}