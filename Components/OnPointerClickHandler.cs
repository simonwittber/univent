using UnityEngine.EventSystems;

namespace DifferentMethods.Univents
{
    public class OnPointerClickHandler : UniventComponent, IPointerClickHandler
    {
        public ActionList onLeftButton, onRightButton, onMiddleButton;

        public void OnPointerClick(PointerEventData eventData)
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