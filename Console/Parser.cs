using PineappleMod.Commands;
using PineappleMod.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PineappleMod.Console
{
    public class Parser
    {
        public Dictionary<string, Dictionary<string, Command>> _namespaces = new Dictionary<string, Dictionary<string, Command>> {
            {"debug", new Dictionary<string, Command>() {{
                    "test", new Commands.Debug()
            }}},
            {"Room", new Dictionary<string, Command>() {
                { "disconnect", new Commands.Room.Disconnect() },
                {"join", new Commands.Room.Join() },
                { "info", new Commands.RoomInfo() }
            }},
            {"Player", new Dictionary<string, Command>() {
                { "info", new Commands.PlayerInfo() },
                { "colour", new Commands.PlayerColour() },
                { "name", new Commands.PlayerColour() }
            }},
            {"Mods", new Dictionary<string, Command>() {
                { "check", new Commands.ModChecker() }
            }}
        };


        /// <summary>  
        /// Parses the input string and executes the corresponding command.  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns>The command output</returns>  
        public string ParseAndExecute(string input)
        {
            var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length <= 1) return "Invalid Token";

            string nsName = tokens[0].Trim();
            string commandName = tokens[1].Trim();
            string[] args = tokens.Skip(2).ToArray();

            if (!_namespaces.TryGetValue(nsName, out var ns))
                return $"Namespace '{nsName}' not found.";

            if (!ns.TryGetValue(commandName, out var command))
                return $"Command '{commandName}' not found in namespace '{nsName}'.";

            Logging.Info(command, command.GetCommandName());
            if (!command.Clear) ConsoleManager.Instance.dontClearThisCommand = true;
            command.BackgroundExecution(args);
            return $"$ {command.GetOutput().ToUpper()}";
        }
    }

    public abstract class Command : MonoBehaviour {
        public abstract void OnExecute(string[] args);
        public abstract string GetCommandName();
        public abstract string GetOutput();
        /// <summary>
        /// If true, the console will clear after this command is executed.
        /// </summary>
        public virtual bool Clear => true;
        public virtual int RequiredArgs => 0;

        public void BackgroundExecution(string[] args) {
            if (RequiredArgs != 0 && args.Length < RequiredArgs)
                throw new ArgumentException($"Expected at least {RequiredArgs} arguments, but only got {args.Length}.");
            OnExecute(args);
        }
    }
}
