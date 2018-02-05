using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DifferentMethods.Univents
{

    [CustomPropertyDrawer(typeof(ActionList))]
    public class ActionListDrawer : CallListDrawer
    {
        int hotIndex = 0;
        System.Action schedule;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var hotUnivent = (ActionList)fieldInfo.GetValue(property.serializedObject.targetObject);
            var methodCallsProperty = property.FindPropertyRelative("calls");
            var baseHeight = (methodCallsProperty.arraySize * 38) + 20;
            if (hotUnivent.showDetail)
                baseHeight += 116;
            return baseHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var hotUnivent = (ActionList)fieldInfo.GetValue(property.serializedObject.targetObject);
            var selectedMethodCall = property.FindPropertyRelative("selectedCallIndex");
            var methodCallsProperty = property.FindPropertyRelative("calls");
            hotIndex = selectedMethodCall.intValue;
            GUI.Box(position, GUIContent.none);

            DrawHeaderButtons(position, label, hotUnivent, methodCallsProperty);
            position.y += 18;
            if (hotUnivent.showDetail)
            {
                DrawDetailControls(position, property);
                position.y += 116;
            }
            EditorGUI.BeginProperty(position, label, property);
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

        void DrawHeaderButtons(Rect position, GUIContent label, ActionList hotUnivent, SerializedProperty methodCalls)
        {
            var buttonX = position.x + (position.width - (18 * 3));
            position.width = 18;
            hotUnivent.showDetail = GUI.Toggle(position, hotUnivent.showDetail, "#", EditorStyles.miniButton);
            position.x += 18;
            position.width = buttonX - position.x;
            EditorGUI.LabelField(position, label, EditorStyles.label);
            position.width = 18;
            position.x = buttonX;
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
