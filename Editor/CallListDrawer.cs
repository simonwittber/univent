using System;
using UnityEditor;

namespace DifferentMethods.Univents
{
    public class CallListDrawer : PropertyDrawer
    {
        [ThreadStatic] public static Call hotCall;
    }

}
