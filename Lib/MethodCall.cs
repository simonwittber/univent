using System;

namespace DifferentMethods.Univents
{

    [System.Serializable]
    public class MethodCall : Call<IEncapsulatedAction>
    {
        protected IEncapsulatedAction encapsulatedAction;

        public override void Invoke()
        {
            if (!enabled) return;
            if (encapsulatedMethodCall == null)
            {
                error = $"{metaMethodInfo.className} is null.";
                enabled = false;
            }
            else
            {
                try
                {
                    encapsulatedMethodCall.Invoke();
                }
                catch (MissingMethodException)
                {
                    error = $"{metaMethodInfo.niceName} is missing.";
                    enabled = false;
                }
            }
        }

        protected override void LoadEncapsulatedMethodCall()
        {
            encapsulatedMethodCall = ClassRegister.CreateInstance<IEncapsulatedAction>(metaMethodInfo.className);
        }
    }

}