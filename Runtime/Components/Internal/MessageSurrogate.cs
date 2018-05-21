using UnityEngine;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class MessageSurrogate : MonoBehaviour
    {
        public System.Action onMessage;

        public void Fire()
        {
            if (onMessage != null) onMessage();
        }
    }
}