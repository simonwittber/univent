using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DifferentMethods.Univents
{
    public class OnTriggerEnterHandler : UniventComponent
    {
        public LayerMask layerMask;
        public ActionList onTriggerEnterEvent;
        int layerMaskValue;

        void Reset()
        {
            layerMask = LayerMask.NameToLayer("Everything");
        }

        void Awake()
        {
            layerMaskValue = layerMask.value;
        }

        void OnTriggerEnter(Collider c)
        {
            if (0 != (layerMaskValue & 1 << c.transform.gameObject.layer))
                onTriggerEnterEvent.Invoke();
        }
    }


}