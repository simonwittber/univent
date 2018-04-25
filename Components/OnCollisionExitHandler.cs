using UnityEngine;

namespace DifferentMethods.Univents
{
    public class OnCollisionExitHandler : UniventComponent
    {
        public LayerMask layerMask;
        public ActionList onCollisionExitEvent;
        int layerMaskValue;

        void Reset()
        {
            layerMask = LayerMask.NameToLayer("Everything");
        }

        void Awake()
        {
            layerMaskValue = layerMask.value;
        }

        void OnCollisionExit(Collision c)
        {
            if (0 != (layerMaskValue & 1 << c.transform.gameObject.layer))
                onCollisionExitEvent.Invoke();
        }
    }
}