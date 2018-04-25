namespace DifferentMethods.Univents
{
    public class OnDisableHandler : UniventComponent
    {
        public ActionList onDisableEvent;

        void OnDisable()
        {
            onDisableEvent.Invoke();
        }
    }


}