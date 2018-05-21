using UnityEngine;
using UnityEngine.EventSystems;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class OnPointerExitSurrogate : MessageSurrogate, IPointerExitHandler
    {
        public void OnPointerExit(PointerEventData eventData) => Fire();
    }
}