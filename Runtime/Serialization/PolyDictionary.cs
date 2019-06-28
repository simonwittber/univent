
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using DifferentMethods.Extensions.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace DifferentMethods.Univents
{
    [System.Serializable]
    public class PolyDictionary : ISerializationCallbackReceiver
    {

        [System.Serializable] class ObjectMap : SerializableDictionary<string, UnityEngine.Object> { }

        ObjectMap objects = new ObjectMap();
        Dictionary<string, object> things = new Dictionary<string, object>();
        [SerializeField] byte[] bytes;


        public T Get<T>(string name)
        {
            return (T)Get(name, typeof(T));
        }

        public object Get(string name, System.Type type)
        {
            object obj = null;
            if (type == typeof(UnityEngine.Object) || type.IsSubclassOf(typeof(UnityEngine.Object)))
                obj = objects.Get(name);
            else
                things.TryGetValue(name, out obj);

            if (obj == null && type.IsValueType)
                return System.Activator.CreateInstance(type);
            return obj;
        }

        public void Set(string name, object obj)
        {
            var type = obj.GetType();
            if (type == typeof(UnityEngine.Object) || type.IsSubclassOf(typeof(UnityEngine.Object)))
                objects.Set(name, obj);
            else
                things[name] = obj;
        }

        public void OnBeforeSerialize()
        {
            var bf = new BinaryFormatter(UnitySerializationSurrogateSelector.GetSurrogateSelector(), new StreamingContext());
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, things);
                bytes = ms.ToArray();
            }
        }

        public void OnAfterDeserialize()
        {
            if (bytes == null || bytes.Length == 0) return;
            var bf = new BinaryFormatter(UnitySerializationSurrogateSelector.GetSurrogateSelector(), new StreamingContext());
            using (var ms = new MemoryStream(bytes))
            {
                things = (Dictionary<string, object>)bf.Deserialize(ms);
            }
        }
    }

}