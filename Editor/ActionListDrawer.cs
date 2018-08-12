using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DifferentMethods.Univents
{

    [CustomPropertyDrawer(typeof(ActionList), true)]
    public class ActionListDrawer : CallListDrawer
    {
        System.Action schedule;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            var methodCallsProperty = property.FindPropertyRelative("calls");
            var baseHeight = (methodCallsProperty.arraySize * 38) + 20 + 24;
            var showDetail = property.FindPropertyRelative("showDetail").boolValue;
            if (showDetail)
                baseHeight += 116;
            return baseHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var rect = position;
            position.xMin += 18 * EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var methodCallsProperty = property.FindPropertyRelative("calls");


            if (methodCallsProperty.arraySize > 0)
                GUI.Box(position, GUIContent.none);

            DrawHeaderButtons(position, label, property);
            position.y += 16;
            if (property.FindPropertyRelative("showDetail").boolValue)
            {
                DrawDetailControls(position, property);
                position.y += 116;
            }
            EditorGUI.BeginProperty(position, label, property);
            // var indent = EditorGUI.indentLevel;
            // EditorGUI.indentLevel = 0;

            position.height = 36;
            var ev = Event.current;
            var count = methodCallsProperty.arraySize;
            position.xMax -= 24;
            var deleteIndex = -1;
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

        void DrawDetailControls(Rect position, SerializedProperty property)
        {
            position.height = 114;
            position.x += 20;
            position.width -= 23;
            GUI.Box(position, GUIContent.none);
            position.height = 16;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("invokeOnce"));
            position.y += position.height + 3;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("probability"));
            position.y += position.height + 3;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("cooldownPeriod"));
            position.y += position.height + 3;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("startDelay"));
            position.y += position.height + 3;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("repeatCount"));
            position.y += position.height + 3;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("repeatDelay"));
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
                var idx = methodCalls.arraySize;
                methodCalls.InsertArrayElementAtIndex(idx);
                methodCalls.serializedObject.ApplyModifiedProperties();
            }

            GUI.color = Color.white;

        }


        void DrawHeaderButtons(Rect position, GUIContent label, SerializedProperty property)
        {

            GUI.color = Color.white;
            if (property.FindPropertyRelative("calls").arraySize == 0)
            {
                EditorGUI.HelpBox(position, $"Drop Gameobjects here to setup {label.text}.", MessageType.Warning);
            }
            else
            {
                position.height = 18;
                label.text += " :";
                EditorGUI.LabelField(position, label, EditorStyles.label);
                position.x += position.xMax - 72;
                position.width = 72;
                var showDetail = property.FindPropertyRelative("showDetail");
                showDetail.boolValue = EditorGUI.Foldout(position, showDetail.boolValue, new GUIContent("Options", "Click to view more options."));
            }
            GUI.color = Color.white;

        }

    }

}
