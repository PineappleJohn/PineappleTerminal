using PineappleMod.ConsoleCommands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PineappleMod.Console
{
    public class Parser
    {
        private readonly Dictionary<string, Namespace> _namespaces;

        public Parser(IEnumerable<Namespace> namespaces)
        {
            _namespaces = namespaces.ToDictionary(ns => ns.name);
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
            string commandName = tokens[1];
            string[] args = tokens.Skip(2).ToArray();

            if (!_namespaces.TryGetValue(nsName, out var ns)) return "Invalid source";

            if (!ns.GetCommands().TryGetValue(commandName, out var commandType)) return "Invalid source (CMD)";

            if (Activator.CreateInstance(commandType) is Command command)
            {
                command.Execute(args);
                return "Command found!";
            }
            return "No command found";
        }
    }

}
