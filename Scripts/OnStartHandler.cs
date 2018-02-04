using UnityEngine;
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