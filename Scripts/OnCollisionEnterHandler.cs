using UnityEngine;

namespace DifferentMethods.Univents
{
    public class OnCollisionEnterHandler : UniventComponent
    {
        public LayerMask layerMask;
        public ActionList onCollisionEnterEvent;
        int layerMaskValue;

        void Reset()
        {
            layerMask = LayerMask.NameToLayer("Everything");
        }

        void Awake()
        {
            layerMaskValue = layerMask.value;
        }

        void OnCollisionEnter(Collision c)
        {
            if (0 != (layerMaskValue & 1 << c.transform.gameObject.layer))
                onCollisionEnterEvent.Invoke();
        }

    }
}