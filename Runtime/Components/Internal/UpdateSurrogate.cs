using UnityEngine;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class UpdateSurrogate : MessageSurrogate
    {
        void Update() => Fire();
    }
}