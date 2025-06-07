using PineappleMod.Console.Command;
using PineappleMod.ConsoleCommands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PineappleMod.Console
{
    public class Parser
    {
        private readonly Dictionary<string, Namespace> _namespaces;
        /// <summary>
        /// Forces the namespace to init as its in a constructor, do not convert this to MonoBehaviour.
        /// </summary>
        public Parser()
        {
            _namespaces = CommandList.Namespaces;
        }

        /// <summary>  
        /// Parses the input string and executes the corresponding command.  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns>any error</returns>  
        public string ParseAndExecute(string input)
        {
            var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length <= 1) return "Invalid Token";

            string nsName = tokens[0];
            nsName.Trim();
            string commandName = tokens[1];
            string[] args = tokens.Skip(2).ToArray();

            if (!_namespaces.TryGetValue(nsName, out var ns)) return $"Invalid source {ns} : {nsName} : DICTIONARY <{_namespaces.Keys}, {_namespaces.Values}";

            if (!ns.GetCommands().TryGetValue(commandName, out var commandType)) return "No command found!";

            var command = ns.GetCommands()[commandName];

            command.Execute(args);
            return "Command found!";
        }
    }

}
