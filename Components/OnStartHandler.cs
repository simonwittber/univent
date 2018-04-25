using UnityEngine;
using UnityEngine.Events;

namespace DifferentMethods.Univents
{

    public class OnStartHandler : UniventComponent
    {
        public ActionList onStartEvent;

        void Start()
        {
            onStartEvent.Invoke();
        }
    }
}