using System.Collections.Generic;
using UnityEngine;

namespace DifferentMethods.Univents
{

    public class UniventScheduler : MonoBehaviour
    {
        PriorityQueue<ScheduledInvoke> tasks = new PriorityQueue<ScheduledInvoke>();
        Stack<ScheduledInvoke> pool = new Stack<ScheduledInvoke>();

        static UniventScheduler instance;
        public static UniventScheduler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UniventScheduler").AddComponent<UniventScheduler>();
                    instance.hideFlags = HideFlags.HideAndDontSave;
                }
                return instance;
            }
        }

        public void Schedule(float time, ActionList univent)
        {
            if (pool.Count == 0)
                tasks.Push(new ScheduledInvoke() { time = time, univent = univent });
            else
            {
                var si = pool.Pop();
                si.time = time;
                si.univent = univent;
                tasks.Push(si);
            }
        }

        void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(this);
            }
            else
            {
                instance = this;
            }
        }

        void Update()
        {
            while (tasks.Count > 0)
            {
                var si = tasks.Pop();
                if (si.time <= Time.time)
                {
                    si.univent.InternalInvoke();
                    si.univent = null;
                    pool.Push(si);
                }
                else
                {
                    tasks.Push(si);
                    break;
                }
            }
        }

    }

}