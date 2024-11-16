#if UNITY_EDITOR
using System;
using UnityEngine;

namespace SceneManagerToolkit
{

    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class RequiredAttribute : PropertyAttribute
    {

    }

}
#endif