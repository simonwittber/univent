using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DifferentMethods.Univents
{

    [CustomPropertyDrawer(typeof(ActionList))]
    public class ActionListDrawer : PropertyDrawer
    {
        [NonSerialized] ReorderableList list = null;
        [NonSerialized] SerializedProperty listProperty;
        [NonSerialized] GUIContent label;
        [NonSerialized] SerializedProperty property;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (list == null) return 0;
            return list.GetHeight();
        }

        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
        {
            this.property = property;
            if (list == null)
            {
                this.label = label;
                this.listProperty = property.FindPropertyRelative("calls");
                this.list = new ReorderableList(property.serializedObject, listProperty);
                list.drawElementCallback = DrawListElement;
                list.drawHeaderCallback = DrawHeader;
            }
            var showDetail = property.FindPropertyRelative("showDetail").boolValue;
            list.headerHeight = showDetail ? 18 + EditorGUIUtility.singleLineHeight * 7 : EditorGUIUtility.singleLineHeight;
            list.elementHeight = (EditorGUIUtility.singleLineHeight * 2) + 8;
            list.DoList(position);
        }

        private void DrawHeader(Rect rect)
        {
            var ox = rect.x;
            var showDetail = property.FindPropertyRelative("showDetail");
            GUI.Label(rect, label, EditorStyles.boldLabel);
            rect.x -= 6;
            showDetail.boolValue = EditorGUI.Foldout(rect, showDetail.boolValue, GUIContent.none);
            if (property.FindPropertyRelative("showDetail").boolValue)
            {
                rect.y += EditorGUIUtility.singleLineHeight;
                rect.x = ox;
                DrawDetailControls(rect);
            }
        }

        void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var prop = listProperty.GetArrayElementAtIndex(index);
            var height = EditorGUI.GetPropertyHeight(prop, GUIContent.none);
            var r = new Rect(rect.x, rect.y + 2, rect.width - 20, height);
            EditorGUI.PropertyField(r, prop, GUIContent.none);
        }

        void DrawDetailControls(Rect position)
        {
            // position.x += 8;
            position.width -= 16;

            position.height = EditorGUIUtility.singleLineHeight;
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


    }

}
