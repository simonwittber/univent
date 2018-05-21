using System.Collections.Generic;
using UnityEngine;

namespace DifferentMethods.Univents
{
    [System.Serializable]
    public class CallList
    {

        [SerializeField] internal int selectedCallIndex = 0;
        public virtual void Invoke() { }

        public virtual IEnumerable<Call> GetCalls()
        {
            throw new System.NotImplementedException();
        }

    }

}