using System.Runtime.Serialization;
using UnityEngine;

namespace DifferentMethods.Univents
{

    public class Vector2Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
        {
            var v = (Vector2)obj;
            info.AddValue("x", v.x);
            info.AddValue("y", v.y);
        }

        public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var v = (Vector2)obj;
            v.x = (float)info.GetValue("x", typeof(float));
            v.y = (float)info.GetValue("y", typeof(float));
            obj = v;
            return obj;
        }
    }

    public class Vector3Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
        {
            var v = (Vector3)obj;
            info.AddValue("x", v.x);
            info.AddValue("y", v.y);
            info.AddValue("z", v.z);
        }

        public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var v = (Vector3)obj;
            v.x = (float)info.GetValue("x", typeof(float));
            v.y = (float)info.GetValue("y", typeof(float));
            v.z = (float)info.GetValue("z", typeof(float));
            return v;
        }
    }

    public class Vector4Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
        {
            var v = (Vector4)obj;
            info.AddValue("x", v.x);
            info.AddValue("y", v.y);
            info.AddValue("z", v.z);
            info.AddValue("w", v.w);
        }

        public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var v = (Vector4)obj;
            v.x = (float)info.GetValue("x", typeof(float));
            v.y = (float)info.GetValue("y", typeof(float));
            v.z = (float)info.GetValue("z", typeof(float));
            v.w = (float)info.GetValue("w", typeof(float));
            obj = v;
            return obj;
        }
    }

    public class ColorSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
        {
            var v = (Color)obj;
            info.AddValue("x", v.r);
            info.AddValue("y", v.g);
            info.AddValue("z", v.b);
            info.AddValue("w", v.a);
        }

        public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var v = (Color)obj;
            v.r = (float)info.GetValue("x", typeof(float));
            v.g = (float)info.GetValue("y", typeof(float));
            v.b = (float)info.GetValue("z", typeof(float));
            v.a = (float)info.GetValue("w", typeof(float));
            obj = v;
            return obj;
        }
    }

    public class QuaternionSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
        {
            var v = (Quaternion)obj;
            info.AddValue("x", v.x);
            info.AddValue("y", v.y);
            info.AddValue("z", v.z);
            info.AddValue("w", v.w);
        }

        public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var v = (Quaternion)obj;
            v.x = (float)info.GetValue("x", typeof(float));
            v.y = (float)info.GetValue("y", typeof(float));
            v.z = (float)info.GetValue("z", typeof(float));
            v.w = (float)info.GetValue("w", typeof(float));
            obj = v;
            return obj;
        }
    }

    public class LayerMaskSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
        {
            var v = ((LayerMask)obj).value;
            info.AddValue("x", v);
        }

        public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var v = (LayerMask)obj;
            v.value = (int)info.GetValue("x", typeof(int));
            obj = v;
            return obj;
        }
    }

    public class UnitySerializationSurrogateSelector
    {
        public static SurrogateSelector GetSurrogateSelector()
        {
            var surrogateSelector = new SurrogateSelector();
            surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), new Vector2Surrogate());
            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3Surrogate());
            surrogateSelector.AddSurrogate(typeof(Vector4), new StreamingContext(StreamingContextStates.All), new Vector4Surrogate());
            surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), new QuaternionSurrogate());
            surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), new ColorSurrogate());
            surrogateSelector.AddSurrogate(typeof(LayerMask), new StreamingContext(StreamingContextStates.All), new LayerMaskSurrogate());
            return surrogateSelector;
        }
    }
}