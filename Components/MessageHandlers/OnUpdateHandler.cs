using UnityEngine;

namespace DifferentMethods.Univents
{
    public class OnUpdateHandler : UniventComponent
    {
        public ActionList onUpdateEvent;

        void Update()
        {
            onUpdateEvent.Invoke();
        }
    }
}