using System;
using System.Collections.Generic;
using UnityEngine;

namespace PineappleMod.ConsoleCommands
{
    public abstract class Namespace : MonoBehaviour
    {
        public string Name => GetNamespaceName();
        public abstract string GetNamespaceName();
        public abstract Dictionary<string, Type> GetCommands();

        public virtual Command GetCommand(string name) {
            return GetCommands().TryGetValue(name, out Type type)
                ? (Command)Activator.CreateInstance(type)
                : null;
        }
    }
}
