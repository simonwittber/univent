using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using UnityEditorInternal;
using System.Linq;

namespace DifferentMethods.Univents
{

    [CustomPropertyDrawer(typeof(Call), true)]
    public class CallDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 32;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var error = property.FindPropertyRelative("error");
            if (Application.isPlaying && error.stringValue != "")
            {
                EditorGUI.HelpBox(position, error.stringValue, MessageType.Error);
                return;
            }
            GUI.Box(position, GUIContent.none);
            position.y += 1;
            position.x += 1;
            EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var gameObject = DrawGameObjectField(position, property);
            DrawMethodSelector(position, property, gameObject);
            position.y += 18;
            position.height = 16;
            var metaMethodInfoProperty = property.FindPropertyRelative("metaMethodInfo");
            position.x += 128;
            position.width -= 128;
            DrawFields(position, property, metaMethodInfoProperty);
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private void DrawFields(Rect position, SerializedProperty property, SerializedProperty metaMethodInfoProperty)
        {
            var componentTypeName = metaMethodInfoProperty.FindPropertyRelative("type").stringValue;
            var methodName = metaMethodInfoProperty.FindPropertyRelative("name").stringValue;
            var parameterTypeNames = metaMethodInfoProperty.FindPropertyRelative("parameterTypeNames");
            var parameterTypes = new List<System.Type>();
            for (var i = 0; i < parameterTypeNames.arraySize; i++)
            {
                var typeName = parameterTypeNames.GetArrayElementAtIndex(i).stringValue;
                parameterTypes.Add(System.Type.GetType(typeName));
            }
            var componentType = System.Type.GetType(componentTypeName);
            if (componentType == null) return;
            var mi = componentType.GetMethod(methodName, parameterTypes.ToArray());
            if (mi == null) return;
            var arguments = CallListDrawer.hotCall.arguments;
            GUI.Box(position, GUIContent.none);
            foreach (var p in mi.GetParameters())
            {
                var nameLabel = new GUIContent($"{p.Name}: ");
                GUI.Label(position, nameLabel);
                var size = GUI.skin.label.CalcSize(nameLabel);
                position.x += size.x;
                var obj = arguments.Get(p.Name, p.ParameterType);
                var newObj = obj;
                position.x += DrawFieldEditor(position, p.ParameterType, obj, out newObj).width + 2;
                if (obj != newObj)
                {
                    arguments.Set(p.Name, newObj);
                }
            }
        }

        void DrawMethodSelector(Rect position, SerializedProperty property, GameObject gameObject)
        {
            position.x += 128;
            position.width -= 128;
            position.height = 18;
            var content = new GUIContent(property.FindPropertyRelative("metaMethodInfo").FindPropertyRelative("niceName").stringValue);
            if (EditorGUI.DropdownButton(position, content, FocusType.Passive))
            {
                var menu = CreateMenu(gameObject, property);
                menu.DropDown(position);
            }
        }

        GameObject DrawGameObjectField(Rect position, SerializedProperty property)
        {
            position.height = 16;
            position.width = 128;
            var gameObjectProperty = property.FindPropertyRelative("gameObject");
            EditorGUI.PropertyField(position, gameObjectProperty, GUIContent.none);
            var componentProperty = property.FindPropertyRelative("component");
            var component = componentProperty.objectReferenceValue;
            var gameObject = gameObjectProperty.objectReferenceValue as GameObject;
            //make sure component is always child of the selected gameObject
            if (gameObject != null && component != null)
                componentProperty.objectReferenceValue = gameObject.GetComponent(component.GetType());
            return gameObject;
        }

        Rect DrawFieldEditor(Rect position, Type type, object obj, out object newObj)
        {
            var rect = position;
            EditorGUI.BeginChangeCheck();
            if (type == typeof(float))
            {
                rect.width = 32;
                obj = EditorGUI.FloatField(rect, (float)(obj)); ;
            }
            else if (type == typeof(int))
            {
                rect.width = 32;
                obj = EditorGUI.IntField(rect, (int)(obj));
            }
            else if (type == typeof(string))
            {
                rect.width = 96;
                obj = EditorGUI.TextField(rect, (string)obj);
            }
            else if (type == typeof(bool))
            {
                rect.width = 32;
                obj = EditorGUI.Toggle(rect, (bool)(obj));
            }
            else if (type.IsSubclassOf(typeof(System.Enum)))
            {
                rect.width = 96;
                obj = EditorGUI.EnumPopup(rect, (System.Enum)(obj));
            }
            else if (type == typeof(Vector3))
            {
                rect.width = 196;
                obj = EditorGUI.Vector3Field(rect, GUIContent.none, (Vector3)(obj));
            }
            else if (type == typeof(Vector2))
            {
                rect.width = 196 * 0.6666666f;
                obj = EditorGUI.Vector2Field(rect, GUIContent.none, (Vector2)(obj));
            }
            else if (type == typeof(Vector4))
            {
                rect.width = 196 * 1.3333333f;
                obj = EditorGUI.Vector4Field(rect, GUIContent.none, (Vector4)(obj));
            }
            else if (type == typeof(Color))
            {
                rect.width = 48;
                obj = EditorGUI.ColorField(rect, GUIContent.none, (Color)(obj));
            }
            else if (type == typeof(LayerMask))
            {
                rect.width = 96;
                obj = LayerMaskField(rect, (LayerMask)(obj));
            }
            else if (type.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                rect.width = 96;
                obj = EditorGUI.ObjectField(rect, GUIContent.none, (UnityEngine.Object)(obj), type, true);
            }
            if (EditorGUI.EndChangeCheck())
            {
            }
            newObj = obj;
            rect.x += rect.width;
            return rect;
        }


