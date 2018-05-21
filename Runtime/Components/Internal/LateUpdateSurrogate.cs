using UnityEngine;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class LateUpdateSurrogate : MessageSurrogate
    {
        void LateUpdate() => Fire();
    }
}