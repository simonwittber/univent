using DifferentMethods.Univents.Internal;
using UnityEngine;

namespace DifferentMethods.Univents
{
    public abstract class UniventMessageComponent : MonoBehaviour
    {
        [EnumFlags]
        public ObservationTrigger messages = ObservationTrigger.Update;

        protected void Add<T>() where T : MessageSurrogate
        {
            var c = gameObject.DefaultComponent<T>();
            c.onMessage += this.Trigger;
            c.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
        }

        public abstract void Trigger();

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
    }
}