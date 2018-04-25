namespace DifferentMethods.Univents
{
    public class Sequence : UniventComponent
    {
        public ActionList[] actionLists;

        [System.NonSerialized] int index = 0;

        void Update()
        {
            if (actionLists.Length > index)
                actionLists[index].Invoke();
            index = (index + 1) % actionLists.Length;
        }

    }
}