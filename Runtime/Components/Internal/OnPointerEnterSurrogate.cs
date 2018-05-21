using UnityEngine;
using UnityEngine.EventSystems;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class OnPointerEnterSurrogate : MessageSurrogate, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData) => Fire();
    }
}