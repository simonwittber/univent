using UnityEngine;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class OnTriggerStaySurrogate : MessageSurrogate
    {
        void OnTriggerStay(Collider other) => Fire();
    }
}