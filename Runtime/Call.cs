using DifferentMethods.Extensions.Serialization;
using UnityEngine;

namespace DifferentMethods.Univents
{
    [System.Serializable]
    public class Call
    {
        public UnityEngine.GameObject gameObject;
        public Component component;
        public MetaMethodInfo metaMethodInfo;
        public PolySerializer arguments;
        public string error = "";
        protected bool enabled = true;

        public virtual void Invoke() { }

        public string NiceName()
        {
            var mi = metaMethodInfo.GetMethodInfo();
            if (mi == null) return "";
            var sb = new System.Text.StringBuilder();
            sb.Append(mi.DeclaringType.Name);
            sb.Append(".");
            sb.Append(mi.Name);
            sb.Append("(");
            var parameters = mi.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                // sb.Append(p.Name);
                // sb.Append(":");
                var arg = arguments.Get(p.Name, p.ParameterType);
                if (arg == null) arg = "None";
                sb.Append(arg.ToString());
                if (i < parameters.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(")");

            return sb.ToString();
        }

    }

    public abstract class Call<T> : Call, ISerializationCallbackReceiver where T : IEncapsulatedMethodCall
    {

        protected T encapsulatedMethodCall;
        protected abstract void LoadEncapsulatedMethodCall();

        public void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(metaMethodInfo.name))
            {
                enabled = false;
                return;
            }
            error = "";
            LoadEncapsulatedMethodCall();
            if (encapsulatedMethodCall == null)
            {
                error = $"{metaMethodInfo.niceName} could not be loaded, was it renamed or deleted?";
                enabled = false;
                return;
            }
            foreach (var fi in encapsulatedMethodCall.GetType().GetFields())
            {
                if (fi.Name == "__component" && fi.FieldType.IsSubclassOf(typeof(Component)) && component != null)
                {
                    fi.SetValue(encapsulatedMethodCall, component);
                }
                else if (fi.Name == "__component" && fi.FieldType == typeof(UnityEngine.GameObject))
                {
                    fi.SetValue(encapsulatedMethodCall, gameObject);
                }
                else
                {
                    fi.SetValue(encapsulatedMethodCall, arguments.Get(fi.Name, fi.FieldType));
                }
            }
        }

        public void OnBeforeSerialize()
        {

        }

    }

}