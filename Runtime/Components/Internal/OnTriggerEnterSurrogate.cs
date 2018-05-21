using UnityEngine;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class OnTriggerEnterSurrogate : MessageSurrogate
    {
        void OnTriggerEnter(Collider other) => Fire();
    }
}