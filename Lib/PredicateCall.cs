using System;

namespace DifferentMethods.Univents
{
    [System.Serializable]
    public class PredicateCall : Call<IEncapsulatedFunction<bool>>
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
                error = $"{metaMethodInfo.className} is missing.";
                enabled = false;
                Result = false;
                return;
            }
            else
            {
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
        }

        protected override void LoadEncapsulatedMethodCall()
        {
            encapsulatedMethodCall = ClassRegister.CreateInstance<IEncapsulatedFunction<bool>>(metaMethodInfo.className);
        }

    }



}