namespace DifferentMethods.Univents
{
    public class OnEnableHandler : UniventComponent
    {
        public ActionList onEnableEvent;

        void OnEnable()
        {
            onEnableEvent.Invoke();
        }
    }


}