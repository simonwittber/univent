using UnityEngine;

namespace DifferentMethods.Univents
{
    public interface IEncapsulatedMethodCall
    {

    }

    public interface IEncapsulatedAction : IEncapsulatedMethodCall
    {
        void Invoke();
    }

    public interface IEncapsulatedFunction<T> : IEncapsulatedMethodCall
    {
        T Invoke();
    }

}