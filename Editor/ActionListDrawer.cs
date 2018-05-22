using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DifferentMethods.Univents
{

    [CustomPropertyDrawer(typeof(ActionList), true)]
    public class ActionListDrawer : CallListDrawer
    {
        int hotIndex = 0;
        System.Action schedule;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var hotUnivent = (ActionList)fieldInfo.GetValue(property.serializedObject.targetObject);
            var methodCallsProperty = property.FindPropertyRelative("calls");
            var baseHeight = (methodCallsProperty.arraySize * 38) + 20 + 20;
            if (hotUnivent.showDetail)
                baseHeight += 116;
            return baseHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var rect = position;
            var hotUnivent = (ActionList)fieldInfo.GetValue(property.serializedObject.targetObject);
            var selectedMethodCall = property.FindPropertyRelative("selectedCallIndex");
            var methodCallsProperty = property.FindPropertyRelative("calls");
            hotIndex = selectedMethodCall.intValue;
            GUI.Box(position, GUIContent.none);

            DrawHeaderButtons(position, label, hotUnivent, methodCallsProperty);
            position.y += 36;
            if (hotUnivent.showDetail)
            {
                DrawDetailControls(position, property);
                position.y += 116;
            }
            EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            // EditorGUI.indentLevel = 0;

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
            // DrawFooterButtons(rect, methodCallsProperty);
            var dropped = DropArea(rect, "Drop GameObject here");
            if (dropped != null)
            {

                schedule += () =>
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

        void DrawFooterButtons(Rect position, SerializedProperty methodCalls)
        {
            position.x = position.xMax - 40;
            position.y = position.yMax - 20;
            position.height = 20;
            position.width = 20;
            GUI.color = Color.red;
            GUI.enabled = (hotIndex >= 0 && hotIndex <= (methodCalls.arraySize - 1));
            if (GUI.Button(position, new GUIContent("", "Click to remove selected call."), EditorStyles.radioButton))
            {
                if (hotIndex >= 0 && hotIndex <= (methodCalls.arraySize - 1))
                {
                    schedule += () => methodCalls.DeleteArrayElementAtIndex(hotIndex);
                }
            }
            GUI.enabled = true;
            position.x += position.width;
            GUI.color = Color.green;
            if (GUI.Button(position, new GUIContent("", "Click to add another call."), EditorStyles.radioButton))
            {
                schedule += () =>
                {
                    var idx = methodCalls.arraySize;
                    methodCalls.InsertArrayElementAtIndex(idx);
                    var p = methodCalls.GetArrayElementAtIndex(idx);
                    p.Reset();
                };
            }

            GUI.color = Color.white;

        }


        void DrawHeaderButtons(Rect position, GUIContent label, ActionList hotUnivent, SerializedProperty methodCalls)
        {
            position.height = 18;
            GUI.color = Color.white;
            EditorGUI.LabelField(position, label, EditorStyles.label);
            position.y += position.height;
            position.x += 9;
            hotUnivent.showDetail = EditorGUI.Foldout(position, hotUnivent.showDetail, new GUIContent("Options", "Click to view more options."));
            GUI.color = Color.white;

        }

    }

}
