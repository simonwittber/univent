using UnityEngine;
namespace DifferentMethods.Univents
{
    [RequireComponent(typeof(Renderer))]
    public class OnBecameInvisibleHandler : UniventComponent
    {
        public ActionList onBecameInvisibleEvent;

        void OnBecameVisible()
        {
            onBecameInvisibleEvent.Invoke();
        }
    }


}