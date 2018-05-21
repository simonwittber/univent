using UnityEngine.EventSystems;

namespace DifferentMethods.Univents
{
    public class OnPointerDownHandler : UniventComponent, IPointerDownHandler
    {
        public ActionList onLeftButton, onRightButton, onMiddleButton;

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    onLeftButton.Invoke();
                    break;
                case PointerEventData.InputButton.Right:
                    onRightButton.Invoke();
                    break;
                case PointerEventData.InputButton.Middle:
                    onMiddleButton.Invoke();
                    break;
            }
        }
    }
}