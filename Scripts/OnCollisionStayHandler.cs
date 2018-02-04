using UnityEngine;

namespace DifferentMethods.Univents
{
    public class OnCollisionStayHandler : UniventComponent
    {
        public LayerMask layerMask;
        public ActionList onCollisionStayEvent;
        int layerMaskValue;

        void Reset()
        {
            layerMask = LayerMask.NameToLayer("Everything");
        }

        void Awake()
        {
            layerMaskValue = layerMask.value;
        }

        void OnCollisionStay(Collision c)
        {
            if (0 != (layerMaskValue & 1 << c.transform.gameObject.layer))
                onCollisionStayEvent.Invoke();
        }

    }
}