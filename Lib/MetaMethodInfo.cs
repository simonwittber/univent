using System.Linq;
using System.Reflection;

namespace DifferentMethods.Univents
{
    [System.Serializable]
    public class MetaMethodInfo
    {
        public string className;
        public string type;
        public string name;
        public string[] parameterTypeNames;
        public string niceName;
        public string returnType;

        public MethodInfo GetMethodInfo()
        {
            var componentType = System.Type.GetType(type);
            if (componentType == null) return null;
            var mi = componentType.GetMethod(name, (from i in parameterTypeNames select System.Type.GetType(i)).ToArray());
            return mi;
        }

    }



}