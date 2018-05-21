using UnityEngine;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class FixedUpdateSurrogate : MessageSurrogate
    {
        void FixedUpdate() => Fire();
    }
}