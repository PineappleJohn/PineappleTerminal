using System;
using System.Collections.Generic;

namespace PineappleMod.ConsoleCommands.Commands.Room
{
    public class RoomNamespace : Namespace
    {
        public Disconnect disconnect => NamespaceObject.AddComponent<Disconnect>();
        public override Dictionary<string, Command> Commands =>
            new Dictionary<string, Command>
            {
                { "disconnect", Disconnect.instance }
            };

        public override string GetNamespaceName()
        {
            return "Room";
        }
    }
}
