using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [NonSerialized] internal bool invoked = false;
        [NonSerialized] internal float nextCallTime = 0;
        [SerializeField] internal int count = 0;

        public override IEnumerable<Call> GetCalls()
        {
            foreach (var i in calls) yield return i;
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

        internal void InternalInvoke()
        {
            // Debug.Log("---------------" + this);
            foreach (var i in calls)
            {
                // Debug.Log(i);
                i.Invoke();
            }
            // Debug.Log("................" + this);
        }

    }

}