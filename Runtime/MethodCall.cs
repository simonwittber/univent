using System;
using Surrogates;

namespace DifferentMethods.Univents
{

    [System.Serializable]
    public class MethodCall : Call<ISurrogateAction>
    {
        protected ISurrogateAction encapsulatedAction;

        public override void Invoke()
        {
            if (!enabled) return;
            if (encapsulatedMethodCall == null)
            {
                if (!LoadSurrogate())
                {
                    error = $"{metaMethodInfo.className} is null.";
                    enabled = false;
                    return;
                }
            }
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

        protected override void LoadEncapsulatedMethodCall()
        {
            encapsulatedMethodCall = SurrogateRegister.Instance.GetSurrogateAction(metaMethodInfo.GetMethodInfo());
        }
    }

}