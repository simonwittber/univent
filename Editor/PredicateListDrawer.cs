using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DifferentMethods.Univents
{
    [CustomPropertyDrawer(typeof(PredicateList))]
    public class PredicateListDrawer : CallListDrawer
    {
        int hotIndex = 0;
        System.Action schedule;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var methodCallsProperty = property.FindPropertyRelative("calls");
            var baseHeight = (methodCallsProperty.arraySize * 38) + 20;
            return baseHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var hotUnivent = (CallList)fieldInfo.GetValue(property.serializedObject.targetObject);
            var selectedMethodCall = property.FindPropertyRelative("selectedCallIndex");
            var methodCallsProperty = property.FindPropertyRelative("calls");
            hotIndex = selectedMethodCall.intValue;
            GUI.Box(position, GUIContent.none);
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(position, label, EditorStyles.label);
            var orProperty = property.FindPropertyRelative("mode");
            position.x += 128;
            position.width -= 164;
            position.height = 16;
            EditorGUI.PropertyField(position, orProperty, GUIContent.none);
            position.x -= 128;
            position.width += 164;
            DrawHeaderButtons(position, methodCallsProperty);
            position.y += 18;
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            position.height = 36;
            var ev = Event.current;
            var count = methodCallsProperty.arraySize;
            for (var i = 0; i < count; i++)
            {
                if (ev.type == EventType.MouseDown && ev.button == 0 && position.Contains(ev.mousePosition))
                {
                    hotIndex = selectedMethodCall.intValue = i;
                }
                if (hotIndex == i)
                {
                    GUI.backgroundColor = Color.blue;
                    GUI.Box(position, GUIContent.none);
                    GUI.backgroundColor = Color.white;
                }
                var calls = hotUnivent.GetCalls().ToArray();
                hotCall = calls[i];
                EditorGUI.PropertyField(position, methodCallsProperty.GetArrayElementAtIndex(i));
                position.y += 38;
            }

            EditorGUI.indentLevel = indent;
            if (schedule != null)
            {
                schedule();
                schedule = null;
            }
            EditorGUI.EndProperty();
        }

        void DrawHeaderButtons(Rect position, SerializedProperty methodCalls)
        {
            position.x += (position.width - (18 * 3));
            position.width = 18;
            position.height = 17;
            position.x += position.width;
            if (GUI.Button(position, "+", EditorStyles.miniButtonLeft))
            {
                schedule += () =>
                {
                    var idx = methodCalls.arraySize;
                    methodCalls.InsertArrayElementAtIndex(idx);
                    var p = methodCalls.GetArrayElementAtIndex(idx);
                    p.Reset();
                };
            }
            position.x += position.width;
            if (GUI.Button(position, "-", EditorStyles.miniButtonRight))
            {
                if (hotIndex >= 0 && hotIndex <= (methodCalls.arraySize - 1))
                {
                    schedule += () => methodCalls.DeleteArrayElementAtIndex(hotIndex);
                }
            }
        }
    }

}
