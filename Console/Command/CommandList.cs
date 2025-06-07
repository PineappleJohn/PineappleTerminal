using PineappleMod.ConsoleCommands;
using PineappleMod.ConsoleCommands.Commands.Debug;
using PineappleMod.ConsoleCommands.Commands.Room;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PineappleMod.Console.Command
{
    public static class CommandList
    {
        private static readonly GameObject cmds = new GameObject("Console Commands");
        private static readonly DebuggingNamespace debugging = cmds.AddComponent<DebuggingNamespace>();
        private static readonly RoomNamespace room = cmds.AddComponent<RoomNamespace>();

        /// <summary>
        /// A dictionary of all namespaces, this is used to parse commands and execute them.
        /// </summary>
        public static Dictionary<string, Namespace> Namespaces = new Dictionary<string, Namespace>() {
                { debugging.GetNamespaceName(), debugging},
                { room.GetNamespaceName(), room }
           };
    }
}
