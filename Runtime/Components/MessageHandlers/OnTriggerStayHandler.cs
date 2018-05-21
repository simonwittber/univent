using UnityEngine;
namespace DifferentMethods.Univents
{
    public class OnTriggerStayHandler : UniventComponent
    {
        public LayerMask layerMask;
        public ActionList onTriggerStayEvent;
        int layerMaskValue;

        void Reset()
        {
            layerMask = LayerMask.NameToLayer("Everything");
        }

        void Awake()
        {
            layerMaskValue = layerMask.value;
        }

        void OnTriggerStay(Collider c)
        {
            if (0 != (layerMaskValue & 1 << c.transform.gameObject.layer))
                onTriggerStayEvent.Invoke();
        }
    }
}