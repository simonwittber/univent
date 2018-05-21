using UnityEngine;

namespace DifferentMethods.Univents
{
    public partial class Sequence : UniventMessageComponent
    {
        public ActionList actions;
        public override void Trigger() => actions.Invoke();
    }
}