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
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var methodCallsProperty = property.FindPropertyRelative("calls");
            var baseHeight = (methodCallsProperty.arraySize * 38) + 20 + 20;
            return baseHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var rect = position;
            var methodCallsProperty = property.FindPropertyRelative("calls");
            GUI.Box(position, GUIContent.none);
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(position, label, EditorStyles.label);
            var orProperty = property.FindPropertyRelative("mode");
            position.x += 128;
            position.width -= 124;
            position.height = 16;
            EditorGUI.PropertyField(position, orProperty, GUIContent.none);
            position.x -= 128;
            position.width += 124;
            position.y += 18;
            var indent = EditorGUI.indentLevel;
            // EditorGUI.indentLevel = 0;

            position.height = 36;
            var ev = Event.current;
            var count = methodCallsProperty.arraySize;
            var deleteIndex = -1;
            position.xMax -= 24;
            for (var i = 0; i < count; i++)
            {
                EditorGUI.PropertyField(position, methodCallsProperty.GetArrayElementAtIndex(i));
                var button = new Rect(position.xMax + 4, -10 + position.y + position.height / 2, 20, 20);
                GUI.color = Color.red;
                if (GUI.Button(button, new GUIContent("", "Delete"), EditorStyles.radioButton))
                    deleteIndex = i;
                GUI.color = Color.white;
                position.y = position.yMax + 4;
            }
            if (deleteIndex >= 0)
            {
                methodCallsProperty.DeleteArrayElementAtIndex(deleteIndex);
                deleteIndex = -1;
                methodCallsProperty.serializedObject.ApplyModifiedProperties();
            }
            position.xMax += 24;

            EditorGUI.indentLevel = indent;
            DrawFooterButtons(rect, methodCallsProperty);
            var dropped = DropArea(rect);
            if (dropped != null)
            {

                EditorApplication.delayCall += () =>
                {
                    foreach (var i in dropped)
                    {
                        var idx = methodCallsProperty.arraySize;
                        methodCallsProperty.InsertArrayElementAtIndex(idx);
                        methodCallsProperty.serializedObject.ApplyModifiedProperties();
                        var p = methodCallsProperty.GetArrayElementAtIndex(idx);
                        p.FindPropertyRelative("gameObject").objectReferenceValue = i;
                    }
                };
            }
            EditorGUI.EndProperty();
        }

        void DrawFooterButtons(Rect position, SerializedProperty methodCalls)
        {
            position.x = position.xMax - 20;
            position.y = position.yMax - 20;
            position.height = 20;
            position.width = 20;
            GUI.color = Color.green;
            if (GUI.Button(position, new GUIContent("", "Click to add another call."), EditorStyles.radioButton))
            {
                EditorApplication.delayCall += () =>
                {
                    var idx = methodCalls.arraySize;
                    methodCalls.InsertArrayElementAtIndex(idx);
                    methodCalls.serializedObject.ApplyModifiedProperties();
                };
            }
            GUI.color = Color.white;
        }
    }
}
