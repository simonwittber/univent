using System;
using Surrogates;

namespace DifferentMethods.Univents
{
    [System.Serializable]
    public class PredicateCall : Call<ISurrogateAction<bool>>
    {

        public bool Result
        {
            get; private set;
        }

        public override void Invoke()
        {
            if (!enabled)
            {
                Result = false;
                return;
            }
            if (encapsulatedMethodCall == null)
            {
                if (!LoadSurrogate())
                {
                    error = $"{metaMethodInfo.className} is missing.";
                    enabled = false;
                    Result = false;
                    return;
                }
            }
            try
            {
                Result = encapsulatedMethodCall.Invoke();
            }
            catch (MissingMethodException)
            {
                error = $"{metaMethodInfo.niceName} is missing.";
                enabled = false;
            }
        }

        protected override void LoadEncapsulatedMethodCall()
        {
            encapsulatedMethodCall = (ISurrogateAction<bool>)SurrogateRegister.Instance.GetSurrogateAction(metaMethodInfo.GetMethodInfo());
        }

    }



}