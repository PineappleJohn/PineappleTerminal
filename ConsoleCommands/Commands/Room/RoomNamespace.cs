using PineappleMod.ConsoleCommands.Commands.Debug;
using System;
using System.Collections.Generic;

namespace PineappleMod.ConsoleCommands.Commands.Room
{
    public class RoomNamespace : Namespace
    {
        public Command GetDisconnect()
        {
            return DebugCommand.instance ?? NamespaceObject.AddComponent<DebugCommand>();
        }
        public override Dictionary<string, Command> Commands =>
            new Dictionary<string, Command>
            {
                { "disconnect", GetDisconnect() }
            };

        public override string GetNamespaceName()
        {
            return "Room";
        }
    }
}
