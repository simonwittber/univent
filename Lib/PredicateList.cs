using System.Collections.Generic;
using UnityEngine;

namespace DifferentMethods.Univents
{
    [System.Serializable]
    public class PredicateList : CallList
    {
        public enum PredicateMode
        {
            SucceedIfAllAreTrue,
            SucceedIfAnyAreTrue,
            SucceedIfAllAreFalse,
            SucceedIfAnyAreFalse
        }
        public PredicateMode mode = PredicateMode.SucceedIfAnyAreTrue;

        [SerializeField] List<PredicateCall> calls = new List<PredicateCall>();

        public override IEnumerable<Call> GetCalls()
        {
            foreach (var i in calls) yield return i;
        }

        public bool Result { get; private set; }

        public override void Invoke()
        {
            switch (mode)
            {
                case PredicateMode.SucceedIfAnyAreTrue:
                    foreach (PredicateCall i in calls)
                    {
                        i.Invoke();
                        if (i.Result)
                        {
                            Result = true;
                            return;
                        }
                    }
                    Result = false;
                    break;
                case PredicateMode.SucceedIfAllAreTrue:
                    foreach (PredicateCall i in calls)
                    {
                        i.Invoke();
                        if (!i.Result)
                        {
                            Result = false;
                            return;
                        }
                    }
                    Result = true;
                    break;
                case PredicateMode.SucceedIfAllAreFalse:
                    foreach (PredicateCall i in calls)
                    {
                        i.Invoke();
                        if (i.Result)
                        {
                            Result = false;
                            return;
                        }
                    }
                    Result = true;
                    break;
                case PredicateMode.SucceedIfAnyAreFalse:
                    foreach (PredicateCall i in calls)
                    {
                        i.Invoke();
                        if (!i.Result)
                        {
                            Result = true;
                            return;
                        }
                    }
                    Result = false;
                    break;

            }
        }

    }

}