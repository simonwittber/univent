using UnityEngine;
namespace DifferentMethods.Univents
{
    [RequireComponent(typeof(Renderer))]
    public class OnBecameVisibleHandler : UniventComponent
    {
        public ActionList onBecameVisibleEvent;

        void OnBecameVisible()
        {
            onBecameVisibleEvent.Invoke();
        }
    }


}