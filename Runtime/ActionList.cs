using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DifferentMethods.Univents
{

    [System.Serializable]
    public class ActionList : CallList
    {
        public bool invokeOnce;
        public float startDelay = 0;
        public float cooldownPeriod = 0;
        public float repeatDelay = 0;
        public int repeatCount = 0;
        public float probability = 1;
        public bool showDetail;
        [SerializeField] List<MethodCall> calls = new List<MethodCall>();
        [NonSerialized] bool invoked = false;
        [NonSerialized] float nextCallTime = 0;
        HashSet<Action> actions = new HashSet<Action>();

        public bool IsInvoked
        {
            get { return invoked; }
        }

        public void AddListener(Action fn)
        {
            actions.Add(fn);
        }

        public override IEnumerable<Call> GetCalls()
        {
            foreach (var i in calls) yield return i;
        }

        public void Reset()
        {
            invoked = false;
        }

        public override void Invoke()
        {
            if (invokeOnce && invoked) return;
            if (probability < 1)
            {
                if (UnityEngine.Random.value > probability)
                    return;
            }
            invoked = true;
            if (cooldownPeriod > 0 && Time.time < nextCallTime) return;
            nextCallTime = Time.time + cooldownPeriod;
            if (startDelay > 0)
                UniventScheduler.Instance.Schedule(Time.time + startDelay, this);
            else
                InternalInvoke();
            if (repeatCount > 0)
            {
                for (var i = 1; i < repeatCount; i++)
                    UniventScheduler.Instance.Schedule(Time.time + startDelay + (repeatDelay * i), this);
            }
        }

        internal virtual void InternalInvoke()
        {
            foreach (var i in calls)
                i.Invoke();
            foreach (var i in actions)
                i();
        }

    }

    [System.Serializable]
    public class ActionList<T> : ActionList
    {
        HashSet<UnityAction<T>> actions = new HashSet<UnityAction<T>>();

        protected T arg;

        public void Invoke(T arg)
        {
            this.arg = arg;
            base.Invoke();
        }

        internal override void InternalInvoke()
        {
            base.InternalInvoke();
            foreach (var i in actions) i(arg);
        }

        public void AddListener(UnityAction<T> fn)
        {
            actions.Add(fn);
        }

        public void RemoveListener(UnityAction<T> fn)
        {
            actions.Remove(fn);
        }
    }

}