        public LayerMask LayerMaskField(Rect rect, LayerMask layerMask)
        {
            var layers = InternalEditorUtility.layers;
            var layerNumbers = new List<int>();
            layerNumbers.Clear();

            for (int i = 0; i < layers.Length; i++)
                layerNumbers.Add(LayerMask.NameToLayer(layers[i]));

            int maskWithoutEmpty = 0;
            for (int i = 0; i < layerNumbers.Count; i++)
            {
                if (((1 << layerNumbers[i]) & layerMask.value) > 0)
                    maskWithoutEmpty |= (1 << i);
            }

            maskWithoutEmpty = EditorGUI.MaskField(rect, GUIContent.none, maskWithoutEmpty, layers);

            int mask = 0;
            for (int i = 0; i < layerNumbers.Count; i++)
            {
                if ((maskWithoutEmpty & (1 << i)) != 0)
                    mask |= (1 << layerNumbers[i]);
            }
            layerMask.value = mask;

            return layerMask;
        }

        protected virtual GenericMenu CreateMenu(GameObject go, SerializedProperty property)
        {
            var menu = new GenericMenu();
            if (go != null)
            {

                var item = "GameObject/";
                foreach (var mi in typeof(GameObject).GetMethods().OrderBy(x => x.Name))
                {
                    if (IsSupportedMethod(mi, property))
                    {
                        menu.AddItem(new GUIContent(item + ClassRegister.GetNiceName(typeof(GameObject), mi)), false, AddCall(go, property, null, mi));
                    }
                }
                foreach (var c in go.GetComponents(typeof(Component)).OrderBy(x => x.GetType().Name))
                {
                    var ct = c.GetType();
                    item = $"{ct.Name}/";
                    foreach (var mi in ct.GetMethods().OrderBy(x => x.Name))
                    {
                        if (IsSupportedMethod(mi, property))
                        {
                            menu.AddItem(new GUIContent(item + ClassRegister.GetNiceName(ct, mi)), false, AddCall(go, property, c, mi));
                        }
                    }
                }

            }
            return menu;
        }

        protected virtual bool IsSupportedMethod(MethodInfo mi, SerializedProperty property)
        {
            if (CallListDrawer.hotCall.GetType() == typeof(MethodCall))
                return UniventCodeGenerator.IsSupportedMethod(mi);
            if (CallListDrawer.hotCall.GetType() == typeof(PredicateCall))
                return UniventCodeGenerator.IsSupportedFunction(mi, typeof(bool));
            return false;
        }

        string Signature(MethodInfo mi)
        {
            return string.Join(", ", (from i in mi.GetParameters() select i.Name));
        }

        protected static GenericMenu.MenuFunction AddCall(GameObject gameObject, SerializedProperty property, Component component, MethodInfo mi)
        {
            return () =>
            {
                var componentType = component == null ? typeof(GameObject) : component.GetType();
                property.FindPropertyRelative("component").objectReferenceValue = component;
                var metaMethodInfo = property.FindPropertyRelative("metaMethodInfo");
                metaMethodInfo.FindPropertyRelative("className").stringValue = ClassRegister.GetClassName(componentType, mi);
                metaMethodInfo.FindPropertyRelative("type").stringValue = componentType.AssemblyQualifiedName;
                metaMethodInfo.FindPropertyRelative("name").stringValue = mi.Name;
                metaMethodInfo.FindPropertyRelative("niceName").stringValue = ClassRegister.GetNiceName(mi);
                var typeNames = ClassRegister.GetParameterTypeNames(mi);
                var typeNamesProperty = metaMethodInfo.FindPropertyRelative("parameterTypeNames");
                typeNamesProperty.ClearArray();
                foreach (var typeName in typeNames)
                {
                    typeNamesProperty.InsertArrayElementAtIndex(typeNamesProperty.arraySize);
                    typeNamesProperty.GetArrayElementAtIndex(typeNamesProperty.arraySize - 1).stringValue = typeName;
                }
                property.serializedObject.ApplyModifiedProperties();
                UniventCodeGenerator.Instance.AddMethod(componentType, mi);
            };

        }

    }
}
