using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DifferentMethods.Univents
{
    [CustomPropertyDrawer(typeof(PredicateList))]
    public class PredicateListDrawer : PropertyDrawer
    {
        [NonSerialized] ReorderableList list = null;
        [NonSerialized] SerializedProperty listProperty;
        [NonSerialized] GUIContent label;
        [NonSerialized] SerializedProperty property;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            var baseHeight = 48f;
            return baseHeight + property.FindPropertyRelative("calls").arraySize * (EditorGUIUtility.singleLineHeight + 4);
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
            list.DoList(position);
        }

        private void DrawHeader(Rect rect)
        {
            // EditorGUI.LabelField(rect, label);
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("mode"), label);
        }

        void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var r = new Rect(rect.x, rect.y + 2, rect.width - 20, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(r, listProperty.GetArrayElementAtIndex(index), GUIContent.none);
        }



    }
}

