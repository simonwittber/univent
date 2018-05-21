using UnityEngine;
using UnityEngine.EventSystems;

namespace DifferentMethods.Univents.Internal
{
    [AddComponentMenu("")]
    public class OnPointerClickSurrogate : MessageSurrogate, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData) => Fire();
    }
}