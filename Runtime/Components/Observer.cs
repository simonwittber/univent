using DifferentMethods.Univents.Internal;
using UnityEngine;

namespace DifferentMethods.Univents
{
    public partial class Observer : UniventMessageComponent
    {
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

        void Awake()
        {
            if (messages.HasFlag(ObservationTrigger.Update)) Add<UpdateSurrogate>();
            if (messages.HasFlag(ObservationTrigger.LateUpdate)) Add<LateUpdateSurrogate>();
            if (messages.HasFlag(ObservationTrigger.FixedUpdate)) Add<FixedUpdateSurrogate>();
            if (messages.HasFlag(ObservationTrigger.OnTriggerEnter)) Add<OnTriggerEnterSurrogate>();
            if (messages.HasFlag(ObservationTrigger.OnTriggerStay)) Add<OnTriggerStaySurrogate>();
            if (messages.HasFlag(ObservationTrigger.OnTriggerExit)) Add<OnTriggerExitSurrogate>();
            if (messages.HasFlag(ObservationTrigger.OnCollisionEnter)) Add<OnCollisionEnterSurrogate>();
            if (messages.HasFlag(ObservationTrigger.OnCollisionStay)) Add<OnCollisionStaySurrogate>();
            if (messages.HasFlag(ObservationTrigger.OnCollisionExit)) Add<OnCollisionExitSurrogate>();
            if (messages.HasFlag(ObservationTrigger.OnPointerEnter)) Add<OnPointerEnterSurrogate>();
            if (messages.HasFlag(ObservationTrigger.OnPointerClick)) Add<OnPointerClickSurrogate>();
            if (messages.HasFlag(ObservationTrigger.OnPointerExit)) Add<OnPointerExitSurrogate>();
        }

        public override void Trigger()
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