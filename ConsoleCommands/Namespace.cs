using System;
using System.Collections.Generic;
using UnityEngine;

namespace PineappleMod.ConsoleCommands
{
    public abstract class Namespace : MonoBehaviour
    {
        /// <summary>
        /// Add all commands to this namespace object in order to force Awake() to run, if you don't do this, the command will not run properly.
        /// </summary>
        public GameObject NamespaceObject => new GameObject(GetNamespaceName());
        /// <summary>
        /// The main dictionary to hold all commands for this namespace.
        /// </summary>
        public virtual Dictionary<string, Command> Commands => new Dictionary<string, Command>{
            
        };

        public static Namespace instance;
        public virtual void Awake()
        {
            instance = this;
        }

        public string Name => GetNamespaceName();
        /// <summary>
        /// The namespace name, this is typed in before the command.
        /// </summary>
        /// <returns>The namespaces name</returns>
        public abstract string GetNamespaceName();
        /// <summary>
        /// Generally you shouldn't need to touch this, it's just required for the parser to work properly.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Command> GetCommands() {
            return Commands;
        }
    }
}
