using System;
using System.Collections.Generic;

namespace PineappleMod.ConsoleCommands.Commands.Room
{
    public class RoomNamespace : Namespace
    {
        public override Dictionary<string, Type> GetCommands() => new Dictionary<string, Type>
       {
           { "disconnect", typeof(Disconnect) }
       };

        public override Command GetCommand(string name)
        {
            return base.GetCommand(name);
        }

        public override string GetNamespaceName()
        {
            return "Room";
        }
    }
}
