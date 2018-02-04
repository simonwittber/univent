using UnityEngine;

namespace DifferentMethods.Univents
{
    public class Observer : UniventComponent
    {
        public bool checkEveryFrame = false;
        public bool invertPredicateResult = false;
        public PredicateList predicate;
        [Space]
        public ActionList onSuccess, onFailure;

        bool lastResult;
        bool currentResult;
        bool firstResult = true;

        void OnEnable()
        {
            firstResult = true;
        }

        void Update()
        {
            if (checkEveryFrame) Trigger();
        }

        public void Trigger()
        {
            predicate.Invoke();
            currentResult = invertPredicateResult ? !predicate.Result : predicate.Result;
            if (firstResult || lastResult != currentResult)
            {
                if (currentResult)
                    onSuccess.Invoke();
                else
                    onFailure.Invoke();
            }
            firstResult = false;
            lastResult = currentResult;
        }

    }
}