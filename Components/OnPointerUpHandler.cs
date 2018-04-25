using UnityEngine.EventSystems;

namespace DifferentMethods.Univents
{
    public class OnPointerUpHandler : UniventComponent, IPointerUpHandler
    {
        public ActionList onLeftButton, onRightButton, onMiddleButton;

        public void OnPointerUp(PointerEventData eventData)
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