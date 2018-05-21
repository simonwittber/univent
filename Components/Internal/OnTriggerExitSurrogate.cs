using UnityEngine;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class OnTriggerExitSurrogate : MessageSurrogate
    {
        void OnTriggerExit(Collider other) => Fire();
    }
}