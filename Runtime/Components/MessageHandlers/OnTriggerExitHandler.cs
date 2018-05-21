using UnityEngine;
namespace DifferentMethods.Univents
{
    public class OnTriggerExitHandler : UniventComponent
    {
        public LayerMask layerMask;
        public ActionList onTriggerExitEvent;
        int layerMaskValue;

        void Reset()
        {
            layerMask = LayerMask.NameToLayer("Everything");
        }

        void Awake()
        {
            layerMaskValue = layerMask.value;
        }

        void OnTriggerExit(Collider c)
        {
            if (0 != (layerMaskValue & 1 << c.transform.gameObject.layer))
                onTriggerExitEvent.Invoke();
        }
    }
}