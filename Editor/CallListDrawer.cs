using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DifferentMethods.Univents
{

    public class CallListDrawer : PropertyDrawer
    {
        // [ThreadStatic] public static Call hotCall;


        public UnityEngine.Object[] DropArea(Rect position)
        {
            var e = Event.current;
            switch (e.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!position.Contains(e.mousePosition))
                        return null;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (e.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        e.Use();
                        return DragAndDrop.objectReferences.ToArray();
                    }
                    break;
            }
            return null;
        }

    }
}